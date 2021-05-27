using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimations : MonoBehaviour {
    Vector3 buttonStartPos = new Vector3();
    Vector3 buttonDefaultPos = new Vector3();
    Vector3 desiredPos = new Vector3();
    public float animationPercent;
    float lerpTimer = 0f;
    bool hoveringOverButton = false;

    bool startAnimationCompleted = false;

    void Start() {
        buttonDefaultPos = transform.position;
    }

    void Update() {
        /*float lerpDuration = 0.5f;
        while (!startAnimationCompleted) {
            Vector3 buttonStartRot = transform.eulerAngles;
            Vector3 buttonDesiredRot = Vector3.zero;
            lerpTimer = 0f;

            lerpTimer += Time.deltaTime;

            animationPercent = lerpTimer / lerpDuration;
            animationPercent = Mathf.Sin(animationPercent * Mathf.PI * 0.5f);

            transform.eulerAngles = Vector3.Slerp(buttonStartRot, buttonDesiredRot, animationPercent);

            // This should theoretically work.
            startAnimationCompleted = transform.eulerAngles == buttonDesiredRot ? startAnimationCompleted = true : startAnimationCompleted = false;
        }*/
    }

    public void OnHover() {
        buttonStartPos = transform.position;
        desiredPos = buttonDefaultPos;
        desiredPos.x = buttonDefaultPos.x - 20f;
        hoveringOverButton = true;
        lerpTimer = 0f;

        StopCoroutine("ButtonDeselected");
        StartCoroutine("ButtonSelected");
    }
    IEnumerator ButtonSelected() {
        float lerpDuration = 1f;
        while (lerpTimer < 1f && hoveringOverButton) {
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
        float lerpDuration = 1f;
        while (lerpTimer < 1f && !hoveringOverButton) {
            lerpTimer += Time.deltaTime;

            animationPercent = lerpTimer / lerpDuration;
            animationPercent = Mathf.Sin(animationPercent * Mathf.PI * 0.5f);

            transform.position = Vector3.Lerp(buttonStartPos, desiredPos, animationPercent);

            yield return null;
        }
    }
}
