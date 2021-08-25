using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newLetter", menuName = "Letter Item")]
public class LetterSO : ScriptableObject
{
    [Header("Unique Information")]
    public string nameString;
    [Tooltip("Does this letter need a tracking number?")]public bool isTracked;
    [Tooltip("Does this letter ignore sorting?")]public bool isSpecial;

    [Header("Spawning Chances")]
    [Range(0, 1)] public float chanceToSpawnSealed;

    [Tooltip("What is the chance the postage stamp will be the wrong way up?")]
    [Range(0, 1)] public float chanceToSpawnStampFlipped;

    [Tooltip("The chance that this letter type will be stamped as intended on spawn")]
    [Range(0, 1)] public float chanceToBeCorrectlyStamped;


    [Tooltip("The chance that this letter will have an invalid color seal")]
    [Range(0, 1)] public float chanceForInvalidSealColor;

    [Tooltip("The chance that this letter will have Tracking Info")]
    [Range(0, 1)] public float chanceToBeTracked;
}
