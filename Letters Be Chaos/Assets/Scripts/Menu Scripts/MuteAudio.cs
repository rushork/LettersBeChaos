using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteAudio : MonoBehaviour
{

    public AudioSource sound;

    public void Skipped()
    {
        if (sound.volume == 1)
        {
            sound.volume = 0;


        }
        else
        {
            sound.volume = 1;
        }
    }
    }
