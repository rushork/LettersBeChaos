using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;


/// <summary>
/// Tracks the progress of medals. Some are persistent, some arent.
/// </summary>
public class MedalManager : MonoBehaviour
{
    [Header("Automatic Sorting")]
    public int lettersSortedCorrectAutomatically;
    public int lettersSortedIncorrectAutomatically;
    public int lettersSortedAutomaticallyTotal;

    [Header("Manual Sorting")]
    public int lettersSortedIncorrectManually;
    public int lettersSortedCorrectManually;
    public int lettersSortedManuallyTotal;

    [Header("Tracked Sorting")]
    public int lettersSortedCorrectTracked;
    public int lettersSortedIncorrectTracked;
    public int lettersSortedTrackedTotal;
    public int pointsLostByIncorrectTracking;
    public int trackedLettersMissed;

    [Header("Destroy Sorting")]
    public int lettersDestroyedCorrect;
    public int lettersDestroyedIncorrect;
    public int lettersDestroyedTotal;
    public int lettersDestroyedCorrectSequentialCount;

    [Header("Colour Sorting")]
    public int lettersSortedRed;
    public int lettersSortedBlue;
    public int lettersSortedGreen;
    public int lettersSortedOrange;
    public int lettersSortedWrongColor;
    public int lettersSortedWrongColorIncorrectly;

    [Header("Feature Sorting")]
    public int lettersSortedUpsideStampOnly;

    [Header("Special Sorting")]
    public List<SpecialLetterStamped> specialLetters;
    public int anySpecialLetterCount;
    public int bombSpecialLetterCount;
    public int lettersSortedByAutoSortCount;
    public int lettersHighlightedWhenCorrectlyDeletedCount;

    public int autoSortBombCount;
    public List<string> bombColors;


    /// <summary>
    /// Count the letter into the medal system. This method will sort the letter and add values where necessary.
    /// </summary>
    /// <param name="letter"> The letter object </param> 
    /// <param name="autoSorted"> Was it processed automatically? </param>
    /// <param name="valid"> Was this letter processing valid, did they sort it correctly? </param>
    public void CountLetter(InteractableLetter letter, bool autoSorted, bool valid, int pointsFromThisProcess)
    {
        if (autoSorted)
        {
            lettersSortedAutomaticallyTotal++;
            if (valid)
            {
                lettersSortedCorrectAutomatically++;

            }
            else
            {
                
                lettersSortedIncorrectAutomatically++;


            }



        }
        else
        {
            lettersSortedManuallyTotal++;

            if (valid)
            {
                lettersSortedCorrectManually++;

                if(letter.hasTrackingInfo && letter.hasBeenTrackStamped)
                {
                    lettersSortedCorrectTracked++;
                }
                else if (letter.hasTrackingInfo && !letter.hasBeenTrackStamped)
                {
                    //it should have tracking info
                    trackedLettersMissed++;
                }
                else if (!letter.hasTrackingInfo && letter.hasBeenTrackStamped)
                {
                    //if the letter shouldnt be tracked, but was:
                    lettersSortedIncorrectTracked++;
                    pointsLostByIncorrectTracking += pointsFromThisProcess;
                }


                //if stamped with the right colour combination
                if (letter.colorStampedWith.Equals(letter.GetSealColor()))
                {
                    if (letter.GetSealColor().Equals(GameSettings.Instance.green))
                    {
                        lettersSortedGreen++;
                    }
                    else if (letter.GetSealColor().Equals(GameSettings.Instance.red))
                    {
                        lettersSortedRed++;
                    }
                    else if (letter.GetSealColor().Equals(GameSettings.Instance.blue))
                    {
                        lettersSortedBlue++;
                    }
                }
                //else if it was stamped with orange, and was invalid (correct combination)
                else if (letter.colorStampedWith.Equals(GameSettings.Instance.delete) && !letter.isValidOnArrival)
                {
                    lettersSortedOrange++;
                }

            }
            else
            {
                lettersSortedIncorrectManually++;

                //for "upside down and nothing else" medal
                if (letter.isPostageStamped && letter.isUpsideDown && letter.isSealed && letter.colorStampedWith.Equals(letter.GetSealColor()))
                {
                    if(letter.hasTrackingInfo && letter.hasBeenTrackStamped)
                    {
                        lettersSortedUpsideStampOnly++;
                    }
                    else if (!letter.hasTrackingInfo && !letter.hasBeenTrackStamped)
                    {
                        lettersSortedUpsideStampOnly++;
                    }
                }

                //for "5 letters and nothing wrong with them"
                if(letter.isPostageStamped && !letter.isUpsideDown && letter.isSealed)
                {
                    if (letter.hasTrackingInfo && letter.hasBeenTrackStamped)
                    {
                        lettersSortedWrongColor++;
                    }
                    else if (!letter.hasTrackingInfo && !letter.hasBeenTrackStamped)
                    {
                        lettersSortedWrongColor++;
                    }
                }

                //for tracking the colours that were stamped
                
            }
        }
    }
}
[System.Serializable]
public class SpecialLetterStamped
{
    public InteractableLetter letter;
    public bool hasBeenStamped;
}




