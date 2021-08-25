using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        // Updates the status bars
        if (usage <= 25) {
            barsActive = 0;
        } else if (usage > 75) {
            barsActive = 3;
        } else if (usage > 50) {
            barsActive = 2;
        } else {
            barsActive = 1;
        }

        // Renders either no sprite or a full bar sprite depending on how many bars are active
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


    // Sets usage of the current instance of stat.
    public void setUsage(int u) {
        usage = u;

        // Checks whether to load a victory/failure scene if the usage is >= 100
        if (usage >= 100 && PlayerPrefs.GetInt("Score") > 0) {
            if (PlayerPrefs.GetInt("Score") > 1000000) {
                PlayerPrefs.SetInt("Diamond", 1);
            }
            if (PlayerPrefs.GetInt("Score") > 100000) {
                PlayerPrefs.SetInt("Purple", 1);
            }
            if (PlayerPrefs.GetInt("Score") > 10000) {
                PlayerPrefs.SetInt("Gold", 1);
            }
            SceneManager.LoadScene(2); // Victory
        } else if (usage >= 100 && PlayerPrefs.GetInt("Score") < 0) {
            SceneManager.LoadScene(3); // Failure
        }

    }



}
