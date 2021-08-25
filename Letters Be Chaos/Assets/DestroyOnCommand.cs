using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCommand : MonoBehaviour
{
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
