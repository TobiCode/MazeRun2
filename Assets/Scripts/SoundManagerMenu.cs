using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManagerMenu : MonoBehaviour
{
    public Text Mute1;
    public Text Mute2;
    private bool mute;
    public AudioSource audioSource;
    private AudioClip[] soundClips;


    // Start is called before the first frame update
    void Start()
    {
        soundClips = Resources.LoadAll<AudioClip>("SoundSave");
        int randomNumber = Random.RandomRange(0, soundClips.Length - 1);
        AudioClip toPlay = soundClips[randomNumber];
        audioSource.clip = toPlay;
        Audio();
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource.isPlaying == false)
        {
            int randomNumber = Random.RandomRange(0, soundClips.Length - 1);
            AudioClip toPlay = soundClips[randomNumber];
            audioSource.clip = toPlay;
            Audio();
        }
    }

    public void MuteUnmute()
    {
        if (!mute)
        {
            Mute1.text = "Unmute";
            Mute2.text = "Unmute";
            AudioListener.pause = true;
            mute = true;
        }
        else
        {
            Mute1.text = "Mute";
            Mute2.text = "Mute";
            AudioListener.pause = false;
            mute = false;
        }
    }

    private void Audio()
    {
        if (audioSource.isPlaying == false)
        {
            //Audio delay between each 
            audioSource.Play();
        }
    }
}
