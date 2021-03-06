using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] TextMeshProUGUI textTemp;
    private Animator textAnim;
    public Transform pointsPopupPrefab;
    private int score = 0;

    private void Awake()
    {
        Instance = this;
        textAnim = textTemp.GetComponent<Animator>();
    }


    public void AddPoints(int amount)
    {
        score += amount;
        textAnim.SetTrigger("AddPoints");
        if(score > 0 && score < 10000)
        {
            textTemp.text = "<color=#31A62E>" + score.ToString() + "</color>";
        }
        else if (score >= 10000 && score < 50000)
        {
            textTemp.text = "<color=#e3961b>" + score.ToString() + "</color>";
        }
        else if (score >= 50000)
        {
            textTemp.text = "<color=#7b2cc9>" + score.ToString() + "</color>";
        }
        else
        {
            textTemp.text = "<color=#B03E3E>" + score.ToString() + "</color>";
        }
        
        PlayerPrefs.SetInt("Score", score);
    }

    public int GetScore()
    {
        return score;
    }

   
}
