using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{

    public TextMeshProUGUI tmp;

    void Start() {
        tmp.SetText("Score: " + PlayerPrefs.GetInt("Score"));
        PlayerPrefs.SetInt("Score", 0);
    }
}
