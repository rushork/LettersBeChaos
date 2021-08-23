using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextRevealer : MonoBehaviour
{

    public AudioSource sound;
    private TextMeshProUGUI tmp;

    IEnumerator Start() {
        tmp = GetComponent<TextMeshProUGUI>();
        tmp.ForceMeshUpdate();

        int totalVisibleChars = tmp.textInfo.characterCount;
        int counter = 0;
        int visibleCount = 0;

        while (visibleCount != totalVisibleChars) { // Loop till the entire story is visible
            
            visibleCount = counter % (totalVisibleChars+1); 
            tmp.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalVisibleChars) {
                yield return new WaitForSeconds(0.05f);
            }

            counter += 1;
            yield return new WaitForSeconds(0.05f);
        }
    }

}

