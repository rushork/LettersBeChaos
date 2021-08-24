using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeButton : MonoBehaviour
{
    private Transform button;
    private Transform highlight;
    private SpriteRenderer colorZone;
    private Transform min;
    private Transform max;
    private bool followMouse;

    private void Awake()
    {
        button = transform.Find("button");
        colorZone = button.Find("Color").GetComponent<SpriteRenderer>();
        highlight = button.Find("highlight");
        min = transform.Find("Min");
        max = transform.Find("Max");
    }


    

    private void OnMouseEnter()
    {

        GameSettings.Instance.arm.isOverUI = true;
        highlight.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        GameSettings.Instance.arm.isOverUI = false;
        highlight.gameObject.SetActive(false);
    }

    private void OnMouseDrag()
    {
        button.transform.position = new Vector2(button.transform.position.x, Mathf.Clamp(Camera.main.ScreenToWorldPoint(Input.mousePosition).y, min.position.y, max.position.y));
        float value = (button.transform.position.y - min.position.y) / (max.position.y - min.position.y);

        if (AudioManager.Instance.masterVolume != value)
        {
            AudioManager.Instance.masterVolume = value;
            AudioManager.Instance.SetSoundLevel();
        }
        
        
        
    }
}
