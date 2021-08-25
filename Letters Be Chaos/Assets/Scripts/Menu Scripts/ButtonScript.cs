using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public void OnMouseOver() {
        AudioManager.Instance.Play("Tick");
    }
}
