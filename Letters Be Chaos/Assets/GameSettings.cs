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
    [SerializeField] private Color32 red;
    [SerializeField] private Color32 blue;
    [SerializeField] private Color32 green;

    private void Awake()
    {
        //now you can use "GameSettings.Instance.(insert method name)" anywhere. 
        //Why use this instead of static classes? static classes are a pain in the ass.
        Instance = this;
    }
}
