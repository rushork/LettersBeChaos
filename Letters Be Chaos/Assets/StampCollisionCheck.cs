using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampCollisionCheck : MonoBehaviour
{
    [SerializeField] StampingArm arm;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        InteractableLetter letter = collision.GetComponent<InteractableLetter>();
        
        if (letter != null)
        {
            
            if (letter.hasBeenSelected)
            {


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
}
