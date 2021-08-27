using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisclaimerScript : MonoBehaviour
{

    public GameObject disclaimer;

    void Start() {
        disclaimer.SetActive(false);
    }

    public void Disclaimer() {
        disclaimer.SetActive(true);
    }

}
