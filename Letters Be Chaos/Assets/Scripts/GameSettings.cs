using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * This class will hold all information on the game settings, stuff that other scripts will need access to.
 */
public class GameSettings : MonoBehaviour
{

    public static GameSettings Instance { get; private set; }

    public StampingArm arm;

    
    private int letterCount;
    private int tempLetterCount;
    private float totalLettersProcessed;
    private float letterCountCorrect;
    private float letterCountIncorrect;
    [Header("Letter UI")]
    public TextMeshProUGUI letterCountText;
    public TextMeshProUGUI letterCountCorrectText;
    public TextMeshProUGUI letterCountIncorrectText;
    public TextMeshProUGUI AccuracyText;
    

    [Header("Usage Stats")]
    public StatControllerScript CPU;
    public StatControllerScript RAM;
    public StatControllerScript HYDRAULIC;


    [Header("Letter Type Prefabs")]
    public Transform letterPrefab_FirstClass;
    public Transform pfPointsText;
    public Transform pfComboPoints;

    [Header("Letter Colors: Decided by random chance")]
    [SerializeField] public Color32 red;
    [SerializeField] public Color32 blue;
    [SerializeField] public Color32 green;
    [SerializeField] public Color32 delete;
    public List<Color32> invalidColors;

    [Header("Areas letters are sent during processing")]
    [SerializeField] public Transform redLocation;
    [SerializeField] public Transform greenLocation;
    [SerializeField] public Transform blueLocation;
    [SerializeField] public Transform deleteLocation;

    [Header("Letter color chances")]
    [Range(0,1)] public float redChance;
    [Range(0, 1)] public float greenChance;
    [Range(0, 1)] public float blueChance;

    [Header("Letter Icons")] //all icons for all possible things wrong with letters.
    public Sprite icon_WrongColor;
    public Sprite icon_NoStamp;
    public Sprite icon_UpsideStamp;
    public Sprite icon_NoSeal;
    public Sprite icon_NoSeal_NoStamp;
    public Sprite icon_NoSeal_UpsideStamp;
    public Sprite icon_WrongColor_NoStamp;
    public Sprite icon_WrongColor_UpsideStamp;
    public Sprite icon_WrongColor_NotTracked;
    public Sprite icon_WrongColor_NotTracked_UpsideStamp;
    public Sprite icon_UpsideStamp_NotTracked;
    public Sprite icon_NoSeal_NotTracked;
    public Sprite icon_NoSeal_NotTracked_UpsideStamp;
    public Sprite icon_NotTracked;


    private void Awake()
    {
        Application.targetFrameRate = 165;
        //this
        //now you can use "GameSettings.Instance.(insert method name)" anywhere. 
        //Why use this instead of static classes? static classes are a pain in the ass.
        Instance = this;
        AccuracyText.text = "Accuracy: --";
    }

