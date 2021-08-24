using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script tracks all things to do with combos. 
/// A combo can be started by doing many things, but breaking it can only be done a certain number of ways.
/// E.g. If you earn a combo by stamping the same colour in a row, and then stamp a different color, you lose it. If you stamp the right color, but the wrong letter, you lose it.
/// </summary>
public class ComboManager : MonoBehaviour
{
    public static ComboManager Instance { get; private set; }

    public Transform comboSpawn;


    private float comboMultiplier = 1f;
    private float comboExpireTimer;
    private float comboExpireTimerMax = 10;

    //Basic "10 or more correct" Combo.
    //Players must stamp 10 letters in a row, without error.
    private int correctSequentialStamps = 0;
    private bool checkingForSequentialStampCombo = true;


    private Color32 lastCorrectlyStampedColor;
    private int correctSequentialStampsWithSameColor = 0;

    private void Awake()
    {
        Instance = this;
        lastCorrectlyStampedColor = Color.black;
        comboExpireTimer = comboExpireTimerMax;
    }

    private void Update()
    {
        //Only fires once, and thats if 10 were stamped.
        if(checkingForSequentialStampCombo)
        {
            checkingForSequentialStampCombo = false;
            if (correctSequentialStamps == 10)
            {
                AddMultiplier(1.5f);
            }
            
            
            
            
        }
        

        //disappear over time
        comboExpireTimer -= Time.deltaTime;
        if(comboExpireTimer <= 0)
        {
            //to stop this, just comboExpireTimer = comboExpireTimerMax.
            ResetMultiplier();
            comboExpireTimer = comboExpireTimerMax;
        }
    }


    private void ResetMultiplier()
    {
        comboMultiplier = 0f;

        //reset all booleans
        checkingForSequentialStampCombo = true;
    }

    public void AddCorrectStamp()
    {
        correctSequentialStamps++;
    }

    public void MarkIncorrectStamp()
    {
        //If the player stamps something incorrectly, this is called.
        correctSequentialStamps = 0;
        correctSequentialStampsWithSameColor = 0;
        lastCorrectlyStampedColor = Color.black;
    }

    public void AddMultiplier(float value)
    {
        comboMultiplier += value;
        ComboPoints.Create(comboSpawn.position, comboMultiplier);
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
        
    }
}
