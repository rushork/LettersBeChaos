using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayPlaySound : MonoBehaviour
{
    
    public void PlayCloseAudio()
    {
        AudioManager.Instance.Play("DrawerOpen");
    }
}
