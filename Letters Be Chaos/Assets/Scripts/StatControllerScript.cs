using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatControllerScript : MonoBehaviour
{

    private int numOfBars = 3;

    public int usage;
    public int barsActive;
    public SpriteRenderer[] bars;
    public Sprite barSprite;
    public Sprite emptyBarSprite;
    private Animator[] animators;
    public Animator b1;
    public Animator b2;
    public Animator b3;
    private bool sync;

    private void Start()
    {
        //im lazy
        animators = new Animator[3];

        animators[0] = b1;
        animators[1] = b2;
        animators[2] = b3;
    }

    // Update is called once per frame
    void Update() {

        // Updates the status bars
        if (usage <= 25) {
            barsActive = 0;
        } else if (usage >= 75) {
            barsActive = 3;
        } else if (usage >= 50) {
            barsActive = 2;
        } else {
            barsActive = 1;
        }

        // Renders either no sprite or a full bar sprite depending on how many bars are active
        for (int i = 0; i < numOfBars; i++) {
            if (i < barsActive) {
                if (bars[i].sprite != barSprite) {
                    sync = true;
                    AudioManager.Instance.Play("BarAppear");
                }
                bars[i].sprite = barSprite;

                if (i == (barsActive - 1))
                {
                    animators[i].SetBool("Flashing", true);
                }
                else
                {
                    animators[i].SetBool("Flashing", false);
                }
            } else {
                bars[i].sprite = emptyBarSprite;
                animators[i].SetBool("Flashing", false);
            }
        }

        if (sync)
        {
            for (int i = 0; i < numOfBars; i++)
            {
                if (i < barsActive)
                {
                    if (animators[i].GetBool("Flashing"))
                    {
                        animators[i].SetTrigger("Reset");
                    }
                }
                
            }

            sync = false;
        }
    }

    public int getUsage() {
        return usage;    
    }


    // Sets usage of the current instance of stat.
    public void setUsage(int u) {
        usage = u;

        // Checks whether to load a victory/failure scene if the usage is >= 100
        if (usage >= 100) {
            GameSettings.Instance.ExitGame();
        }

    }



}
