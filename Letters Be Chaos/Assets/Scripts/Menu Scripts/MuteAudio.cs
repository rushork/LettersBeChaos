using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteAudio : MonoBehaviour
{

    public AudioSource sound;

    public void Skipped() {
        sound.Stop();
        Destroy(this.gameObject);
    }
}
