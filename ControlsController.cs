using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsController : MonoBehaviour {

    [SerializeField] GameObject controlsMenu;
    bool controlsShown = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && controlsShown) {
            controlsMenu.SetActive(false);
            controlsShown = false;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && !controlsShown) {
            controlsMenu.SetActive(true);
            controlsShown = true;
        }
    }
}
