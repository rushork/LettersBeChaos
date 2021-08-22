using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableLetter : MonoBehaviour
{
    [Tooltip("The scriptable object assigned to this letter.")] [SerializeField] private LetterSO letterScriptable;
    [Tooltip("The location for any generic stamp")] [SerializeField] private Transform leftStampLocation;
    [Tooltip("The location for 'trash' stamps")] [SerializeField] private Transform rightStampLocation;
    private SpriteRenderer sealRenderer;
    private Color32 sealColor;

    

    private void Awake()
    {
        sealRenderer = transform.Find("seal").GetComponent<SpriteRenderer>();
        
    }
}
