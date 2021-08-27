using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableHighlightFlash : MonoBehaviour
{
    private SpriteRenderer myRenderer;
    private Color myOriginalColor;
    private Color myNewColor;
    private bool increasing;
    private bool decreasing;
  


    private void Awake()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        myOriginalColor = myRenderer.color;
        
    }

    private void Start()
    {
       
    }

    private void Update()
    {

        if(myRenderer.color.a >= 1f)
        {
            decreasing = true;
            increasing = false;
        }
        else if (myRenderer.color.a <= 0f)
        {
            increasing = true;
            decreasing = false;
        }

        

        if (decreasing)
        {
            myOriginalColor.a -= (3 * Time.deltaTime);
        }
        else if (increasing)
        {
            myOriginalColor.a += (3 * Time.deltaTime);
        }
        myRenderer.color = myOriginalColor;

    }

    private void OnDisable()
    {
        myOriginalColor.a = 1f;
        myRenderer.color = myOriginalColor;
    }


}
