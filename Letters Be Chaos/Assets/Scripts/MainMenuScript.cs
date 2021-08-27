using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    public DisclaimerScript disclaimer;

    private bool disclaimerShown;

    public void PlayGame() {
       
        if (PlayerPrefs.GetInt("Highscore") > 0 || disclaimerShown) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        } else if (!disclaimerShown) {
            disclaimerShown = true;
            disclaimer.Disclaimer();
        }
    }

    public void MainMenu() {
        SceneManager.LoadScene(0);
    }

    public void MedalRoom() {
        SceneManager.LoadScene(4);
    }

}
