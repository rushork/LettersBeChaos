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

    public List<Transform> comboSpawns;
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

    //time without failure
    private float totalTimeWithoutFailure = 0;
    private int totalCorrect;
    private float timeToCheckIfStillStamping = 15f;
    private bool isStillStamping = true;
    private float timerCount;

    private float timeSinceLastMultiplier;
    private bool checkingInterval;

    private void Awake()
    {
        Instance = this;
        lastCorrectlyStampedColor = Color.black;
        comboExpireTimer = comboExpireTimerMax;
        comboAnimator = comboTextDebug.GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(CheckIfStamping());
    }


    private IEnumerator CheckIfStamping()
    {
        int temp = totalCorrect;
        yield return new WaitForSeconds(timeToCheckIfStillStamping);
        if(totalCorrect > temp)
        {
            isStillStamping = true;
        }
        else
        {
            isStillStamping = false;
        }
    }

    private void Update()
    {

        timeSinceLastMultiplier += Time.deltaTime;


        if (isStillStamping)
        {
            totalTimeWithoutFailure += Time.deltaTime;
            if (totalTimeWithoutFailure >= 60)
            {
                AddTimerValueCount(60f);
                totalTimeWithoutFailure = 0;

            }
        }
        

        if (correctSequentialStamps5orMore >= 5)
        {
            if(correctSequentialStamps5orMore == 10)
            {
                AddMultiplier(0.7f, "10 in a row");
                correctSequentialStamps5orMore = 0;
            }
            else if (correctSequentialStamps5orMore == 5)
            {
                AddMultiplier(0.2f, "5 in a row");
                correctSequentialStamps5orMore = 0;
            }
            
            
        }

        //if (correctSequentialStamps10orMore == 10)
        //{
            
        //    correctSequentialStamps10orMore = 0;
            

        //}
        



        if (correctSequentialStampsWithSameColor == 10)
        {
            correctSequentialStampsWithSameColor = 0;
            AddMultiplier(0.7f,"10 Identical Colours!");
        }



        if (allColoursStamped)
        {
            allColoursStamped = false;
            hasStampedBlue = false;
            hasStampedGreen = false;
            hasStampedOrange = false;
            hasStampedRed = false;
            AddMultiplier(8f, "Stamped all the colours!");
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
                    AddMultiplier(1.5f, "Delete 5 in under 3 seconds");
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
        totalCorrect++;
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
        totalCorrect--;
        //you failed, reset the timer and the counter.
        totalTimeWithoutFailure = 0;
        ResetMultiplier();
        //If the player stamps something incorrectly, this is called.
        correctSequentialStamps10orMore = 0;
        correctSequentialStamps5orMore = 0;
        correctSequentialStampsWithSameColor = 0;
        lastCorrectlyStampedColor = Color.black;
    }


    public void AddMultiplier(float value, string message)
    {

        if (comboMultiplier == 1) {
            AudioManager.Instance.Play("Multiplier");
        } else {
            AudioManager.Instance.Play("MultiplierGain");
        }

        comboMultiplier += value;



        if(timeSinceLastMultiplier < 2f)
        {
            ComboPoints.Create(comboSpawns[1].position, comboMultiplier, message);
            timeSinceLastMultiplier = 0;
        }
        else
        {
            ComboPoints.Create(comboSpawns[0].position, comboMultiplier, message);

            timeSinceLastMultiplier = 0;
        }

        
        comboExpireTimer = comboExpireTimerMax;
        comboTextDebug.text = comboMultiplier + "x Multiplier";
        comboAnimator.SetTrigger("MultiplierModified");

        
    }

    public float GetMultiplier()
    {
        return comboMultiplier;
    }

    
    public void AddTimerValueCount(float timePassed)
    {
        timerCount += timePassed;

        if(timerCount == 60)
        {
            AddMultiplier(5,"60 seconds without failure");
        }
        else if (timerCount == 120)
        {
            AddMultiplier(8, "2 minutes without failure");
        }
        else if (timerCount == 240)
        {
            AddMultiplier(15, "4 minutes without failure");
        }
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
