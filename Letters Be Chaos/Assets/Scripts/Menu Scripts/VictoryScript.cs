using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScript : MonoBehaviour
{

    void Start()
    {
        int r = Random.Range(0, 2);
 
        switch (r) {
            case 0:
                AudioManager.Instance.Play("CONTINUE");
                break;
            case 1:
                AudioManager.Instance.Play("NATIONPROUD");
                break;
            default:
                break;
        }
    }

}
