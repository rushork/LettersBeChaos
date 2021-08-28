using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBackgroundLetter : MonoBehaviour
{
    

    public void SetSprite(Sprite sprite)
    {
        GetComponent<Image>().sprite = sprite;
    }

    private void Update()
    {
        if(transform.position.y <= -440)
        {
            Destroy(gameObject);
        }
    }
}
