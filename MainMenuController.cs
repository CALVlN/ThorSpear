using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void NewGame() {
        SceneManager.LoadScene("NewMovement");
        if (PauseMenu.isPaused) {
            PauseMenu.isPaused = false;
            Time.timeScale = 1;
        }
        QualitySettings.SetQualityLevel(PauseMenu.currentQuality, true);
    }

    public void Quit() {
        Application.Quit();
    }
}
