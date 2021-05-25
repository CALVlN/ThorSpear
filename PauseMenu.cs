using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public static bool isPaused = false;
    public static int currentQuality = 2;

    /* OPTIONS MENU VARIABLES */
    [SerializeField] Slider sensitivitySlider;
    public float mouseSensitivity = 3f;

    // Start is called before the first frame update
    void Start() {
        pauseMenu.SetActive(false);
        if (!isPaused) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                ResumeGame();
            }
            else {
                PauseGame();
            }
        }
    }
    public void PauseGame() {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ResumeGame() {
        isPaused = false;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (isPaused) {
            isPaused = false;
            Time.timeScale = 1;
        }
    }
    public void MainMenu() {
        SceneManager.LoadScene("Main Menu");
        // idk if this is needed. Probably not.
        if (PauseMenu.isPaused) {
            PauseMenu.isPaused = false;
            Time.timeScale = 1;

            currentQuality = QualitySettings.GetQualityLevel();
            if (currentQuality < 3) {
                QualitySettings.SetQualityLevel(2, true);
            }
        }
    }

    public void Options() {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    /* OPTIONS MENU */
    public void Back() {
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void MouseSensitivityChange() {
        // Set mouse sensitivity.
        mouseSensitivity = sensitivitySlider.value;
    }
    
    /* GRAPHICS OPTIONS */
    public void High() {
        QualitySettings.SetQualityLevel(3, true);
    }
    public void Medium() {
        QualitySettings.SetQualityLevel(2, true);
    }
    public void Low() {
        QualitySettings.SetQualityLevel(1, true);
    }
    public void Why() {
        QualitySettings.SetQualityLevel(0, true);
    }
}
