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

    /// <summary>
    /// Sort the letter and assign points.
    /// </summary>
    /// <param name="letter"></param>
    public void ProcessLetter(InteractableLetter letter)
    {
        int points = 0;
        string debugMessage = "";

        //if the letter is upright, with a valid color seal (rgb) and has a stamp:
        if (letter.isValidOnArrival)
        {
            
            //if the letter was correctly stamped by the player
            if (letter.isCorrectColor)
            {
                //if the seal is red:
                if (letter.GetSealColor().Equals(red))
                {
                    debugMessage += " Was correct color: red";
                    points += 50;
                }
                //if the seal is blue:
                else if (letter.GetSealColor().Equals(blue))
                {
                    debugMessage += " Was correct color: blue";
                    points += 20;
                }
                //if the seal is green:
                else if (letter.GetSealColor().Equals(green))
                {
                    debugMessage += " Was correct color: green";
                    points += 10;

                }
                //if the seal is orange, i.e. marked for reprocessing
                else if (letter.GetSealColor().Equals(delete))
                {
                    debugMessage += " Was correct color: Orange";
                    //this would lose you points, but its impossible for the code to get here.
                }
                else
                {
                    debugMessage += " Unknown Error, no color.";
                }

                FindObjectOfType<AudioManager>().Play("Point");
            }
            else
            {
                //if it wasnt
                if (letter.GetSealColor().Equals(red))
                {
                    debugMessage += " Was incorrect color: red";
                    points -= 50;
                }
                else if (letter.GetSealColor().Equals(blue))
                {
                    debugMessage += " Was incorrect color: blue";
                    points -= 20;
                }
                else if (letter.GetSealColor().Equals(green))
                {
                    debugMessage += " Was incorrect color: green";
                    points -= 10;

                }
                else if (letter.GetSealColor().Equals(delete))
                {
                    //if the player marks a valid letter for deletion
                    debugMessage += " Was incorrectly marked for deletion.";
                    points -= 20; //lose a lot of points

                }
            }
        }
        else
        {
            //if the letter was invalid on arrival, deduct points depending on what you stamp it with.
            //if the seal is red:
            if (letter.GetSealColor().Equals(red))
            {
                //sending a high priority letter which is invalid loses more points than a low priority letter.
                points -= 60;
            }
            //if the seal is blue:
            else if (letter.GetSealColor().Equals(blue))
            {
               
                points -= 45;
            }
            //if the seal is green:
            else if (letter.GetSealColor().Equals(green))
            {
              
                points -= 35;

            }
            //if the seal is orange, you're deleting an invalid letter 
            else if (letter.GetSealColor().Equals(delete))
            {
                FindObjectOfType<AudioManager>().Play("Point");
                points += 100; //100 points for correctly discarding the letter
            }
            else
            {
                debugMessage += " Unknown Error, no color.";
            }
        }






        debugMessage += " [Points at the end: " + points + "]";
        ScoreManager.Instance.AddPoints(points);

        bool isNegative = false;
        if(points < 0)
        {
            isNegative = true;
        }

        Debug.Log(debugMessage);
        PointsText.Create(letter.transform.position, points, isNegative);
        Destroy(letter.gameObject);
    }
}
