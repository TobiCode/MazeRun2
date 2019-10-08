using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManagerScript : MonoBehaviour
{
    public Text Mute1;
    public Text Mute2;
    private bool mute;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
