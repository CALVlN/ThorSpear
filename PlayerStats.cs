using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour {
    // Hit flash
    [SerializeField] CanvasGroup healthFlashImage;
    [SerializeField] CanvasGroup healthPulseImage;
    public float flashOpacity = 1f;
    float pulseOpacity = 0f;
    // Health stuff
    public float health = 100f;
    public bool hitByTurret = false;
    public HealthController healthBar;
    // Death screen stuff
    [SerializeField] GameObject deathScreen;
    [SerializeField] GameObject scoreGameObject;
    bool defyingDeath = false;
    public static bool deathScreenActive;
    // Hit effect
    float targetIntensity;
    float pulsingTimer = 0f;
    float minIntensity = 0.5f;
    float pulseSpeed = 1f;

    // Start is called before the first frame update
    void Start() {
        health = 100f;
        healthFlashImage = healthFlashImage.GetComponent<CanvasGroup>();
        deathScreen.SetActive(false);
        deathScreenActive = false;
        pulsingTimer = 0f;
    }

    // Update is called once per frame
    void Update() {
        // If player died
        if (health <= 0 && !defyingDeath) {
            deathScreen.SetActive(true);
            deathScreenActive = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
        pulsingTimer += Time.deltaTime;
        if (pulsingTimer >= 0.2f && healthPulseImage.alpha > 0f) {
            flashOpacity = 0f;
            healthPulseImage.alpha = flashOpacity;
        }
    }
    public void HitByTurret() {
        health -= 10;
        hitByTurret = false;
        healthBar.SetHealth(health);
        StartCoroutine(HealthFlash());
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
    }
    public void hitByEnemy() {
        // Time.deltaTime might be needed to keep health framerate independent. Otherwise people with better computers would be at a disadvantage. Probably.
        health -= 10f * Time.deltaTime;
        healthBar.SetHealth(health);
        pulsingTimer = 0f;

        pulseOpacity = Mathf.MoveTowards(pulseOpacity, targetIntensity, Time.deltaTime * pulseSpeed);

        if (pulseOpacity >= 1f) {
            pulseOpacity = 1f;
            targetIntensity = minIntensity;
        }
        else if (pulseOpacity <= minIntensity) {
            pulseOpacity = minIntensity;
            targetIntensity = 1f;
        }

        healthPulseImage.alpha = pulseOpacity;

        /*if (pulseOpacity < 1f) {
            pulseOpacity += Time.deltaTime * 4;
            healthPulseImage.alpha = pulseOpacity;

            Debug.Log("PulseUp");
        }

        if (flashOpacity > 0.2f) {
            pulseOpacity -= Time.deltaTime * 4;
            healthPulseImage.alpha = pulseOpacity;

            Debug.Log("PulseDown");
        }*/
    }
    public void DefyDeath() {
        defyingDeath = true;
        deathScreen.SetActive(false);
        deathScreenActive = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        scoreGameObject.SetActive(false);
    }
    public void Restart() {
        health = 100f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("Restart");
    }
}
