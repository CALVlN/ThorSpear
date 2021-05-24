using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimations : MonoBehaviour
{
    Vector3 buttonStartPos = new Vector3();
    Vector3 buttonDefaultPos = new Vector3();
    Vector3 desiredPos = new Vector3();
    public float animationPercent;
    float lerpTimer = 0f;
    bool hoveringOverButton = false;

    void Start() {
        buttonDefaultPos = transform.position;
    }

    public void OnHover() {
        buttonStartPos = transform.position;
        desiredPos = buttonStartPos;
        desiredPos.x = buttonStartPos.x + 30f;
        hoveringOverButton = true;
        lerpTimer = 0f;

        StartCoroutine("ButtonSelected");
    }
    IEnumerator ButtonSelected() {
        float lerpDuration = 0.6f;
        while (animationPercent < lerpDuration && hoveringOverButton) {

            lerpTimer += Time.deltaTime;

            animationPercent = lerpTimer / lerpDuration;
            animationPercent = Mathf.Sin(animationPercent * Mathf.PI * 0.5f);

            transform.position = Vector3.Lerp(buttonStartPos, desiredPos, animationPercent);

            yield return null;
        }
    }
    public void OnHoverExit() {
        desiredPos = buttonDefaultPos;
        hoveringOverButton = false;
        buttonStartPos = transform.position;
        lerpTimer = 0f;

        StopCoroutine("ButtonSelected");
        StartCoroutine("ButtonDeselected");
    }
    IEnumerator ButtonDeselected() {
        float lerpDuration = 0.6f;
        while (animationPercent > 0f && !hoveringOverButton) {

            lerpTimer += Time.deltaTime;

            animationPercent = lerpTimer / lerpDuration;
            animationPercent = Mathf.Sin(animationPercent * Mathf.PI * 0.5f);

            transform.position = Vector3.Lerp(buttonStartPos, desiredPos, animationPercent);
            //Debug.Log(desiredPos);

            yield return null;
        }
    }
}
