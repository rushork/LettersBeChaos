using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampCollisionCheck : MonoBehaviour
{
    [SerializeField] StampingArm arm;
    [HideInInspector] public bool hasHitLetter;
    [SerializeField] private LetterSpawner spawner;
    public BoxCollider2D redZone;
    public BoxCollider2D greenZone;
    public BoxCollider2D blueZone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        InteractableLetter letter = collision.GetComponent<InteractableLetter>();
        bool avoidStandardProcessing = false;
        if (letter != null)
        {
            
            



            if (letter.hasBeenSelected)
            {

                //for bombs
                if (letter.letterScriptable.nameString == "Bomb")
                {
                    AudioManager.Instance.Play("Explosion");
                    Explode();
                }
                else if (letter.letterScriptable.nameString == "BombR")
                {
                    if (arm.GetColor().Equals(GameSettings.Instance.red))
                    {
                        AudioManager.Instance.Play("Explosion");
                        ExplodeAtRandom("red");
                        avoidStandardProcessing = true;
                    }
                    else
                    {
                        //misfire
                        letter.gameObject.GetComponent<LetterSelfDestruct>().DestructionTimerMax = 0.1f;
                        GameSettings.Instance.IncreaseRandom(10);
                        AudioManager.Instance.Play(AudioManager.Instance.returnIncorrect(5));

                    }
                    
                }
                else if (letter.letterScriptable.nameString == "BombB")
                {
                    if (arm.GetColor().Equals(GameSettings.Instance.blue))
                    {
                        AudioManager.Instance.Play("Explosion");
                        ExplodeAtRandom("blue");
                        avoidStandardProcessing = true;
                    }
                    else
                    {
                        //misfire
                        letter.gameObject.GetComponent<LetterSelfDestruct>().DestructionTimerMax = 0.1f;
                        GameSettings.Instance.IncreaseRandom(10);
                        AudioManager.Instance.Play(AudioManager.Instance.returnIncorrect(5));
                    }

                }
                else if (letter.letterScriptable.nameString == "BombG")
                {
                    if (arm.GetColor().Equals(GameSettings.Instance.green))
                    {
                        AudioManager.Instance.Play("Explosion");
                        ExplodeAtRandom("green");
                        avoidStandardProcessing = true;
                    }
                    else
                    {
                        //misfire
                        letter.gameObject.GetComponent<LetterSelfDestruct>().DestructionTimerMax = 0.1f;
                        GameSettings.Instance.IncreaseRandom(10);
                        AudioManager.Instance.Play(AudioManager.Instance.returnIncorrect(5));
                    }

                }
                else if (letter.letterScriptable.nameString == "Summon")
                {
                    AudioManager.Instance.Play("Explosion");
                    spawner.SummonBomb();
                }
                else if (letter.letterScriptable.nameString == "Trash")
                {
                    AudioManager.Instance.Play("Explosion");
                    EnableDeleteHighlights();
                    letter.gameObject.GetComponent<LetterSelfDestruct>().DestructionTimerMax = 0.1f;
                }
                else if (letter.letterScriptable.nameString == "Column")
                {
                    AudioManager.Instance.Play("Explosion");
                    ColumnSort();
                    letter.gameObject.GetComponent<LetterSelfDestruct>().DestructionTimerMax = 0.1f;
                }


                hasHitLetter = true;

                if (arm.GetColor().Equals(GameSettings.Instance.red))
                {
                    letter.SetTarget(GameSettings.Instance.redLocation);

                }
                else if (arm.GetColor().Equals(GameSettings.Instance.blue))
                {
                    letter.SetTarget(GameSettings.Instance.blueLocation);

                }
                else if (arm.GetColor().Equals(GameSettings.Instance.green))
                {
                    letter.SetTarget(GameSettings.Instance.greenLocation);
                    

                }
                else if (arm.GetColor().Equals(GameSettings.Instance.delete))
                {
                    letter.SetTarget(GameSettings.Instance.deleteLocation);


                }



                letter.StampWithColor(arm.GetColor(), arm.isStampingTracked);
                AudioManager.Instance.Play("InkyThud");

                if (avoidStandardProcessing)
                {
                    letter.usagePenaltyEnabled = false;
                    letter.SendForProcessing();
                }
                else
                {
                    letter.usagePenaltyEnabled = true;
                    letter.SendForProcessing();
                }
                

            }
        }
    }

    private void ColumnSort()
    {
        Collider2D[] letters = Physics2D.OverlapCircleAll(transform.position, 5f, 1 << LayerMask.NameToLayer("Letters"));

        foreach (Collider2D letter in letters)
        {
            InteractableLetter letterScript = letter.GetComponent<InteractableLetter>();
            if (letterScript != null)
            {

                if (letterScript.isValidOnArrival)
                {
                    if (letterScript.GetSealColor().Equals(GameSettings.Instance.red))
                    {
                        Vector2 extents = redZone.size / 2f;
                        Vector2 point = new Vector2(Random.Range(-extents.x, extents.x), Random.Range(-extents.y, extents.y) + redZone.bounds.center.y);
                        letterScript.SetTarget(point);

                    }
                    else if (letterScript.GetSealColor().Equals(GameSettings.Instance.green))
                    {
                        Vector2 extents = greenZone.size / 2f;
                        Vector2 point = new Vector2(Random.Range(-extents.x, extents.x), Random.Range(-extents.y, extents.y) + greenZone.bounds.center.y);
                        letterScript.SetTarget(point);
                    }
                    else if (letterScript.GetSealColor().Equals(GameSettings.Instance.blue))
                    {
                        Vector2 extents = blueZone.size / 2f;
                        Vector2 point = new Vector2(Random.Range(-extents.x, extents.x), Random.Range(-extents.y, extents.y) + blueZone.bounds.center.y);
                        letterScript.SetTarget(point);
                    }

                    letterScript.InitiateColumnSort();
                }
                
               
            }
        }
    }

    private void Explode()
    {
        Collider2D[] letters = Physics2D.OverlapCircleAll(transform.position, 5f, 1 << LayerMask.NameToLayer("Letters"));

        foreach(Collider2D letter in letters)
        {
            InteractableLetter letterScript = letter.GetComponent<InteractableLetter>();
            if(letterScript != null && !letterScript.letterScriptable.isSpecial)
            {

                Rigidbody2D body = letter.GetComponent<Rigidbody2D>();

                Vector2 dir = (letter.transform.position - transform.position) * 400f;
                
                body.AddForce(dir);

                if (letterScript.isValidOnArrival)
                {
                    if (letterScript.GetSealColor().Equals(GameSettings.Instance.red))
                    {

                        letterScript.SetTarget(GameSettings.Instance.redLocation);

                    }
                    else if (letterScript.GetSealColor().Equals(GameSettings.Instance.blue))
                    {
                        letterScript.SetTarget(GameSettings.Instance.blueLocation);

                    }
                    else if (letterScript.GetSealColor().Equals(GameSettings.Instance.green))
                    {
                        letterScript.SetTarget(GameSettings.Instance.greenLocation);


                    }
                    else if (letterScript.GetSealColor().Equals(GameSettings.Instance.delete))
                    {
                        letterScript.SetTarget(GameSettings.Instance.deleteLocation);


                    }

                    letterScript.StampWithColor(letterScript.GetSealColor(),arm.isStampingTracked);
                }
                else
                {
                    letterScript.SetTarget(GameSettings.Instance.deleteLocation);
                    letterScript.StampWithColor(GameSettings.Instance.delete, arm.isStampingTracked);
                }

                StartCoroutine(WaitFor(0.2f,letterScript, body));
                //letterScript.SendForProcessing();
            }
        }
    }

    //Will send all letters to the same color regardless. Must be stamped with the right color to work!
    private void ExplodeAtRandom(string color)
    {
        Collider2D[] letters = Physics2D.OverlapCircleAll(transform.position, 5f, 1 << LayerMask.NameToLayer("Letters"));

        foreach (Collider2D letter in letters)
        {
            InteractableLetter letterScript = letter.GetComponent<InteractableLetter>();
            if (letterScript != null && !letterScript.letterScriptable.isSpecial)
            {

                Rigidbody2D body = letter.GetComponent<Rigidbody2D>();

                Vector2 dir = (letter.transform.position - transform.position) * 400f;

                body.AddForce(dir);

                Color32 currentColor;
                Transform currentTransformTarget;
                if(color == "red")
                {
                    currentColor = GameSettings.Instance.red;
                    currentTransformTarget = GameSettings.Instance.redLocation;
                }
                else if (color == "green")
                {
                    currentColor = GameSettings.Instance.green;
                    currentTransformTarget = GameSettings.Instance.greenLocation;
                }
                else
                {
                    currentColor = GameSettings.Instance.blue;
                    currentTransformTarget = GameSettings.Instance.blueLocation;
                }

                letterScript.SetTarget(currentTransformTarget);
                letterScript.StampWithColor(currentColor, arm.isStampingTracked);

                StartCoroutine(WaitFor(0.2f, letterScript, body));
                //letterScript.SendForProcessing();
            }




        }
    }

    private void EnableDeleteHighlights()
    {
        Collider2D[] letters = Physics2D.OverlapCircleAll(transform.position, 5f, 1 << LayerMask.NameToLayer("Letters"));

        foreach (Collider2D letter in letters)
        {
            InteractableLetter letterScript = letter.GetComponent<InteractableLetter>();
            if (letterScript != null && !letterScript.letterScriptable.isSpecial)
            {

                if (!letterScript.isValidOnArrival)
                {
                    letterScript.ShowDeleteHighlight();
                }
            }




        }
    }

    private IEnumerator WaitFor(float time, InteractableLetter letterScript, Rigidbody2D body)
    {
        yield return new WaitForSeconds(time);
        if(body != null)
        {
            body.velocity = Vector2.zero;
            body.angularVelocity = 0;
        }
        
        letterScript.SendForProcessing();
    }

}
