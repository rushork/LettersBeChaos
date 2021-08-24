using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance { get; private set; }


    public Sound[] sounds; 

    void Awake()
    {
        Instance = this;

        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
        }
    }

    public void Play (string name) {
        foreach (Sound s in sounds) {
            if (s.name == name) {
                s.source.Play();
            }
        }
    }

    public string returnIncorrect(int top) {
        int random = Random.Range (0, top) + 1;
        if (random == top) {
            return "Incorrect";
        } else {
            return "IncorrectBeep";
        }
    }
}
