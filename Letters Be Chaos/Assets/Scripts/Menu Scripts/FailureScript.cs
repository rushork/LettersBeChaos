using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailureScript : MonoBehaviour
{

    void Start()
    {
        int r = Random.Range(0, 2);
 
        switch (r) {
            case 0:
                AudioManager.Instance.Play("FASTER");
                break;
            case 1:
                AudioManager.Instance.Play("WORKHARDER");
                break;
            default:
                break;
        }
    }

}
