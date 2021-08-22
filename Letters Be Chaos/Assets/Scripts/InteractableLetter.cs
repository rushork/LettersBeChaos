using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableLetter : MonoBehaviour
{
    [Tooltip("The scriptable object assigned to this letter.")] [SerializeField] private LetterSO letterScriptable;
    [Tooltip("The location for any generic stamp")] [SerializeField] private Transform leftStampLocation;
    [Tooltip("The location for 'trash' stamps")] [SerializeField] private Transform rightStampLocation;
    private SpriteRenderer mySpriteRenderer;
    private SpriteRenderer sealRenderer;
    private Color32 sealColor;
    private string trackingNumber; //this is a string because it contains letters and numbers.


    //these values are decided after spawning
    private bool hasTrackingInfo;
    private bool isUpsideDown;
    private bool isCorrectColor;
    private bool isSealed;



    private void Awake()
    {
        sealRenderer = transform.Find("Seal").GetComponent<SpriteRenderer>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void Start()
    {
        HandleRandomVariations();
    }

    /// <summary>
    /// Handles the color, shape, size and other randomized attributes of a letter.
    /// </summary>
    private void HandleRandomVariations()
    {
        //the total is equal to 100% chance, so we dont have to make sure they all add to 100 as 20 + 2434 + 1 is still 100% of the chance
        //doubt that makes sense but trust the maths bro
        float randomTotal = letterScriptable.chanceBlue + letterScriptable.chanceGreen + letterScriptable.chanceRed;

        float chanceBlue = letterScriptable.chanceBlue / randomTotal;
        float chanceRed = letterScriptable.chanceRed / randomTotal;
        float chanceGreen = letterScriptable.chanceGreen / randomTotal;

        //get the random value 
        float randomVal = Random.Range(0, 1);
        Debug.Log(randomVal);
        Debug.Log(chanceBlue);
        Debug.Log(chanceGreen);
        Debug.Log(chanceRed);

        if (randomVal <= chanceBlue)
        {
            sealColor = GameSettings.Instance.blue;
        }
        else if (randomVal <= chanceGreen)
        {
            sealColor = GameSettings.Instance.green;
        }
        else if (randomVal <= chanceRed)
        {
            sealColor = GameSettings.Instance.red;
        }

        sealRenderer.color = sealColor;
        isCorrectColor = true; // this is temporary


        //chance to flip
        if(randomVal <= letterScriptable.chanceToSpawnFlipped)
        {
            sealRenderer.flipY = true;
            isUpsideDown = true;
        }

        //chance to spawn sealed
        if(randomVal <= letterScriptable.chanceToSpawnSealed)
        {
            //set the seal to visible if it's sealed.
            sealRenderer.gameObject.SetActive(true);
            isSealed = true;
        }

    }
}
