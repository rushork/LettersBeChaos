using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableLetter : MonoBehaviour
{


    public static InteractableLetter CreateLetter(Transform prefab,Transform origin,Vector3 targetLocation, float zPos)
    {
        
        AudioManager.Instance.Play("Thwoomp");
        Transform interactableLetterTransform = Instantiate(prefab, new Vector3(origin.position.x, origin.position.y, zPos), Quaternion.AngleAxis(Random.Range(-20,20), Vector3.forward));
        InteractableLetter letter = interactableLetterTransform.GetComponent<InteractableLetter>();
        letter.targetLocationToMoveTo = new Vector3(targetLocation.x, targetLocation.y, zPos);


        return letter;
    }





    [Tooltip("The scriptable object assigned to this letter.")] public LetterSO letterScriptable;
    [Tooltip("The location for any generic stamp")] [SerializeField] private SpriteRenderer mechanicalStampZoneRenderer;
    [SerializeField] private Sprite trackedStamp;
    [SerializeField] private Sprite trackedPostageStamp;

    private SpriteRenderer mySpriteRenderer;
    private SpriteRenderer sealRenderer;
    private SpriteRenderer stampRenderer;
    private GameObject highlightObject;
    private GameObject highlightObjectDelete;
    private Color32 sealColor;
    [HideInInspector]public Color32 colorStampedWith; // this is the colour that it has been stamped with by the player
    private string trackingNumber; //this is a string because it contains letters and numbers.
    private bool highlightable; //can this be highlighted?
    private bool isBeingProcessed;
    private bool isHighlighted;
    private bool isBeingColumnSorted;
    [HideInInspector] public bool usagePenaltyEnabled;
    [HideInInspector] public bool hasBeenSelected;


    //these values are decided after spawning
    public bool hasTrackingInfo;
    public bool hasBeenTrackStamped;
    public bool isUpsideDown;
    public bool isCorrectColorCombo;
    public bool isSealed;
    public bool isPostageStamped;
    public bool hasInvalidColor;
    public bool isValidOnArrival; // letters are only valid if they have an upright stamp, a valid coloured seal and no defects

    public bool wasSortedByAutoSort;
    public bool wasSortedByColourBomb;

    //for spawning
    private Vector3 targetLocationToMoveTo;
    private bool isMoving = true;

    private void Awake()
    {
        sealRenderer = transform.Find("Seal").GetComponent<SpriteRenderer>();
        stampRenderer = transform.Find("PostageStamp").GetComponent<SpriteRenderer>();
        mySpriteRenderer = transform.Find("Letter").GetComponent<SpriteRenderer>();
        highlightObject = transform.Find("Highlight").gameObject;
        highlightObjectDelete = transform.Find("deleteHighlight").gameObject;
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
                AudioManager.Instance.Play("PaperLand");
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


        if (isBeingColumnSorted)
        {
            transform.position = Vector3.Lerp(transform.position, targetLocationToMoveTo, 6f * Time.deltaTime);
            if (Vector2.Distance(transform.position, targetLocationToMoveTo) <= 0.1)
            {
                gameObject.layer = LayerMask.NameToLayer("Letters");
                isBeingColumnSorted = false;
            }
        }
    }

    public void InitiateColumnSort()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        isBeingColumnSorted = true;
    }

    public void ShowDeleteHighlight()
    {
        highlightObjectDelete.gameObject.SetActive(true);
    }

    /// <summary>
    /// Handles the color, shape, size and other randomized attributes of a letter.
    /// </summary>
    private void HandleRandomVariations()
    {
        //get the random value 
        float randomVal = Random.Range(0f, 1f);

        //is it stamped?
        if (GameSettings.Instance.noStampAllowed)
        {
            if (randomVal <= letterScriptable.chanceToBeCorrectlyStamped)
            {
                //set the seal to visible if it's sealed.
                stampRenderer.gameObject.SetActive(true);
                isPostageStamped = true;
            }



        }
        else
        {
            if (!letterScriptable.isSpecial)
            {
                stampRenderer.gameObject.SetActive(true);
                isPostageStamped = true;
            }
        }
        

        randomVal = Random.Range(0f, 1f);


        if (GameSettings.Instance.trackingAllowed)
        {
            if (letterScriptable.isTracked && isPostageStamped)
            {
                if (randomVal <= letterScriptable.chanceToBeTracked)
                {
                    stampRenderer.sprite = trackedPostageStamp;
                    hasTrackingInfo = true;

                }


            }
        }
        

        randomVal = Random.Range(0f, 1f);

        if (GameSettings.Instance.invalidColorAllowed)
        {
            if (randomVal <= letterScriptable.chanceForInvalidSealColor)
            {
                sealColor = GameSettings.Instance.GetRandomColor();
                hasInvalidColor = true;
            }
            else
            {
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
            }
        }
        else
        {
            if(GameSettings.Instance.secondColorAllowed)
            {
                if (randomVal <= GameSettings.Instance.redChance)
                {
                    if (GameSettings.Instance.thirdColorAllowed)
                    {
                        sealColor = GameSettings.Instance.red;
                    }
                    else
                    {
                        if (randomVal <= GameSettings.Instance.greenChance)
                        {
                            sealColor = GameSettings.Instance.green;
                        }
                        else
                        {

                            sealColor = GameSettings.Instance.blue;
                        }
                    }
                    
                }
                else if (randomVal > GameSettings.Instance.redChance && randomVal <= GameSettings.Instance.greenChance)
                {
                    sealColor = GameSettings.Instance.green;
                }
                else
                {
                    
                    sealColor = GameSettings.Instance.blue;
                }
            }
            else
            {
                sealColor = GameSettings.Instance.green;
            }
        }

        

        

        sealRenderer.color = sealColor;

        randomVal = Random.Range(0f, 1f);

        //chance to flip
        if (GameSettings.Instance.upsideStampAllowed)
        {
            if (randomVal <= letterScriptable.chanceToSpawnStampFlipped)
            {
                stampRenderer.flipY = true;
                isUpsideDown = true;
            }

        }
        

        randomVal = Random.Range(0f, 1f);

        //chance to spawn sealed
        if (GameSettings.Instance.noSealAllowed)
        {
            if (randomVal <= letterScriptable.chanceToSpawnSealed)
            {
                //set the seal to visible if it's sealed.
                sealRenderer.gameObject.SetActive(true);
                isSealed = true;
            }
        }
        else
        {
            //as long as it isnt a special letter, it should be active.
            if (!letterScriptable.isSpecial)
            {
                sealRenderer.gameObject.SetActive(true);
                isSealed = true;
            }
            
        }
        

        



        if(isSealed && !isUpsideDown && isPostageStamped && !hasInvalidColor)
        {
            isValidOnArrival = true;
        }
        else
        {
            isValidOnArrival = false;
        }

    }

    public void StampWithColor(Color32 colorStamped, bool trackStampEnabled)
    {
        colorStampedWith = colorStamped;
        mechanicalStampZoneRenderer.color = colorStampedWith;
        mechanicalStampZoneRenderer.gameObject.SetActive(true);

        

        if (colorStamped.Equals(sealColor))
        {
            isCorrectColorCombo = true;
        }
        else
        {
            isCorrectColorCombo = false;
        }

        if (letterScriptable.isTracked && trackedPostageStamp && trackStampEnabled)
        {
            mechanicalStampZoneRenderer.sprite = trackedStamp;
            hasBeenTrackStamped = true;
        }
        else
        {
            hasBeenTrackStamped = false;
        }

        //if not valid and marked with orange, its the correct color. will override tracking
        if (!isValidOnArrival && colorStampedWith.Equals(GameSettings.Instance.delete))
        {
            isCorrectColorCombo = true;
        }


        
    }

    public void SetTarget(Transform pos)
    {
        targetLocationToMoveTo = pos.position;
    }
    public void SetTarget(Vector3 location)
    {
        targetLocationToMoveTo = location;
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
