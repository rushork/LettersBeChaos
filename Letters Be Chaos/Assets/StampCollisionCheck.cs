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
            
            //for bombs
            if(letter.letterScriptable.nameString == "Bomb")
            {
                Explode();
            }



            if (letter.hasBeenSelected)
            {
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



                letter.StampWithColor(arm.GetColor());
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
            if(letterScript != null)
            {
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

                    letterScript.StampWithColor(letterScript.GetSealColor());
                }
                else
                {
                    letterScript.SetTarget(GameSettings.Instance.deleteLocation);
                    letterScript.StampWithColor(GameSettings.Instance.delete);
                }

                letterScript.SendForProcessing();
            }
        }
    }

}
