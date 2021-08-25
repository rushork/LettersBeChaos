using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampCollisionCheck : MonoBehaviour
{
    [SerializeField] StampingArm arm;
    [HideInInspector] public bool hasHitLetter;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        InteractableLetter letter = collision.GetComponent<InteractableLetter>();
        
        if (letter != null)
        {
            
            



            if (letter.hasBeenSelected)
            {

                //for bombs
                if (letter.letterScriptable.nameString == "Bomb")
                {
                    Explode();
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
                letter.SendForProcessing();

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

    private IEnumerator WaitFor(float time, InteractableLetter letterScript, Rigidbody2D body)
    {
        yield return new WaitForSeconds(time);
        body.velocity = Vector2.zero;
        body.angularVelocity = 0;
        letterScript.SendForProcessing();
    }

}
