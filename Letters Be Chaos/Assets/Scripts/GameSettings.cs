using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class will hold all information on the game settings, stuff that other scripts will need access to.
 */
public class GameSettings : MonoBehaviour
{

    public static GameSettings Instance { get; private set; }

    public StampingArm arm;


    [Header("Usage Stats")]
    public StatControllerScript CPU;
    public StatControllerScript RAM;
    public StatControllerScript HYDRAULIC;


    [Header("Letter Type Prefabs")]
    public Transform letterPrefab_FirstClass;
    public Transform pfPointsText;

    [Header("Letter Colors: Decided by random chance")]
    [SerializeField] public Color32 red;
    [SerializeField] public Color32 blue;
    [SerializeField] public Color32 green;
    [SerializeField] public Color32 delete;

    [Header("Areas letters are sent during processing")]
    [SerializeField] public Transform redLocation;
    [SerializeField] public Transform greenLocation;
    [SerializeField] public Transform blueLocation;
    [SerializeField] public Transform deleteLocation;

    [Header("Letter color chances")]
    [Range(0,1)] public float redChance;
    [Range(0, 1)] public float greenChance;
    [Range(0, 1)] public float blueChance;

    
    private void Awake()
    {
        Application.targetFrameRate = 165;
        //this
        //now you can use "GameSettings.Instance.(insert method name)" anywhere. 
        //Why use this instead of static classes? static classes are a pain in the ass.
        Instance = this;
    }

    public void ProcessLetter(InteractableLetter letter)
    {
        int points = 0;

        //if orange stamp is applied to invalid letter
        if (!letter.isValid)
        {
            if (letter.isCorrectColor)
            {
                points += 5;
            }
        }

        if (!letter.isPostageStamped)
        {
            points -= 3;
        }
        if (!letter.isSealed)
        {
            points -= 7;
        }
        if (!letter.isUpsideDown)
        {
            points -= 1;
        }

        if (letter.isCorrectColor && letter.isValid)
        {
            FindObjectOfType<AudioManager>().Play("Point");
            if (letter.GetSealColor().Equals(red))
            {
                points += 5;
            }
            else if (letter.GetSealColor().Equals(blue))
            {
                points += 2;
            }
            else if (letter.GetSealColor().Equals(green))
            {
                points += 1;

            }
        }

        ScoreManager.Instance.AddPoints(points);
        Destroy(letter.gameObject);
    }
}
