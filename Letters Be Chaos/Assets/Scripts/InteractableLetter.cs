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
    private SpriteRenderer stampRenderer;
    private Color32 sealColor;
    private string trackingNumber; //this is a string because it contains letters and numbers.


    //these values are decided after spawning
    private bool hasTrackingInfo;
    private bool isUpsideDown;
    private bool isCorrectColor;
    private bool isSealed;
    private bool isPostageStamped;



    private void Awake()
    {
        sealRenderer = transform.Find("Seal").GetComponent<SpriteRenderer>();
        stampRenderer = transform.Find("PostageStamp").GetComponent<SpriteRenderer>();
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
     

        //get the random value 
        float randomVal = Random.Range(0f, 1f);
 

        if (randomVal <= GameSettings.Instance.redChance)
        {
            sealColor = GameSettings.Instance.red;
        }
        else if (randomVal > GameSettings.Instance.redChance && randomVal <= GameSettings.Instance.greenChance)
        {
            sealColor = GameSettings.Instance.green;
        }
        else
        {
            sealColor = GameSettings.Instance.blue;
        }

        sealRenderer.color = sealColor;
        isCorrectColor = true; // this is temporary


        //chance to flip
        if(randomVal <= letterScriptable.chanceToSpawnStampFlipped)
        {
            stampRenderer.flipY = true;
            isUpsideDown = true;
        }

        //chance to spawn sealed
        if(randomVal <= letterScriptable.chanceToSpawnSealed)
        {
            //set the seal to visible if it's sealed.
            sealRenderer.gameObject.SetActive(true);
            isSealed = true;
        }

        if(randomVal <= letterScriptable.chanceToBeCorrectlyStamped)
        {
            //set the seal to visible if it's sealed.
            stampRenderer.gameObject.SetActive(true);
            isPostageStamped = true;
        }

    }
}
