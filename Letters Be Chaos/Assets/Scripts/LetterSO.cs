using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newLetter", menuName = "Letter Item")]
public class LetterSO : ScriptableObject
{
    [Header("Unique Information")]
    public string nameString;
    [Tooltip("Does this letter need a tracking number?")]public bool isTracked;

    [Header("Spawning Chances")]
    [Range(0, 1)] public float chanceToSpawnSealed;

    [Tooltip("What is the chance the postage stamp will be the wrong way up?")]
    [Range(0, 1)] public float chanceToSpawnStampFlipped;

    [Tooltip("The chance that this letter type will be stamped as intended on spawn")]
    [Range(0, 1)] public float chanceToBeCorrectlyStamped;
    

    
}
