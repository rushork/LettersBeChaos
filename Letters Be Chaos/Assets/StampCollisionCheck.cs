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
            
            if (letter.GetHighlightedStatus())
            {
                
                if(letter.CompareSealColor(arm.GetColor()))
                {
                    if (letter.GetSealColor().Equals(GameSettings.Instance.red))
                    {
                        ScoreManager.Instance.AddPoints(5);
                    }
                    else if (letter.GetSealColor().Equals(GameSettings.Instance.blue))
                    {
                        ScoreManager.Instance.AddPoints(2);
                    }
                    else if (letter.GetSealColor().Equals(GameSettings.Instance.green))
                    {
                        ScoreManager.Instance.AddPoints(1);
                    }
                }

                Destroy(letter.gameObject);
            }
        }
    }
}
