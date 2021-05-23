using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearAttack : MonoBehaviour {
    [SerializeField] Transform player;
    [SerializeField] Transform playerCamera;
    [SerializeField] Transform weaponTargetPosition;
    [SerializeField] Transform aimTarget;
    [SerializeField] Transform overShoulderTarget;
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
    float timeElapsed = 0f;
    bool readyToThrow = false;
    bool initiateAttack = false;
    float lerpPercent = 0f;
    float attackCooldown;
    float throwMultiplier = 0f;
    float holdFor = 0f;
    bool hasHeld = false;
    float primaryAttackingTime = 0f;
    //bool addDrag = false;
    //float dragTime = 0f;

    // Rotation related variables.
    /*Vector3 spearCurrentPos = new Vector3();
    Vector3 spearTargetPos = new Vector3();
    Vector3 spearOldPos = new Vector3();*/

    // Start is called before the first frame update
    void Start() {
        Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
    }

    // Update is called once per frame
    void Update() {
        if (!PauseMenu.isPaused) {
            if (Input.GetMouseButtonDown(0) && CanAttack() && !initiateAttack || initiateAttack) {
                PrimaryAttack();
            }
        }
        if (primaryAttacking) {
            primaryAttackingTime += Time.deltaTime;

            if (primaryAttackingTime >= 1f && !gameObject.GetComponent<SummonSpear>().pickingUpItem && !gameObject.GetComponent<SummonSpear>().holdingItem) {
                primaryAttacking = false;
                primaryAttackingTime = 0f;

                // Turn on gravity.
                spearRigidbody.useGravity = true;
            }
            else if (gameObject.GetComponent<SummonSpear>().pickingUpItem) {
                primaryAttacking = false;
                primaryAttackingTime = 0f;
            }
        }
    }
    // Return true if below conditions are met, in which case attacking should be allowed.
    bool CanAttack() {
        bool holdingItem = gameObject.GetComponent<SummonSpear>().holdingItem;
        bool pickingUpItem = gameObject.GetComponent<SummonSpear>().pickingUpItem;
        bool airControlling = gameObject.GetComponent<SummonSpear>().airControlling;

        // If holdingItem and !pickingUpItem from SpearPickup.cs, and !attacking and !initiateAttack from this script, return true. Else, return false.
        return (holdingItem && !pickingUpItem && !primaryAttacking && !airControlling && !initiateAttack);
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
        initiateAttack = true;
        Vector3 startPos = weaponTargetPosition.position;
        Vector3 endPos = overShoulderTarget.position;

        float lerpDuration = 1;

        if (holdFor > 0f) {
            holdFor += Time.deltaTime;
            if (holdFor > 0.5f) {
                attackCooldown = 0.01f;
                holdFor = 0f;
            }
        }

        if (!readyToThrow && holdFor == 0f) {
            lerpPercent = timeElapsed / lerpDuration; // float t = time / duration 
            lerpPercent = lerpPercent * lerpPercent * (3f - 2f * lerpPercent);


            transform.position = Vector3.Lerp(startPos, endPos, lerpPercent);
            timeElapsed += Time.deltaTime;

            if (lerpPercent > 0.9999f && !hasHeld || attackCooldown > 0f && !hasHeld) {
                hasHeld = true;
                holdFor = 0.01f;
            }
            if (attackCooldown > 0f) { attackCooldown += Time.deltaTime; }
            if (attackCooldown > 2f) {
                timeElapsed = 0f;
                attackCooldown = 0f;
                hasHeld = false;
                initiateAttack = false;
            }
        }

        if (!PauseMenu.isPaused) {
            if (lerpPercent > 0.2f && Input.GetMouseButtonDown(0)) {
                throwMultiplier = lerpPercent;
                Debug.Log("ran");
                readyToThrow = true;
                holdFor = 0f;
            }
        }
        
        if (readyToThrow) {
            transform.rotation = weaponTargetPosition.rotation;
            primaryAttacking = true;
            float throwStrength = gameObject.GetComponent<SummonSpear>().throwStrength * lerpPercent;
            gameObject.GetComponent<SummonSpear>().holdingItem = false;
            gameObject.GetComponent<SummonSpear>().pickingUpItem = false;
            gameObject.GetComponent<SummonSpear>().startDistCalled = false;

            throwStrength = throwStrength * throwMultiplier;

            // Set weapon head and shaft colliders to no longer be triggers so they interact with other colliders.
            spearShaft.isTrigger = false;
            spearHammerHead.isTrigger = false;
            // Reset percentOrSmth to zero.
            gameObject.GetComponent<SummonSpear>().percentOrSmthn = 0f;

            /* ADDING VELOCITY TO DROPPED WEAPON - maybe use a function here?*/
            Vector3 playerVelocity = gameObject.GetComponent<SummonSpear>().playerController.velocity;
            // Add player speed to weapon.
            //spearRigidbody.velocity = playerVelocity;
            // Add extra veocity.
            Vector3 directionVector = (aimTarget.position - transform.position).normalized;
            spearRigidbody.AddForce(throwStrength * 1.5f * directionVector, ForceMode.Impulse);
            spearRigidbody.AddForce(-throwStrength / 10 * playerCamera.transform.up, ForceMode.Impulse);
            // Set spear as child of the player's parent.
            transform.parent = player.transform.parent;
            readyToThrow = false;
            initiateAttack = false;
            hasHeld = false;
            timeElapsed = 0f;
        }
    }
}
