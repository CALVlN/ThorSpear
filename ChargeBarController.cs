using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeBarController : MonoBehaviour
{
    public GameObject chargeBar;
    public Slider slider;
    public bool chargeBarActive;


    void Start() {

    }

    void Update() {
        if (slider.value > 0.00001f && !chargeBarActive) {
            chargeBar.SetActive(true);
            chargeBarActive = true;
        }
        else if (gameObject.GetComponent<CooldownController>().cooldownBarActive && chargeBarActive) {
            chargeBar.SetActive(false);
            chargeBarActive = false;
        }
    }

    public void SetCharge(float charge) {
        slider.value = charge;
    }
}
