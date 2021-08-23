using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableLetter : MonoBehaviour
{


    public static InteractableLetter CreateLetter(Transform origin,Vector3 targetLocation, float zPos)
    {
        
        Transform interactableLetterTransform = Instantiate(GameSettings.Instance.letterPrefab_FirstClass, new Vector3(origin.position.x, origin.position.y, zPos), Quaternion.AngleAxis(Random.Range(-20,20), Vector3.forward));
        InteractableLetter letter = interactableLetterTransform.GetComponent<InteractableLetter>();
        letter.targetLocationToMoveTo = new Vector3(targetLocation.x, targetLocation.y, zPos);
       


        return letter;
    }





    [Tooltip("The scriptable object assigned to this letter.")] [SerializeField] private LetterSO letterScriptable;
    [Tooltip("The location for any generic stamp")] [SerializeField] private Transform leftStampLocation;
    [Tooltip("The location for 'trash' stamps")] [SerializeField] private Transform rightStampLocation;
    private SpriteRenderer mySpriteRenderer;
    private SpriteRenderer sealRenderer;
    private SpriteRenderer stampRenderer;
    private GameObject highlightObject;
    private Color32 sealColor;
    private string trackingNumber; //this is a string because it contains letters and numbers.


    //these values are decided after spawning
    private bool hasTrackingInfo;
    private bool isUpsideDown;
    private bool isCorrectColor;
    private bool isSealed;
    private bool isPostageStamped;

    //for spawning
    private Vector3 targetLocationToMoveTo;
    private bool isMoving = true;

    private void Awake()
    {
        sealRenderer = transform.Find("Seal").GetComponent<SpriteRenderer>();
        stampRenderer = transform.Find("PostageStamp").GetComponent<SpriteRenderer>();
        mySpriteRenderer = transform.Find("Letter").GetComponent<SpriteRenderer>();
        highlightObject = transform.Find("Highlight").gameObject;

    }

    private void Start()
    {
        HandleRandomVariations();
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetLocationToMoveTo, 5 * Time.deltaTime);
            if(Vector2.Distance(transform.position,targetLocationToMoveTo) <= 0.1)
            {
                isMoving = false;
            }
        }
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

    private void OnMouseEnter()
    {
        highlightObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        highlightObject.SetActive(false);
    }

}
