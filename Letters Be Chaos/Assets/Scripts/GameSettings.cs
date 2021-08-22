using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class will hold all information on the game settings, stuff that other scripts will need access to.
 */
public class GameSettings : MonoBehaviour
{

    public static GameSettings Instance { get; private set; }


    [Header("Letter Colors: Decided by random chance")]
    [SerializeField] public Color32 red;
    [SerializeField] public Color32 blue;
    [SerializeField] public Color32 green;

    [Header("Letter color chances")]
    [Range(0,1)] public float redChance;
    [Range(0, 1)] public float greenChance;
    [Range(0, 1)] public float blueChance;


    private void Awake()
    {
        //now you can use "GameSettings.Instance.(insert method name)" anywhere. 
        //Why use this instead of static classes? static classes are a pain in the ass.
        Instance = this;
    }
}
