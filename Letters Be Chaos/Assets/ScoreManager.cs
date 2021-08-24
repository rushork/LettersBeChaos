using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] TextMeshProUGUI textTemp;
    public Transform pointsPopupPrefab;
    private int score = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void AddPoints(int amount)
    {
        score += amount;
        textTemp.text = score.ToString();
        PlayerPrefs.SetInt("Score", score);
    }

   
}
