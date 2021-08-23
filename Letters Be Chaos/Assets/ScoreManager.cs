using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private int score = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void AddPoints(int amount)
    {
        score += amount;
        Debug.Log(score);
        PlayerPrefs.SetInt("Score", score);
    }

    public void RemovePoints()
    {

    }
}
