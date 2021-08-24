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
    [Tooltip("The location for any generic stamp")] [SerializeField] private SpriteRenderer mechanicalStampZoneRenderer;

    private SpriteRenderer mySpriteRenderer;
    private SpriteRenderer sealRenderer;
    private SpriteRenderer stampRenderer;
    private GameObject highlightObject;
    private Color32 sealColor;
    [HideInInspector]public Color32 colorStampedWith; // this is the colour that it has been stamped with by the player
    private string trackingNumber; //this is a string because it contains letters and numbers.
    private bool highlightable; //can this be highlighted?
    private bool isBeingProcessed;
    private bool isHighlighted;
    [HideInInspector] public bool hasBeenSelected;

    //these values are decided after spawning
    public bool hasTrackingInfo;
    public bool isUpsideDown;
    public bool isCorrectColor;
    public bool isSealed;
    public bool isPostageStamped;
    public bool isValidOnArrival; // letters are only valid if they have an upright stamp, a valid coloured seal and no defects
    

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
        if (isMoving && !isBeingProcessed)
        {
            transform.position = Vector3.Lerp(transform.position, targetLocationToMoveTo, 5f * Time.deltaTime);
            if(Vector2.Distance(transform.position,targetLocationToMoveTo) <= 0.1)
            {
                isMoving = false;

                highlightable = true;


                gameObject.layer = LayerMask.NameToLayer("Letters");
            }
        }
        if (isBeingProcessed)
        {

            if(transform.rotation.z != 0)
            {

                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, 0f), 4.0f * Time.deltaTime);
            }

            if (isMoving)
            {
                transform.position = Vector3.Lerp(transform.position, targetLocationToMoveTo, 8f * Time.deltaTime);
                if (Vector2.Distance(transform.position, targetLocationToMoveTo) <= 0.1)
                {
                    gameObject.layer = LayerMask.NameToLayer("Letters");
                    isMoving = false;
                }
            }
            else
            {
                GameSettings.Instance.ProcessLetter(this);
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

        randomVal = Random.Range(0f, 1f);

        //chance to flip
        if (randomVal <= letterScriptable.chanceToSpawnStampFlipped)
        {
            stampRenderer.flipY = true;
            isUpsideDown = true;
        }

        randomVal = Random.Range(0f, 1f);

        //chance to spawn sealed
        if (randomVal <= letterScriptable.chanceToSpawnSealed)
        {
            //set the seal to visible if it's sealed.
            sealRenderer.gameObject.SetActive(true);
            isSealed = true;
        }

        randomVal = Random.Range(0f, 1f);

        if (randomVal <= letterScriptable.chanceToBeCorrectlyStamped)
        {
            //set the seal to visible if it's sealed.
            stampRenderer.gameObject.SetActive(true);
            isPostageStamped = true;
        }



        if(isSealed && !isUpsideDown && isPostageStamped)
        {
            isValidOnArrival = true;
        }
        else
        {
            isValidOnArrival = false;
        }

    }

    public void StampWithColor(Color32 colorStamped)
    {
        colorStampedWith = colorStamped;
        mechanicalStampZoneRenderer.color = colorStampedWith;
        mechanicalStampZoneRenderer.gameObject.SetActive(true);
        if (colorStamped.Equals(sealColor))
        {
            isCorrectColor = true;
        }
        else
        {
            isCorrectColor = false;
        }

        //if not valid and marked with orange, its the correct color.
        if(!isValidOnArrival && colorStampedWith.Equals(GameSettings.Instance.delete))
        {
            isCorrectColor = true;
        }
    }

    public void SetTarget(Transform pos)
    {
        targetLocationToMoveTo = pos.position;
    }

    public void SendForProcessing()
    {
        highlightable = false;
        highlightObject.SetActive(false);
        isHighlighted = false;
        

        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        isMoving = true;
        isBeingProcessed = true; //now it will not become highlightable again
    }

    private void OnMouseEnter()
    {
        if (highlightable)
        {
            isHighlighted = true;
            highlightObject.SetActive(true);
        }
        
    }

    private void OnMouseExit()
    {
        if (highlightable)
        {
            isHighlighted = false;
            highlightObject.SetActive(false);
        }
   
    }

    private void OnMouseDown()
    {
        if (isHighlighted && GameSettings.Instance.arm.IsIdle())
        {
            hasBeenSelected = true;
        }
    }


    public bool GetHighlightedStatus()
    {
        return isHighlighted;
    }

    public bool CompareSealColor(Color32 otherColor)
    {
        if (isSealed)
        {
            if(otherColor.Equals(sealColor))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public Color32 GetSealColor()
    {
        return sealColor;
    }

}
