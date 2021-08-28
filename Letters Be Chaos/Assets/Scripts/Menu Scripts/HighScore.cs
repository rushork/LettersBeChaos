using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScore : MonoBehaviour
{

    public TextMeshProUGUI highscore;

    void Start()
    {
        if (PlayerPrefs.GetInt("Highscore") > 0) {
            highscore.SetText("Highscore: " + PlayerPrefs.GetInt("Highscore"));
        } else {
            highscore.SetText("Highscore: 0");
        }
    }

}
