using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour {
    // Hit flash
    [SerializeField] CanvasGroup healthFlashImage;
    public float flashOpacity = 1f;
    // Health stuff
    public float health = 100f;
    public bool hitByTurret = false;
    public HealthController healthBar;
    // Death screen stuff
    [SerializeField] GameObject deathScreen;
    bool defyingDeath = false;
    public static bool deathScreenActive;

    // Start is called before the first frame update
    void Start() {
        health = 100f;
        healthFlashImage = healthFlashImage.GetComponent<CanvasGroup>();
        deathScreen.SetActive(false);
        deathScreenActive = false;
    }

    // Update is called once per frame
    void Update() {
        if (hitByTurret) {
            health -= 10;
            hitByTurret = false;
            healthBar.SetHealth(health);
            StartCoroutine(HealthFlash());
        }
        // If player died
        if (health <= 0 && !defyingDeath) {
            deathScreen.SetActive(true);
            deathScreenActive = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
        IEnumerator HealthFlash() {
            flashOpacity = 0f;

            while (flashOpacity < 1f) {
                flashOpacity += Time.deltaTime * 10;
                healthFlashImage.alpha = flashOpacity;

                yield return new WaitForSeconds(0.0001f);
            }

            yield return new WaitForSeconds(0.1f);

            while (flashOpacity > 0f) {
                flashOpacity -= Time.deltaTime * 5;
                healthFlashImage.alpha = flashOpacity;

                yield return new WaitForSeconds(0.0001f);
            }

            healthFlashImage.alpha = 0f;
            /*StopCoroutine("HealthFlash");*/
        }
    }

    public void DefyDeath() {
        defyingDeath = true;
        deathScreen.SetActive(false);
        deathScreenActive = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        Debug.Log("DefyDeath");
    }
    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("Restart");
    }
}
