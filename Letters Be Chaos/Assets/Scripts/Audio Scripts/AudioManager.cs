using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance { get; private set; }

    [HideInInspector]public float masterVolume = 1f;
    private AudioSource backgroundTheme;
    public Sound[] sounds; 

    void Awake()
    {
        
        Instance = this;

        backgroundTheme = GetComponent<AudioSource>();
        backgroundTheme.volume = masterVolume;
        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume * masterVolume;
            s.source.pitch = s.speed;
        }
    }

    public void SetSoundLevel()
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume * masterVolume;
        }
        backgroundTheme.volume = masterVolume;
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
            Play("IncorrectBeep");
            StartCoroutine(soundWaiter(1, "Incorrect"));
            return null;
        } else {
            return "IncorrectBeep";
        }
    }

    IEnumerator soundWaiter(int time, string name) {
        yield return new WaitForSeconds(time);
        Play(name);
    }

}