    /// <summary>
    /// Sort the letter and assign points.
    /// </summary>
    /// <param name="letter"></param>
    public void ProcessLetter(InteractableLetter letter)
    {

        
        Sprite iconSet = null; //this is the icon we will use for errors
        int points = 0;
        string debugMessage = "";


        if (!letter.letterScriptable.isSpecial)
        {
            //only count if not special.
            totalLettersProcessed++;

            //if the letter is upright, with a valid color seal (rgb) and has a stamp:
            if (letter.isValidOnArrival)
            {
                if(letter.hasTrackingInfo && letter.hasBeenTrackStamped)
                {
                    //you correctly tracked this letter, theres a 100 bonus
                    points += 100;
                }
                else if (!letter.hasTrackingInfo && letter.hasBeenTrackStamped)
                {
                    //incorrect stamped with tracking info, wasting company money.
                    AudioManager.Instance.Play(AudioManager.Instance.returnIncorrect(5));
                    points -= 400;
                }

                //if the letter was correctly stamped by the player
                if (letter.isCorrectColorCombo)
                {
                    //if the seal is red:
                    if (letter.GetSealColor().Equals(red))
                    {
                        debugMessage += " Was correct color: red";
                     
                        points += 50;
                        tempLetterCount--;
                    }
                    //if the seal is blue:
                    else if (letter.GetSealColor().Equals(blue))
                    {
                        debugMessage += " Was correct color: blue";
                      
                        points += 20;
                        tempLetterCount--;
                    }
                    //if the seal is green:
                    else if (letter.GetSealColor().Equals(green))
                    {
                        debugMessage += " Was correct color: green";
                       
                        points += 10;
                        tempLetterCount--;
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

                    ComboManager.Instance.AddCorrectStamp();
                    ComboManager.Instance.SetLastCorrectlyStamped(letter.colorStampedWith);
                    AudioManager.Instance.Play("Point");
                }
                else
                {
                    //if it wasnt
                    if (letter.GetSealColor().Equals(red))
                    {
                        debugMessage += " Was incorrect color: red";
                        points -= 150;
                    
                        increaseRandom(10);
                        AudioManager.Instance.Play(AudioManager.Instance.returnIncorrect(5));
                    }
                    else if (letter.GetSealColor().Equals(blue))
                    {
                        debugMessage += " Was incorrect color: blue";
                        points -= 80;
                      
                        increaseRandom(10);
                        AudioManager.Instance.Play(AudioManager.Instance.returnIncorrect(5));
                    }
                    else if (letter.GetSealColor().Equals(green))
                    {
                        debugMessage += " Was incorrect color: green";
                        points -= 50;
                      
                        increaseRandom(10);
                        AudioManager.Instance.Play(AudioManager.Instance.returnIncorrect(5));
                    }
                    else if (letter.GetSealColor().Equals(delete))
                    {
                        AudioManager.Instance.Play(AudioManager.Instance.returnIncorrect(3));
                        //if the player marks a valid letter for deletion
                        debugMessage += " Was incorrectly marked for deletion.";
                     
                        points -= 500; //lose a lot of points
                        increaseRandom(26);
                    }
                }
            }
            else
            {
                //if the letter was invalid on arrival, deduct points depending on what you stamp it with.
                //if the letter is the correct color (orange)
                if (letter.colorStampedWith.Equals(delete))
                {
                    AudioManager.Instance.Play("Point");
                    ComboManager.Instance.SetLastCorrectlyStamped(letter.colorStampedWith);
                 
                    points += 100; //100 points for correctly discarding the letter
                    tempLetterCount--;
                }
                //if it wasnt orange, and it was invalid, take these.
                else
                {
                    AudioManager.Instance.Play(AudioManager.Instance.returnIncorrect(5));
                    //if the seal is red:
                    if (letter.GetSealColor().Equals(red))
                    {
                        //sending a high priority letter which is invalid loses more points than a low priority letter.
                        points -= 150;
                        
                        increaseRandom(15);
                    }
                    //if the seal is blue:
                    else if (letter.GetSealColor().Equals(blue))
                    {
                        points -= 80;
                      
                        increaseRandom(15);
                    }
                    //if the seal is green:
                    else if (letter.GetSealColor().Equals(green))
                    {
                        points -= 50;
                       
                        increaseRandom(15);
                    }
                    else
                    {
                        


                    }

                    if(letter.hasTrackingInfo && !letter.hasBeenTrackStamped)
                    {
                        //you missed a high priority letter.
                        points -= 150;

                    }

                    

                }




            }


            //Is the letter sealed?
            if (letter.isSealed)
            {
                //Is the seal a valid color, and have we stamped it right?
                if (letter.isCorrectColorCombo)
                {
                    if (letter.isPostageStamped)
                    {
                        if (letter.hasTrackingInfo)
                        {
                            if (!letter.hasBeenTrackStamped)
                            {
                                iconSet = icon_NotTracked;

                                if (letter.isUpsideDown)
                                {
                                    iconSet = icon_UpsideStamp_NotTracked;
                                }
                            }
                        }
                        else
                        {
                            if (letter.isUpsideDown)
                            {
                                iconSet = icon_UpsideStamp;
                            }
                        }
                    }
                    else
                    {
                        iconSet = icon_NoStamp;
                    }
                }
                else
                {
                    iconSet = icon_WrongColor;

                    if (letter.isPostageStamped)
                    {
                        if (letter.hasTrackingInfo)
                        {
                            if (!letter.hasBeenTrackStamped)
                            {
                                iconSet = icon_WrongColor_NotTracked;

                                if (letter.isUpsideDown)
                                {
                                    iconSet = icon_WrongColor_NotTracked_UpsideStamp;
                                }
                            }
                        }
                        else
                        {
                            if (letter.isUpsideDown)
                            {
                                iconSet = icon_WrongColor_UpsideStamp;
                            }
                        }
                    }
                    else
                    {
                        iconSet = icon_WrongColor_NoStamp;
                    }
                }
            }
            else
            {
                iconSet = icon_NoSeal;

                if (letter.isPostageStamped)
                {
                    if (letter.hasTrackingInfo)
                    {
                        if (!letter.hasBeenTrackStamped)
                        {
                            iconSet = icon_NoSeal_NotTracked;

                            if (letter.isUpsideDown)
                            {
                                iconSet = icon_NoSeal_NotTracked_UpsideStamp;
                            }
                        }
                    }
                    else
                    {
                        if (letter.isUpsideDown)
                        {
                            iconSet = icon_NoSeal_UpsideStamp;
                        }
                    }
                }
                else
                {
                    iconSet = icon_NoSeal_NoStamp;
                }
            }

        }
        else
        {
            //the letter is special. Add points for a special letter.
            if(letter.letterScriptable.nameString == "Bomb")
            {
                points += 1000;
            }
        }
        

        bool isNegative = false;
        if(points < 0)
        {
            letterCountIncorrect++;
            ComboManager.Instance.MarkIncorrectStamp();
            isNegative = true;
        }
        else
        {
            letterCountCorrect++;
        }
        if (points > 0 && ComboManager.Instance.comboActive)
        {

            points = Mathf.RoundToInt(points * ComboManager.Instance.GetMultiplier());
        }

        ScoreManager.Instance.AddPoints(points);
        PointsText.Create(letter.transform.position, points, isNegative, iconSet);
        letterCount--;
        letterCountText.text = "Letters: " + letterCount.ToString();

        letterCountCorrectText.text = "Correct: " + "<color=#31A62E>" + letterCountCorrect.ToString() + "</color>";
        letterCountIncorrectText.text = "Incorrect: " + "<color=#B03E3E>" + letterCountIncorrect.ToString() + "</color>";

        if(totalLettersProcessed > 0)
        {
            AccuracyText.text = "Accuracy:<color=#" + GetHexColorFromFloat(letterCountCorrect / totalLettersProcessed) + ">" + ((letterCountCorrect / totalLettersProcessed) * 100).ToString("F0") + "%</color>";
        }
        else
        {
            AccuracyText.text = "Accuracy:<color=#" + GetHexColorFromFloat(letterCountCorrect / totalLettersProcessed) + ">" + "100%</color>";
        }


        Destroy(letter.gameObject);
    }

    private string GetHexColorFromFloat(float value)
    {


        if (value >= 0.75f)
        {
            return "31A62E";
        }
        else if (value >= 0.5f)
        {
            return "FA6A0A";
        }
        else if (value < 0.5f)
        {
            return "B03E3E";
        }
        else
        {
            return "e3961b";
        }
    }

    // Adds letter to onscreen count & calculates usage increase every 5 unresolved letters
    public void AddLetter() {
        // Onscreen count
        letterCount++;
        letterCountText.text = "Letters: " + letterCount.ToString();

        // Every 5 uncorrectly resolved letters, increase random stat by 10.
        tempLetterCount++;
        if (tempLetterCount > 5) {
            increaseRandom(10);
            tempLetterCount = 0;
        }
    }

    // Increases a random stat.
    void increaseRandom(int usageIncrease) {
        int r = Random.Range(0, 3);

        switch (r) {
            case 0:
                GameSettings.Instance.CPU.setUsage(GameSettings.Instance.CPU.getUsage() + usageIncrease);
                break;
            case 1:
                GameSettings.Instance.RAM.setUsage(GameSettings.Instance.HYDRAULIC.getUsage() + usageIncrease);
                break;
            case 2:
                GameSettings.Instance.HYDRAULIC.setUsage(GameSettings.Instance.HYDRAULIC.getUsage() + usageIncrease);
                break;
            default:
                break;
        }
    }

    public Color32 GetRandomColor()
    {
        int random = Random.Range(0, invalidColors.Count);
        Debug.Log(random);
        for(int i = 0; i < invalidColors.Count; i++)
        {
            if(random == i)
            {
                return invalidColors[i];
            }
        }

        return new Color32(255,255,255,255);
    }
   
}
