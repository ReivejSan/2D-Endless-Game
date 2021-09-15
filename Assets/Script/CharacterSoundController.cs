using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundController : MonoBehaviour
{
    public AudioClip jump;
    public AudioClip scoreHighlight;

    private AudioSource _audioPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayJump()
    {
        _audioPlayer.PlayOneShot(jump);
    }

    public void PlayScoreHighlight()
    {
        _audioPlayer.PlayOneShot(scoreHighlight);
    }
}
