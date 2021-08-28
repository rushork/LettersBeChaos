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
   

    [Header("Feature Sorting")]
    public int lettersSortedUpsideStampOnly;
    public int lettersSortedIncorrectColourOnly;

    [Header("Special Sorting")]
    public List<SpecialLetterStamped> specialLetters;
    public bool differentBombCheckRunning;
    public int differentBombCount;
    public int anySpecialLetterCount;
    public int totalSpecialLetterCount;
    public int tripleBombComboCount = -1; //-1 because we need to advance the counter so we dont start the coroutine every frame
    public int lettersSortedByAutoSortCount;
    public int lettersHighlightedWhenCorrectlyDeletedCount;
    public int specialLetterTotalCount;
    public int autoSortBombCount;
    public List<string> bombColors;

    [Header("Other")]
    public int totalLettersSorted;
    public bool measuringLetterWipeSpeedS8;
    public bool measuringLetterWipeSpeedS7;

    [Header("DEV TOOLS")]
    public List<string> medalID;
    public MedalTray trayScript;
    CoroutineQueue medalQueue;
    public List<MedalSpriteIndex> medalSpriteIndices;

    [System.Serializable]
    public class MedalSpriteIndex
    {
        public string code;
        public Sprite sprite;
        
    }



    public static MedalManager Instance { get; private set; }

    private void Awake()
    {
        
        Instance = this;
        medalQueue = new CoroutineQueue(this);
        medalQueue.StartLoop();
    
    }

 
    //manages medal queue system
    private void UnlockMedal(string code, int am)
    {
        if(PlayerPrefs.GetInt(code) == default)
        {

            PlayerPrefs.SetInt(code, am);
            medalQueue.EnqueueAction(MedalUnlockCoroutine(code));


        }
        
    }

    private IEnumerator MedalUnlockCoroutine(string code)
    {
        Sprite sprite = medalSpriteIndices.Find(x => x.code == code).sprite;

        if (sprite != null)
        {
            AudioManager.Instance.Play("MedalUnlock");
            
            trayScript.OpenTray(sprite);
        }

        yield return new WaitForSeconds(4.5f);

        

        
    }

    private void Update()
    {
       
     

        if (lettersSortedCorrectManually >= 200 && (PlayerPrefs.GetInt("P1") == 0 || PlayerPrefs.GetInt("P1") == default))
        {
            //Unlocked "I wont be Replaced!"
            UnlockMedal("P1",1);
        }
        if (lettersSortedCorrectManually >= 800 && (PlayerPrefs.GetInt("P5") == 0 || PlayerPrefs.GetInt("P5") == default))
        {
            //Unlocked "Or will i?"
            UnlockMedal("P5",1);
        }
        if (lettersSortedCorrectManually >= 1500 && (PlayerPrefs.GetInt("P6") == 0 || PlayerPrefs.GetInt("P6") == default))
        {
            //Unlocked "no, i dont think i will"
            UnlockMedal("P6", 1);
        }

        if (lettersDestroyedCorrect >= 1000 && (PlayerPrefs.GetInt("P2") == 0 || PlayerPrefs.GetInt("P2") == default))
        {
            UnlockMedal("P2", 1);
        }
        if (lettersDestroyedCorrect >= 2000 && (PlayerPrefs.GetInt("P3") == 0 || PlayerPrefs.GetInt("P3") == default))
        {
            UnlockMedal("P3", 1);
        }
        if (lettersDestroyedCorrect >= 5000 && (PlayerPrefs.GetInt("P4") == 0 || PlayerPrefs.GetInt("P4") == default))
        {
            UnlockMedal("P4", 1);
        }

        if (lettersDestroyedAutomatically >= 1000 && (PlayerPrefs.GetInt("P7") == 0 || PlayerPrefs.GetInt("P7") == default))
        {
            UnlockMedal("P7", 1);
        }


        //tracking
        if (lettersSortedCorrectTracked >= 1000 && (PlayerPrefs.GetInt("P8") == 0 || PlayerPrefs.GetInt("P8") == default))
        {
            UnlockMedal("P8", 1);
        }


        if (specialLetterTotalCount >= 30 && (PlayerPrefs.GetInt("S4") == 0 || PlayerPrefs.GetInt("S4") == default))
        {
            UnlockMedal("S4", 1);
        }


        if(lettersDestroyedCorrectWithoutMissing >= 20 && (PlayerPrefs.GetInt("N1") == 0 || PlayerPrefs.GetInt("N1") == default))
        {
            //unlocks "Cautious deletion"
            UnlockMedal("N1", 1);
        }

        if((pointsLostByIncorrectTracking >= 10000) && (PlayerPrefs.GetInt("N2") == 0 || PlayerPrefs.GetInt("N2") == default))
        {
            //if N2 is not yet unlocked
            UnlockMedal("N2", 1);
        } 


        if(lettersSortedCorrectTracked >= 100 && (PlayerPrefs.GetInt("N3") == default || PlayerPrefs.GetInt("N3") == 0))
        {
            UnlockMedal("N3", 1);
        }

        if(lettersSortedUpsideStampOnly >= 20 && (PlayerPrefs.GetInt("N4") == default || PlayerPrefs.GetInt("N4") == 0))
        {
            UnlockMedal("N4", 1);
        }

        if(lettersSortedIncorrectColourOnly >= 5 && (PlayerPrefs.GetInt("N5") == default || PlayerPrefs.GetInt("N5") == 0))
        {
            UnlockMedal("N5", 1);
        }

        if(lettersHighlightedWhenCorrectlyDeletedCount >= 100 && (PlayerPrefs.GetInt("S6") == default || PlayerPrefs.GetInt("S6") == 0))
        {
            UnlockMedal("S6", 1);
        }

        if(lettersSortedByAutoSortCount >= 500 && (PlayerPrefs.GetInt("S5") == default || PlayerPrefs.GetInt("S5") == 0))
        {
            UnlockMedal("S5",1);
        }

    }



    public void SetEndGameMedals()
    {
        int score = ScoreManager.Instance.GetScore();
        float accuracy = GameSettings.Instance.letterCountCorrect / GameSettings.Instance.totalLettersProcessed;


        if (score > 5000 && accuracy >= 0.4f)
        {
            UnlockMedal("E1", 1);
        }
        if (score > 10000 && accuracy >= 0.6f)
        {
            UnlockMedal("E2", 1);
        }
        if (score > 50000 && accuracy >= 0.7f)
        {
            UnlockMedal("E3", 1);
        }
        if (score > 1000000 && accuracy >= 0.85f)
        {
            UnlockMedal("E4", 1);
        }

        if(score < 5000 && accuracy >= 0.98f)
        {
            UnlockMedal("E5", 1);
        }
        if (score < 30000 && accuracy >= 0.8f)
        {
            UnlockMedal("E6", 1);
        }
        if (score > 70000 && accuracy <= 0.2f)
        {
            UnlockMedal("E7", 1);
        }
        if (accuracy <= 0.1f)
        {
            UnlockMedal("E8", 1);
        }
        if (score >= 99000 && score <= 100000)
        {
            UnlockMedal("E9",1);
        }


        // Give score based medals
        // Don't change these to else if's otherwise someone can't get multiple score based medals.
        if (score > 1000000)
        {
            UnlockMedal("1MilMedal", 1);
        }
        if (score > 500000)
        {
            UnlockMedal("500kMedal", 1);
        }
        if (score > 100000)
        {
            UnlockMedal("100kMedal", 1);
        }
        if (score > 50000)
        {
            UnlockMedal("50kMedal", 1);
        }
        if (score > 10000)
        {
            UnlockMedal("10kMedal", 1);
        }
        if (score > 5000)
        {
            UnlockMedal("5kMedal", 1);
        }
        if (score > 1000)
        {
            UnlockMedal("1kMedal", 1);
        }


        int count = 0;
        foreach (string str in medalID)
        {
            
            if(PlayerPrefs.GetInt(str) == 1)
            {
                count++;
            }
        }
        if(count >= 37)
        {
            UnlockMedal("A1", 1);
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
        
        if (!letter.hasTrackingInfo && letter.hasBeenTrackStamped)
        {
            
            pointsLostByIncorrectTracking -= pointsFromThisProcess;
        }

        //A letter has been processed, we need to count how many they process since the first.

        if (!measuringLetterWipeSpeedS8)
        {
            
            StartCoroutine(LetterWipeMeasurement(totalLettersSorted, 60));
        }
        if (!measuringLetterWipeSpeedS7)
        {
            StartCoroutine(LetterWipeMeasurement(totalLettersSorted, 6));
           
        }
        //figure out why this coroutine isnt running

        if (autoSorted || autoSortedBySortingBomb || colourBomb)
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


                //for n5
                if (letter.hasInvalidColor && letter.isPostageStamped && !letter.isUpsideDown && letter.colorStampedWith.Equals(letter.GetSealColor()))
                {
                    
                    lettersSortedIncorrectColourOnly++;
                }


                if (letter.isValidOnArrival)
                {
                    //this letter was valid, and was sorted incorrectly. A letter was "missed".
                    lettersDestroyedCorrectWithoutMissing = 0;
                }

                //for "upside down and nothing else" medal. In the "invalid" section as they must be *incorrectly* processed.
                if (letter.isPostageStamped && letter.isUpsideDown && letter.isSealed && letter.colorStampedWith.Equals(letter.GetSealColor()))
                {
                    lettersSortedUpsideStampOnly++;
                    
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

        if (valid)
        {
            totalLettersSorted++;
        }


        if (letter.letterScriptable.isSpecial)
        {
            // CheckToStampSpecialLetter(letter, colourBomb);


            if (autoSortedBySortingBomb || colourBomb )
            {
                
                
                tripleBombComboCount++;
                //it would equal zero after the first bomb of any type, because it starts at -1
                if (tripleBombComboCount == 0)
                {
                    tripleBombComboCount = 1; //now its set to 1, because we have 1 bomb, but it no longer satisfies the condition for the coroutine to be called.
                    StartCoroutine(CompareBombCountAfterTime(5f));
                }
            }


            CheckToStampSpecialLetter(letter, colourBomb); //you've processed a special letter 
            specialLetterTotalCount++;

        }


        




        //if we have stamped more or equal to the amount of special letter types
        if ((PlayerPrefs.GetInt("S1") == default || PlayerPrefs.GetInt("S1") == 0) && (anySpecialLetterCount >= specialLetters.Count))
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
                UnlockMedal("S1", 1);
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
        if (letter.letterScriptable.isSpecial && (colourBomb || autoSortedBySortingBomb || letter.letterScriptable.nameString == "Summon"
             || letter.letterScriptable.nameString == "Trash" || letter.letterScriptable.nameString == "Column") )
        {
            if (PlayerPrefs.GetInt("S3") == 0 || PlayerPrefs.GetInt("S3") == default)
                //if its a bomb and the coroutine isnt running, we need to track if they blow up and more bombs in this timeframe.
                if (!differentBombCheckRunning)
                {
                    //should check as long as the medal isnt already unlocked.
                    StartCoroutine(CompareDifferentBombCountAfterTime(specialLetters.Find(x => x.letter.letterScriptable == letter.letterScriptable), 5f));
                }
        }
    }


    private void CheckToStampSpecialLetter(InteractableLetter letter, bool colourBomb)
    {
        SpecialLetterStamped stampedLetter = specialLetters.Find(x => x.letter.letterScriptable == letter.letterScriptable);

        if (stampedLetter != null)
        {
            anySpecialLetterCount++;
            //has this specific type been stamped before?
            if (stampedLetter.hasBeenStamped == false)
            {
                //find it in the list
                //"find x, where x.letter is the same as this letter ^"
                specialLetters.Find(x => x.letter.letterScriptable == letter.letterScriptable).hasBeenStamped = true;


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
            UnlockMedal("S2", 1);
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
            UnlockMedal("S3", 1);
        }

        //the different bomb count is reset.
        differentBombCount = 0;
        differentBombCheckRunning = false;
    }


    private IEnumerator LetterWipeMeasurement(int initialValue, float time)
    {
        

        if(time == 6)
        {
            measuringLetterWipeSpeedS7 = true;
        }
        else if(time == 60)
        {
            measuringLetterWipeSpeedS8 = true;
        }

        
        yield return new WaitForSeconds(time);
        if (totalLettersSorted - initialValue >= 80)
        {
            //unlocks "Janitor"
            UnlockMedal("S7", 1);
            
            if (totalLettersSorted - initialValue >= 500)
            {
                
                //unlocks "why even click?"
                UnlockMedal("S8", 1);
            }
        }

        if (PlayerPrefs.GetInt("S7") == default || PlayerPrefs.GetInt("S7") == 0)
        {
            //we still havent unlocked S8, so allow it to run again.
            measuringLetterWipeSpeedS7 = false;

        }
        if (PlayerPrefs.GetInt("S8") == default || PlayerPrefs.GetInt("S8") == 0)
        {
            //we still havent unlocked S8, so allow it to run again.
            measuringLetterWipeSpeedS8 = false;

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




