using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatControllerScript : MonoBehaviour
{

    private int numOfBars = 3;

    public int usage;
    public int barsActive;
    public GameObject[] bars;
    public Sprite barSprite;
    public Sprite emptyBarSprite;

    // Update is called once per frame
    void Update() {

        if (usage <= 25) {
            barsActive = 0;
        } else if (usage > 75) {
            barsActive = 3;
        } else if (usage > 50) {
            barsActive = 2;
        } else {
            barsActive = 1;
        }

        for (int i = 0; i < numOfBars; i++) {
            if (i < barsActive) {
                bars[i].GetComponent<SpriteRenderer>().sprite = barSprite;
            } else {
                bars[i].GetComponent<SpriteRenderer>().sprite = emptyBarSprite;
            }
        }  
    }

    public int getUsage() {
        return usage;    
    }

    public void setUsage(int u) {
        usage = u;
    }



}
