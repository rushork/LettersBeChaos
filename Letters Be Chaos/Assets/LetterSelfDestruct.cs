using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LetterSelfDestruct : MonoBehaviour
{
    public float DestructionTimerMax;
    public TextMeshPro text;
    public Transform animPrefab;


    private void Update()
    {
        DestructionTimerMax -= Time.deltaTime;
        text.text = DestructionTimerMax.ToString("F0");
        if(DestructionTimerMax < 0)
        {
            //play an animation or something here.
            SpriteRenderer renderer = Instantiate(animPrefab, transform.position, transform.rotation).transform.Find("Letter").GetComponent<SpriteRenderer>();
            renderer.sprite = this.transform.Find("Letter").GetComponent<SpriteRenderer>().sprite;
            Destroy(gameObject);
            
        }
    }
}
