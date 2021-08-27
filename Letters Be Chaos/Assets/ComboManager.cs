using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// This script tracks all things to do with combos. 
/// A combo can be started by doing many things, but breaking it can only be done a certain number of ways.
/// E.g. If you earn a combo by stamping the same colour in a row, and then stamp a different color, you lose it. If you stamp the right color, but the wrong letter, you lose it.
/// </summary>
public class ComboManager : MonoBehaviour
{
    public static ComboManager Instance { get; private set; }

    public Transform comboSpawn;
    public TextMeshProUGUI comboTextDebug;
    private Animator comboAnimator;

    [HideInInspector]public bool comboActive;

    private float comboMultiplier = 1f;
    private float comboExpireTimer;
    private float comboExpireTimerMax = 10;

    //Basic "10 or more correct" Combo.
    //Players must stamp 10 letters in a row, without error.
    private int correctSequentialStamps10orMore = 0;
    private bool checkingForSequentialStampCombo = true;

    //10 or more with the same color
    private Color32 lastCorrectlyStampedColor;
    private int correctSequentialStampsWithSameColor = 0;
    private bool checkingForSequentialIdenticalStampCombo = true;

    //Stamp 4 different colors

    private bool hasStampedRed;
    private bool hasStampedBlue;
    private bool hasStampedGreen;
    private bool hasStampedOrange;
    private bool allColoursStamped;

    private int correctSequentialStamps5orMore = 0;
   

    //delete 5 in a row in under 3 seconds
    private int deleteSequential3SecondCount = 0;
    private bool checkingForSequential3SecondDeletion;
    private int tempValue; //stores the difference between the two.
    private float deleteSequentialTimerDuration = 3f;

    private void Awake()
    {
        Instance = this;
        lastCorrectlyStampedColor = Color.black;
        comboExpireTimer = comboExpireTimerMax;
        comboAnimator = comboTextDebug.GetComponent<Animator>();
    }

    private void Update()
    {


        if (correctSequentialStamps10orMore == 10)
        {
            correctSequentialStamps10orMore = 0;
          

            //every 5 letters, add another combo.
            if (correctSequentialStamps5orMore == 5)
            {
                correctSequentialStamps5orMore = 0;
                AddMultiplier(2f);
            }
            else
            {
                AddMultiplier(1.5f);
            }

        }
        else if (correctSequentialStamps5orMore == 5)
        {
            correctSequentialStamps5orMore = 0;
            AddMultiplier(0.2f);
        }



        if (correctSequentialStampsWithSameColor == 10)
        {
            correctSequentialStampsWithSameColor = 0;
            AddMultiplier(2f);
        }



        if (allColoursStamped)
        {
            allColoursStamped = false;
            hasStampedBlue = false;
            hasStampedGreen = false;
            hasStampedOrange = false;
            hasStampedRed = false;
            AddMultiplier(10f);
            //slightly longer to play with it.
            comboExpireTimer += 5f;
        }


        if (checkingForSequential3SecondDeletion)
        {
            deleteSequentialTimerDuration -= Time.deltaTime;
            if(deleteSequentialTimerDuration <= 0)
            {
                if(deleteSequential3SecondCount - tempValue >= 5)
                {
                    AddMultiplier(3);
                }
                tempValue = 0;
                deleteSequential3SecondCount = 0;
                checkingForSequential3SecondDeletion = false;
            }
        }



















        if (comboMultiplier > 1)
        {
            comboActive = true;
            

        }
        else
        {
            comboActive = false;
        }




        comboAnimator.SetBool("Active", comboActive);

        //disappear over time
        if (comboActive)
        {
            comboExpireTimer -= Time.deltaTime;
            if (comboExpireTimer <= 0)
            {
                //to stop this, just comboExpireTimer = comboExpireTimerMax.
                ResetMultiplier();
                comboExpireTimer = comboExpireTimerMax;
            }
        }

      
        
    }


    private void ResetMultiplier()
    {
        comboMultiplier = 1f;
        comboTextDebug.text = "1x Multiplier";
        //reset all booleans
        checkingForSequentialStampCombo = true;
        ResetUniqueStamping();
    }

    public void AddCorrectStamp(InteractableLetter letter)
    {
        correctSequentialStamps10orMore++;
        correctSequentialStamps5orMore++;

        if (!letter.isValidOnArrival)
        {
            deleteSequential3SecondCount++;
            if (!checkingForSequential3SecondDeletion)
            {
                checkingForSequential3SecondDeletion = true;
                tempValue = deleteSequential3SecondCount;
            }
            
        }
    }

    public void MarkIncorrectStamp(InteractableLetter letter)
    {
        ResetMultiplier();
        //If the player stamps something incorrectly, this is called.
        correctSequentialStamps10orMore = 0;
        correctSequentialStamps5orMore = 0;
        correctSequentialStampsWithSameColor = 0;
        lastCorrectlyStampedColor = Color.black;
    }


    public void AddMultiplier(float value)
    {

        if (comboMultiplier == 1) {
            AudioManager.Instance.Play("Multiplier");
        } else {
            AudioManager.Instance.Play("MultiplierGain");
        }

        comboMultiplier += value;
        ComboPoints.Create(comboSpawn.position, comboMultiplier);
        comboExpireTimer = comboExpireTimerMax;
        comboTextDebug.text = comboMultiplier + "x Multiplier";
        comboAnimator.SetTrigger("MultiplierModified");

        
    }

    public float GetMultiplier()
    {
        return comboMultiplier;
    }

    


    public void SetLastCorrectlyStamped(Color32 colorStamped)
    {
        if(lastCorrectlyStampedColor == Color.black)
        {
            lastCorrectlyStampedColor = colorStamped;
        }
        else
        {
            if (lastCorrectlyStampedColor.Equals(colorStamped))
            {
                //they're the same color, that means they stamped correctly twice in a row.
                correctSequentialStampsWithSameColor++;
            }
            else
            {
                correctSequentialStampsWithSameColor = 0;
            }
        }


        //check to see unique stamping
        if (colorStamped.Equals(GameSettings.Instance.red))
        {
           
            if(!hasStampedRed)
            {
                Debug.Log("red");
                hasStampedRed = true;
            }
            else
            {
                ResetUniqueStamping();
            }
        }
        else if (colorStamped.Equals(GameSettings.Instance.blue))
        {
            if (!hasStampedBlue)
            {
                Debug.Log("blue");
                hasStampedBlue = true;
            }
            else
            {
                ResetUniqueStamping();
            }
        }
        else if (colorStamped.Equals(GameSettings.Instance.green))
        {
            if (!hasStampedGreen)
            {
                hasStampedGreen = true;
            }
            else
            {
                ResetUniqueStamping();
            }
        }
        else if (colorStamped.Equals(GameSettings.Instance.delete))
        {
            if (!hasStampedOrange)
            {
                hasStampedOrange = true;
            }
            else
            {
                ResetUniqueStamping();
            }
        }


        if(hasStampedBlue && hasStampedGreen && hasStampedOrange && hasStampedRed)
        {
            allColoursStamped = true;
        }

    }

    private void ResetUniqueStamping()
    {
        allColoursStamped = false;
        hasStampedBlue = false;
        hasStampedGreen = false;
        hasStampedOrange = false;
        hasStampedRed = false;
    }

   
}
