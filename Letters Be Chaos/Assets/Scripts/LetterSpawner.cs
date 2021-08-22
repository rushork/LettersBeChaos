using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will manage where letters spawn and how they move.
/// For now, they can be spawned by clicking.
/// </summary>
public class LetterSpawner : MonoBehaviour
{
    [SerializeField] Transform letterPrefab_Basic;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(letterPrefab_Basic, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
        }
    }
}
