using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampIconMouseFollow : MonoBehaviour
{
    

    void Update()
    {
        transform.position = new Vector2(Mathf.Clamp(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, -2.25f, 2.25f), Mathf.Clamp(Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1.1f, 0.6f));
    }
}
