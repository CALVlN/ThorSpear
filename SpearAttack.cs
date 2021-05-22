using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearAttack : MonoBehaviour {
    [SerializeField] Transform player;
    [SerializeField] Transform playerCamera;
    //[SerializeField] Transform weaponTargetPosition;
    [SerializeField] Transform spear;
    [SerializeField] Rigidbody spearRigidbody;
    [SerializeField] Rigidbody enemyRigidbody;
    // Collider variables
    [SerializeField] Collider spearShaft;
    [SerializeField] Collider spearHammerHead;
    [SerializeField] Renderer enemyRenderer;
    [SerializeField] Material deadRobotMaterial;
    [SerializeField] Transform robotPrefab;

    // float weaponDamage = 10f;
    bool primaryAttacking = false;
    float primaryAttackingTime = 0f;
    //bool addDrag = false;
    //float dragTime = 0f;

    // Rotation related variables.
    /*Vector3 spearCurrentPos = new Vector3();
    Vector3 spearTargetPos = new Vector3();
    Vector3 spearOldPos = new Vector3();*/

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0) && CanAttack()) {
            PrimaryAttack();
        }
        if (primaryAttacking) {
            primaryAttackingTime += Time.deltaTime;

            if (primaryAttackingTime >= 1f && !gameObject.GetComponent<SummonSpear>().pickingUpItem && !gameObject.GetComponent<SummonSpear>().holdingItem) {
                primaryAttacking = false;
                primaryAttackingTime = 0f;

                // Turn on gravity.
                spearRigidbody.useGravity = true;

                // Temporarily add drag to the spear to slow it down.
                //addDrag = true;
            }
            else if (gameObject.GetComponent<SummonSpear>().pickingUpItem) {
                primaryAttacking = false;
                primaryAttackingTime = 0f;
            }
        }
        /*if (addDrag) {
            primaryAttacking = false;
            primaryAttackingTime = 0f;
            dragTime += Time.deltaTime;
            //Debug.Log(dragTime);
            spearRigidbody.drag = 2f;
            spearRigidbody.angularDrag = 2f;

            //Debug.Log("Add drag");

            if (spearRigidbody.useGravity && addDrag) {
                // Turn off gravity.
                spearRigidbody.useGravity = false;
            }

            if (dragTime >= 1) {
                Debug.Log("Disable drag.");
                spearRigidbody.drag = 0f;
                spearRigidbody.angularDrag = 0.05f;
                addDrag = false;
                dragTime = 0f;
                // Turn on gravity.
                spearRigidbody.useGravity = true;
            }
        }*/
    }
    // Return true if below conditions are met, in which case attacking should be allowed.
    bool CanAttack() {
        bool holdingItem = gameObject.GetComponent<SummonSpear>().holdingItem;
        bool pickingUpItem = gameObject.GetComponent<SummonSpear>().pickingUpItem;
        bool airControlling = gameObject.GetComponent<SummonSpear>().airControlling;

        // If holdingItem and !pickingUpItem from SpearPickup.cs, and !attacking from this script, return true. Else, return false.
        return (holdingItem && !pickingUpItem && !primaryAttacking && !airControlling);
    }
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Enemy") && collision.relativeVelocity.magnitude > 10f) {
            // Possibly add stuff to make the robot explode into pieces and then disappear later.
            collision.gameObject.GetComponentInChildren<MeshRenderer>().material = deadRobotMaterial;
            //enemyRenderer.GetComponent<MeshRenderer>().material = deadRobotMaterial;
            collision.rigidbody.constraints = RigidbodyConstraints.None;
            Instantiate(robotPrefab, new Vector3(-1.8f, 6.3f, 4.4f), Quaternion.identity);
        }
    }
    private void OnTriggerEnter(Collider other) {

    }

    // Attack and do 10 damage to all enemies hit by the hammer.
    void PrimaryAttack() {
        primaryAttacking = true;
        float throwStrength = gameObject.GetComponent<SummonSpear>().throwStrength;
        gameObject.GetComponent<SummonSpear>().holdingItem = false;
        gameObject.GetComponent<SummonSpear>().pickingUpItem = false;
        gameObject.GetComponent<SummonSpear>().startDistCalled = false;

        // Set weapon head and shaft colliders to no longer be triggers so they interact with other colliders.
        spearShaft.isTrigger = false;
        spearHammerHead.isTrigger = false;

        // Reset percentOrSmth to zero.
        gameObject.GetComponent<SummonSpear>().percentOrSmthn = 0f;

        /* ADDING VELOCITY TO DROPPED WEAPON - maybe use a function here?*/
        Vector3 playerVelocity = gameObject.GetComponent<SummonSpear>().playerController.velocity;

        // Add player speed to item.
        spearRigidbody.velocity = playerVelocity;

        // Add extra veocity in the direction the camera is facing.
        spearRigidbody.AddForce(throwStrength * 2 * playerCamera.transform.forward, ForceMode.Impulse);
        spearRigidbody.AddForce(throwStrength / 30 * playerCamera.transform.up, ForceMode.Impulse);

        // Set spear as child of the player's parent.
        transform.parent = player.transform.parent;
    }
}
