using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;


/// <summary>
/// Tracks the progress of medals. Some are persistent, some arent.
/// </summary>
public class MedalManager : MonoBehaviour
{

 


    public class OnLetterEventArgs
    {
        public InteractableLetter letter;
    }














    /// <summary>
    /// Count the letter into the medal system. This method will sort the letter and add values where necessary.
    /// </summary>
    /// <param name="letter"> The letter object </param> 
    /// <param name="auto"> Was it processed automatically? </param>
    /// <param name="valid"> Was this letter processing valid, did they sort it correctly? </param>
    public void CountLetter(InteractableLetter letter, bool auto, bool valid)
    {

    }
}




