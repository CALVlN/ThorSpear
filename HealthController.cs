using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public Slider slider;

    public void SetHealth(float health) {
        slider.value = health;
    }
}
