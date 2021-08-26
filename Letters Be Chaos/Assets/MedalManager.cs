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
    public int lettersDestroyedAutomatically;
    public int lettersDestroyedCorrect;
    public int lettersDestroyedCorrectWithoutMissing;
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
    public bool differentBombCheckRunning;
    public int differentBombCount;
    public int anySpecialLetterCount;
    public int tripleBombComboCount = -1; //-1 because we need to advance the counter so we dont start the coroutine every frame
    public int lettersSortedByAutoSortCount;
    public int lettersHighlightedWhenCorrectlyDeletedCount;
    public int bombLetterTotalCount;
    public int autoSortBombCount;
    public List<string> bombColors;

    [Header("Other")]
    public int totalLettersSorted;
    public bool measuringLetterWipeSpeed;



    public static MedalManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (lettersSortedCorrectManually == 200)
        {
            //Unlocked "I wont be Replaced!"
            PlayerPrefs.SetInt("P1", 1);
        }
        else if (lettersSortedCorrectManually == 800)
        {
            //Unlocked "Or will i?"
            PlayerPrefs.SetInt("P5", 1);
        }
        else if (lettersSortedCorrectManually == 1500)
        {
            //Unlocked "no, i dont think i will"
            PlayerPrefs.SetInt("P6", 1);
        }

        if (lettersDestroyedCorrect == 1000)
        {
            PlayerPrefs.SetInt("P2", 1);
        }
        else if (lettersDestroyedCorrect == 2000)
        {
            PlayerPrefs.SetInt("P3", 1);
        }
        else if (lettersDestroyedCorrect == 5000)
        {
            PlayerPrefs.SetInt("P4", 1);
        }

        if (lettersDestroyedAutomatically == 1000)
        {
            PlayerPrefs.SetInt("P7", 1);
        }


        //tracking
        if (lettersSortedCorrectTracked == 1000)
        {
            PlayerPrefs.SetInt("P8", 1);
        }


        if (bombLetterTotalCount == 30)
        {
            PlayerPrefs.SetInt("S4", 1);
        }


        if(lettersDestroyedCorrectWithoutMissing == 20)
        {
            //unlocks "Cautious deletion"
            PlayerPrefs.SetInt("N1", 1);
        }

        if(pointsLostByIncorrectTracking >= 10000 && PlayerPrefs.GetInt("N2") == 0)
        {
            //if N2 is not yet unlocked
            PlayerPrefs.SetInt("N2", 1);
        } 


        if(lettersSortedCorrectTracked == 100)
        {
            PlayerPrefs.SetInt("N3", 1);
        }

        if(lettersSortedUpsideStampOnly == 20)
        {
            PlayerPrefs.SetInt("N4", 1);
        }

        if(lettersSortedWrongColorIncorrectly == 5)
        {
            PlayerPrefs.SetInt("N5", 1);
        }

    }



    public void SetEndGameMedals()
    {
        int score = ScoreManager.Instance.GetScore();
        float accuracy = GameSettings.Instance.letterCountCorrect / GameSettings.Instance.totalLettersProcessed;


        if (score > 5000 && accuracy >= 0.4f)
        {
            PlayerPrefs.SetInt("E1", 1);
        }
        if (score > 10000 && accuracy >= 0.6f)
        {
            PlayerPrefs.SetInt("E2", 1);
        }
        if (score > 50000 && accuracy >= 0.7f)
        {
            PlayerPrefs.SetInt("E3", 1);
        }
        if (score > 1000000 && accuracy >= 0.85f)
        {
            PlayerPrefs.SetInt("E4", 1);
        }

        if(score < 5000 && accuracy >= 0.98f)
        {
            PlayerPrefs.SetInt("E5", 1);
        }
        if (score < 30000 && accuracy >= 0.8f)
        {
            PlayerPrefs.SetInt("E6", 1);
        }
        if (score < 70000 && accuracy <= 0.2f)
        {
            PlayerPrefs.SetInt("E7", 1);
        }
        if (accuracy <= 0.1f)
        {
            PlayerPrefs.SetInt("E8", 1);
        }
        if (score >= 99000 && score <= 100000)
        {
            PlayerPrefs.SetInt("E9",1);
        }


        // Give score based medals
        // Don't change these to else if's otherwise someone can't get multiple score based medals.
        if (score > 1000000)
        {
            PlayerPrefs.SetInt("1MilMedal", 1);
        }
        if (score > 500000)
        {
            PlayerPrefs.SetInt("500kMedal", 1);
        }
        if (score > 100000)
        {
            PlayerPrefs.SetInt("100kMedal", 1);
        }
        if (score > 50000)
        {
            PlayerPrefs.SetInt("50kMedal", 1);
        }
        if (score > 10000)
        {
            PlayerPrefs.SetInt("10kMedal", 1);
        }
        if (score > 5000)
        {
            PlayerPrefs.SetInt("5kMedal", 1);
        }
        if (score > 1000)
        {
            PlayerPrefs.SetInt("1kMedal", 1);
        }
    }



    /// <summary>
    /// Count the letter into the medal system. This method will sort the letter and add values where necessary.
    /// </summary>
    /// <param name="letter"> The letter object </param> 
    /// <param name="autoSorted"> Was it processed automatically? </param>
    /// <param name="valid"> Was this letter processing valid, did they sort it correctly? </param>
    public void CountLetter(InteractableLetter letter, bool autoSorted, bool autoSortedBySortingBomb, bool colourBomb, bool valid, int pointsFromThisProcess)
    {

        //A letter has been processed, we need to count how many they process since the first.



        if (autoSorted)
        {
            lettersSortedAutomaticallyTotal++;
            if (valid)
            {
                lettersSortedCorrectAutomatically++;

                if (!letter.isValidOnArrival)
                {
                    //then it must have been deleted correctly.
                    lettersDestroyedCorrect++;
                    lettersDestroyedAutomatically++;
                    lettersDestroyedTotal++;
                    
                }
            }
            else
            {

                lettersSortedIncorrectAutomatically++;

                if (letter.isValidOnArrival)
                {
                    //it was valid on arrival but it didnt sort it properly (a coloured bomb)
                    lettersDestroyedIncorrect++;
                    lettersDestroyedTotal++;
                }

            }

            if (autoSortedBySortingBomb)
            {
                lettersSortedByAutoSortCount++;
            }


        }
        else
        {
            lettersSortedManuallyTotal++;

            if (valid)
            {
                lettersSortedCorrectManually++;






                if (letter.hasTrackingInfo && letter.hasBeenTrackStamped)
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
                    
                    lettersDestroyedCorrectWithoutMissing++;
                    lettersSortedOrange++;

                    //the letter was deleted correctly.
                    lettersDestroyedCorrect++;
                    lettersDestroyedCorrectSequentialCount++;
                    lettersDestroyedTotal++;
                }

            }
            else
            {
                lettersSortedIncorrectManually++;


                if (letter.isValidOnArrival)
                {
                    //this letter was valid, and was sorted incorrectly. A letter was "missed".
                    lettersDestroyedCorrectWithoutMissing = 0;
                }

                //for "upside down and nothing else" medal. In the "invalid" section as they must be *incorrectly* processed.
                if (letter.isPostageStamped && letter.isUpsideDown && letter.isSealed && letter.colorStampedWith.Equals(letter.GetSealColor()))
                {
                    if (letter.hasTrackingInfo && letter.hasBeenTrackStamped)
                    {
                        
                        lettersSortedUpsideStampOnly++;
                    }
                    else if (!letter.hasTrackingInfo && !letter.hasBeenTrackStamped)
                    {
                        lettersSortedUpsideStampOnly++;
                    }
                }

                //for "5 letters and nothing wrong with them"
                if (letter.isPostageStamped && !letter.isUpsideDown && letter.isSealed)
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

                //it was marked for deletion but shouldnt have been.
                if (letter.colorStampedWith.Equals(GameSettings.Instance.delete) && letter.isValidOnArrival)
                {
                    lettersDestroyedIncorrect++;
                    lettersDestroyedTotal++;
                }

            }




        }


        if (letter.letterScriptable.isSpecial)
        {
            // CheckToStampSpecialLetter(letter, colourBomb);


            if (autoSortedBySortingBomb || colourBomb)
            {
                bombLetterTotalCount++;
                CheckToStampSpecialLetter(letter, colourBomb); //you've processed a bomb 
                tripleBombComboCount++;
                //it would equal zero after the first bomb of any type, because it starts at -1
                if (tripleBombComboCount == 0)
                {
                    tripleBombComboCount = 1; //now its set to 1, because we have 1 bomb, but it no longer satisfies the condition for the coroutine to be called.
                    StartCoroutine(CompareBombCountAfterTime(5f));
                }
            }
        }




        //for medal unlocking
        if (anySpecialLetterCount == specialLetters.Count)
        {
            int count = 0;
            foreach (SpecialLetterStamped letterSpecial in specialLetters)
            {
                if (letterSpecial.hasBeenStamped)
                {
                    //if a letter has been stamped, then add to count.   
                    count++;
                }
            }


            //if the count is equal to the amount of special letters in the game, 
            //then we have stamped all of them.
            if (count >= specialLetters.Count)
            {
                //Unlocks "Skipping Letter Day"
                PlayerPrefs.SetInt("S1", 1);
                //we have stamped at least one of each of the special letters.

                if (!differentBombCheckRunning)
                {
                    foreach (SpecialLetterStamped letterSpecial in specialLetters)
                    {
                        //reset them back to false as long as the coroutine checking them isnt running
                        letterSpecial.hasBeenStamped = false;
                    }
                }

            }
        }

        //If its a bomb. This needs to run after the previous code which is why it isnt in the above if statement
        if (letter.letterScriptable.isSpecial && (colourBomb || autoSortedBySortingBomb))
        {
            if (PlayerPrefs.GetInt("S3") == 0)
                //if its a bomb and the coroutine isnt running, we need to track if they blow up and more bombs in this timeframe.
                if (!differentBombCheckRunning)
                {
                    //should check as long as the medal isnt already unlocked.
                    StartCoroutine(CompareDifferentBombCountAfterTime(specialLetters.Find(x => x.letter == letter), 5f));
                }
        }
    }


    private void CheckToStampSpecialLetter(InteractableLetter letter, bool colourBomb)
    {
        SpecialLetterStamped stampedLetter = specialLetters.Find(x => x.letter == letter);

        if (stampedLetter != null)
        {
            anySpecialLetterCount++;
            //has this specific type been stamped before?
            if (stampedLetter.hasBeenStamped == false)
            {
                //find it in the list
                //"find x, where x.letter is the same as this letter ^"
                specialLetters.Find(x => x.letter == letter).hasBeenStamped = true;


            }
            else
            {
                //this letter type has already been stamped.
                differentBombCount = 0;
            }

        }

    }


    //coroutines

    private IEnumerator CompareBombCountAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        //we have stamped 2 or more bombs in the time it took for the coroutine to run
        if (tripleBombComboCount >= 3)
        {
            //Unlocks "Cluster"
            PlayerPrefs.SetInt("S2", 1);
        }
    }


    //after the time, check if 3 different types have been stamped.
    private IEnumerator CompareDifferentBombCountAfterTime(SpecialLetterStamped theExisting, float time)
    {
        differentBombCheckRunning = true;
        if (theExisting != null)
        {
            foreach (SpecialLetterStamped stampedLetters in specialLetters)
            {
                //if its a bomb and its not equal to the current one
                if (stampedLetters.isBomb && stampedLetters != theExisting)
                {
                    //set them all to false except ours.
                    stampedLetters.hasBeenStamped = false;
                }
            }
        }



        yield return new WaitForSeconds(time);


        //after the duration, we then need to check how many are now stamped, not including our own.

        int count = 0;
        foreach (SpecialLetterStamped stampedLetters in specialLetters)
        {
            //if its a bomb and its not equal to the current one
            if (stampedLetters.isBomb && stampedLetters != theExisting)
            {
                count++;
            }
        }

        //the count is greater than or equal to 2, we have stamped 2 more since stamping the first, and now should unlock the medal.
        if (count >= 2)
        {
            //Unlocks "Fragmentation"
            PlayerPrefs.SetInt("S3", 1);
        }

        //the different bomb count is reset.
        differentBombCount = 0;
        differentBombCheckRunning = false;
    }


    private IEnumerator LetterWipeMeasurement(int initialValue, float time)
    {
        measuringLetterWipeSpeed = true;
        int count = 0;
        yield return new WaitForSeconds(time);
        if (totalLettersSorted - initialValue >= 80)
        {
            //unlocks "Janitor"
            PlayerPrefs.SetFloat("S7", 1);
            count++;
            if (totalLettersSorted - initialValue >= 500)
            {
                count++;
                //unlocks "why even click?"
                PlayerPrefs.SetFloat("S8", 1);
            }
        }

        if (count < 2)
        {
            //we still havent unlocked S8, so allow it to run again.
            measuringLetterWipeSpeed = false;

        }
        //we've unlocked all the medals for this area, dont bother running the coroutine again.
    }

}




[System.Serializable]
public class SpecialLetterStamped
{
    public InteractableLetter letter;
    public bool hasBeenStamped;
    public bool isBomb;
}




