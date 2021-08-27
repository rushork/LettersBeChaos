using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{

    public TextMeshProUGUI score;


    // Self explanatory
    void Start() {
        score.SetText("Score: " + PlayerPrefs.GetInt("Score") + "\nHighscore: " + PlayerPrefs.GetInt("Highscore"));
        PlayerPrefs.SetInt("Score", 0);
    }
}
