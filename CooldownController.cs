using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownController : MonoBehaviour
{
    public GameObject cooldownBar;
    public Slider slider;
    public bool cooldownBarActive;

    void Start() {

    }

    void Update() {
        if (slider.value < 2f && !cooldownBarActive) {
            cooldownBar.SetActive(true);
            cooldownBarActive = true;
        }
        else if (slider.value == 2f && cooldownBarActive) {
            cooldownBar.SetActive(false);
            cooldownBarActive = false;
        }
    }

    public void SetCooldown(float cooldown) {
        slider.value = 2f - cooldown;
    }
}