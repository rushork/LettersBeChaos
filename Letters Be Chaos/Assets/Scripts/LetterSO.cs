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
    [Tooltip("What is the chance the seal will be the wrong way up?")] [Range(0, 1)] public float chanceToSpawnFlipped;
    [Tooltip("The chance that this letter type will be stamped as intended on spawn")]
    public float chanceToBeCorrectlyStamped;
    [Tooltip("The chance of this letter having a seal with this colour")]
    [Header("Colour Chances")]
    [Range(0, 1)] public float chanceGreen;
    [Range(0, 1)] public float chanceRed;
    [Range(0, 1)] public float chanceBlue;
}
