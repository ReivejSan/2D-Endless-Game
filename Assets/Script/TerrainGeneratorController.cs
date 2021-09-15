using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneratorController : MonoBehaviour
{
    [Header("Templates")]
    public List<TerrainTemplateController> terrainTemplates;
    public float terrainTemplateWidth;

    [Header("Generator Area")]
    public Camera gameCamera;
    public float areaStartOffset;
    public float areaEndOffset;


    [Header("Force Early Template")]
    public List<TerrainTemplateController> earlyTerrainTemplates;

    private const float _debugLineHeight = 10.0f;

    private List<GameObject> _spawnedTerrain;
    private float _lastGeneratedPositionX;
    private float _lastRemovedPositionX;

    private Dictionary<string, List<GameObject>> pool;

    private void Start()
    {
        pool = new Dictionary<string, List<GameObject>>();
        
        _spawnedTerrain = new List<GameObject>();

        _lastGeneratedPositionX = GetHorizontalPositionEnd();
        _lastRemovedPositionX = _lastGeneratedPositionX = terrainTemplateWidth;

        foreach(TerrainTemplateController terrain in earlyTerrainTemplates)
        {
            GenerateTerrain(_lastGeneratedPositionX, terrain);
            _lastGeneratedPositionX += terrainTemplateWidth;
        }

        while(_lastGeneratedPositionX < GetHorizontalPositionEnd())
        {
            GenerateTerrain(_lastGeneratedPositionX);
            _lastGeneratedPositionX += terrainTemplateWidth;
        }
    }

    private void Update()
    {
        while(_lastGeneratedPositionX < GetHorizontalPositionEnd())
        {
            GenerateTerrain(_lastGeneratedPositionX);
            _lastGeneratedPositionX += terrainTemplateWidth;
        }

        while(_lastRemovedPositionX + terrainTemplateWidth < GetHorizontalPositionStart())
        {
            _lastRemovedPositionX += terrainTemplateWidth;
            RemoveTerrain(_lastRemovedPositionX);
        }
    }

    private void GenerateTerrain(float posX, TerrainTemplateController forceTerrain = null)
    {
        GameObject newTerrain = Instantiate(terrainTemplates[Random.Range(0, terrainTemplates.Count)].gameObject, transform);

        newTerrain.transform.position = new Vector2(posX, 0f);

        _spawnedTerrain.Add(newTerrain);
    }

    private void RemoveTerrain(float posX)
    {
        GameObject terrainToRemove = null;

        foreach(GameObject item in _spawnedTerrain)
        {
            if(item.transform.position.x == posX)
            {
                terrainToRemove = item;
                break;
            }
        }

        if(terrainToRemove != null)
        {
            _spawnedTerrain.Remove(terrainToRemove);
            Destroy(terrainToRemove);
        }
    }

    private float GetHorizontalPositionStart()
    {
        return gameCamera.ViewportToWorldPoint(new Vector2(0f, 0f)).x + areaStartOffset;
    }

    private float GetHorizontalPositionEnd()
    {
        return gameCamera.ViewportToWorldPoint(new Vector2(1f, 0f)).x + areaEndOffset;
    }

    private void OnDrawGizmos()
    {
        Vector3 areaStartPosition = transform.position;
        Vector3 areaEndPosition = transform.position;

        areaStartPosition.x = GetHorizontalPositionStart();
        areaEndPosition.x = GetHorizontalPositionEnd();

        Debug.DrawLine(areaStartPosition + Vector3.up * _debugLineHeight / 2, areaStartPosition + Vector3.down * _debugLineHeight / 2, Color.red);
        Debug.DrawLine(areaEndPosition + Vector3.up * _debugLineHeight / 2, areaEndPosition + Vector3.down * _debugLineHeight / 2, Color.red);
    }

    private GameObject GenerateFromPool(GameObject item, Transform parent)
    {
        if (pool.ContainsKey(item.name))
        {
            if (pool[item.name].Count > 0)
            {
                GameObject newItemFromPool = pool[item.name][0];
                pool[item.name].Remove(newItemFromPool);
                newItemFromPool.SetActive(true);
                return newItemFromPool;
            }
        }
        else
        {
            pool.Add(item.name, new List<GameObject>());
        }

        GameObject newItem = Instantiate(item, parent);
        newItem.name = item.name;
        return newItem;
    }

    private void ReturnToPool(GameObject item)
    {
        if (!pool.ContainsKey(item.name))
        {
            Debug.LogError("INVALID POOL ITEM!");
        }

        pool[item.name].Add(item);
        item.SetActive(false);
    }
}
