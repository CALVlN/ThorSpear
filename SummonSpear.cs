using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonSpear : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform playerCamera;
    [SerializeField] Transform weaponTargetPosition;
    [SerializeField] Transform spear;
    [SerializeField] Rigidbody spearRigidbody;

    public bool holdingItem = false;
    public bool pickingUpItem = false;
    float timer = 0f;
    float delayAmount = 0f;
    public float throwStrength = 70f;

    Vector3 playerVelocity = new Vector3();
    public PlayerController playerController;

    // Lerp variables
    public Vector3 spearGroundPos = new Vector3();
    public Vector3 spearTargetPos = new Vector3();
    public Quaternion spearGroundRot = new Quaternion();
    public Quaternion spearTargetRot = new Quaternion();
    public float percentOrSmthn = 0f;
    /*double itemPickupStartTime = 0;
    double itemArrivesAfter = 0;
    double itemTravelTime = 0;*/

    // SmoothDamp-related variables
    Vector3 spearCurrentPos = new Vector3();
    Vector3 spearOldPos = new Vector3();
    float smoothTimeChange = 0.005f;

    // Vector3 spearOlderPos = new Vector3();
    public float rotationSpeed = 1f;

    // Collider variables
    [SerializeField] Collider spearShaft;
    [SerializeField] Collider spearHammerHead;

    // hammerAirControlVariables
    [SerializeField] Transform airControlDesiredPos;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame.
    void Update() {
        // if q is pressed and the player is holding an item and not picking up an item, do this.
        if (Input.GetKeyDown("q") && holdingItem && !pickingUpItem) {
            DropItem();
        }

        // If the right mouse button is pressed and the player is not holding or picking up an item, do this.
        if (Input.GetMouseButtonDown(1) && !holdingItem && !pickingUpItem) {
            PickUpItem();
        }

        // If the variable holdingItem is true, then hold the weapon.
        if (holdingItem) {
            HoldItem();
        }

        if (Input.GetKey("f") && canFly()) {
            flyWithHammer();
        }
        bool canFly() {
            if (!holdingItem) {
                return true;
            }
            else {
                return false;
            }
        }
    }

    // This is run constantly from the frame after the player summons their weapon until they stop holding it.
    void HoldItem() {
        spearTargetPos = weaponTargetPosition.transform.position;
        spearTargetRot = weaponTargetPosition.transform.rotation;

        // If the item is being picked up, do this.
        if (pickingUpItem == true) {
            /* TODO
             * Make weapon rotation work properly. Also make it aim in the direction of travel until just before it arrives.
             * Make weapon travel faster at end of smoothdamp function. Currently even if it looks like it's in your hand, it technically isn't.
            */

            spearCurrentPos = spear.transform.position;
            Vector3 spearCurrentVel = spearRigidbody.velocity;
            float spearSmoothTime = 0.01f;
            float spearMaxVelocity = 60f;

            // If the weapon's distance to the player's hand is less than 0.12f, teleport to target position and set pickingUpItem to false.
            if (Vector3.Distance(spearCurrentPos, spearTargetPos) < 0.12f) {
                pickingUpItem = false;
                spear.transform.position = spearTargetPos;
            }

            // Use the SmoothDamp method to move the spear towards the target position smoothly.
            spear.transform.position = Vector3.SmoothDamp(spearCurrentPos, spearTargetPos, ref spearCurrentVel, spearSmoothTime, spearMaxVelocity);

            // Snappy prototyping rotation.
            spear.LookAt(spearTargetPos);
            // Fix weird rotation problem.
            transform.rotation *= Quaternion.FromToRotation(Vector3.up, Vector3.forward);
            spearOldPos = spear.transform.position;
        }

        float distanceToPlayer = Vector3.Distance(spearCurrentPos, spearTargetPos);
        if (distanceToPlayer < 12f) {
            // Increases percentOrSmthn over time.
            if (timer >= delayAmount && distanceToPlayer >= 0) {
                timer = 0f;
                percentOrSmthn += 0.01f;
            }
            // Use the Slerp method to rotate the spear towards the target rotation until percentOrSmthn reaches 100%. Needs to be redone.
            spear.transform.rotation = Quaternion.Slerp(spear.rotation, spearTargetRot, percentOrSmthn);
        }

        // If the item has gotten to the destination (player), do this.
        if (spearCurrentPos == spearTargetPos) {
            pickingUpItem = false;
            spear.rotation = spearTargetRot;
        }

        if (!pickingUpItem && holdingItem) {
            // Set spear as child of player.
            transform.parent = player.transform;
        }
    }

    // This runs once when the player picks up the item.
    void PickUpItem() {
        spearGroundPos = spear.transform.position;
        spearGroundRot = spear.transform.rotation;
        holdingItem = true;
        pickingUpItem = true;

        // Turn off gravity.
        spearRigidbody.useGravity = false;

        // Set weapon velocity and rotation to zero.
        spearRigidbody.velocity = Vector3.zero;
        spearRigidbody.angularVelocity = Vector3.zero;

        // Set weapon head and shaft colliders to triggers so they don't interact with other colliders.
        spearShaft.isTrigger = true;
        spearHammerHead.isTrigger = true;
    }
    void DropItem() {
        holdingItem = false;
        pickingUpItem = false;

        // Turn on gravity.
        spearRigidbody.useGravity = true;

        // Set weapon head and shaft colliders to no longer be triggers so they interact with other colliders.
        spearShaft.isTrigger = false;
        spearHammerHead.isTrigger = false;

        // Reset percentOrSmth to zero.
        percentOrSmthn = 0f;

        /* ADDING VELOCITY TO DROPPED WEAPON - maybe use a function here?*/
        playerVelocity = playerController.velocity;
        throwStrength = 70;

        // Add player speed to item.
        spearRigidbody.velocity = playerVelocity;

        // Add extra veocity in the direction the camera is facing.
        spearRigidbody.AddForce(throwStrength/9 * playerCamera.transform.forward, ForceMode.Impulse);
        spearRigidbody.AddForce(throwStrength/9 * playerCamera.transform.up, ForceMode.Impulse);

        // Set spear as child of the player's parent.
        transform.parent = player.transform.parent;
    }
    void flyWithHammer() {
        spearTargetPos = player.transform.position;
        spearCurrentPos = spear.transform.position;

        Vector3 spearCurrentVel = spearRigidbody.velocity;
        float spearSmoothTime = 0.01f;
        float spearMaxVelocity = 60f;
        // Use the SmoothDamp method to move the spear towards the target position smoothly.
        spear.transform.position = Vector3.SmoothDamp(spearCurrentPos, spearTargetPos, ref spearCurrentVel, spearSmoothTime, spearMaxVelocity);
        // Snappy prototyping rotation.
        spear.LookAt(airControlDesiredPos);
        // Fix weird rotation problem.
        transform.rotation *= Quaternion.FromToRotation(Vector3.up, Vector3.forward);
    }
}
