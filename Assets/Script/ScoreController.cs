using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [Header("Score Highlight")]
    public int scoreHighlightRange;
    public CharacterSoundController sound;

    private int _lastScoreHighlight = 0;
    private int _currentScore = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        _currentScore = 0;
        _lastScoreHighlight = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetCurrentScore()
    {
        return _currentScore;
    }

    public void IncreaseCurrentScore(int increment)
    {
        _currentScore += increment;

        if(_currentScore - _lastScoreHighlight > scoreHighlightRange)
        {
            sound.PlayScoreHighlight();
            _lastScoreHighlight += scoreHighlightRange;
        }
    }

    public void FinishScoring()
    {
        if(_currentScore > ScoreData.highScore)
        {
            ScoreData.highScore = _currentScore;
        }
    }
}
