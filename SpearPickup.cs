using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearPickup : MonoBehaviour
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

    Vector3 itemDropVelocity = new Vector3();
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

    // Collider-related variables
    [SerializeField] Collider spearShaft;
    [SerializeField] Collider spearHammerHead;

    // Start is called before the first frame update
    void Start() {

    }

    void Update() {
        // if e is pressed and the payer is holding an item and not picking up an item, do this.
        if (Input.GetKeyDown("e") && holdingItem && !pickingUpItem) {
            holdingItem = false;
            pickingUpItem = false;
            DropItem();
        }

        RaycastToWeapon();
        if (holdingItem) {
            HoldItem();
        }
    }

    void HoldItem() {
        spearTargetPos = weaponTargetPosition.transform.position;
        spearTargetRot = weaponTargetPosition.transform.rotation;

        // If the item is not at the lerping destination, do this.
        if (percentOrSmthn <= 1) {
            timer += Time.deltaTime;
            spear.transform.position = Vector3.Slerp(spearGroundPos, spearTargetPos, percentOrSmthn);
            spear.transform.rotation = Quaternion.Slerp(spearGroundRot, spearTargetRot, percentOrSmthn);
        }

        // If the item has gotten to the lerping destination, do this.
        if (percentOrSmthn >= 1) {
            pickingUpItem = false;
        }

        if (!pickingUpItem && holdingItem) {
            // Set spear as child of player.
            transform.parent = player.transform;
        }

        if (timer >= delayAmount && pickingUpItem) {
            timer = 0f;
            percentOrSmthn += 0.01f;
        }
    }

    // This runs once when the player presses e to pick up the item.
    void PickUpItem() {
        /*itemPickupStartTime = Time.timeAsDouble;
        itemArrivesAfter = itemPickupStartTime + 0.5;*/

        // Turn off gravity.
        spearRigidbody.useGravity = false;

        // Set weapon head and shaft colliders to triggers so they don't interact with other colliders.
        spearShaft.isTrigger = true;
        spearHammerHead.isTrigger = true;

        // Set weapon velocity and rotation to zero.
        spearRigidbody.velocity = Vector3.zero;
        spearRigidbody.angularVelocity = Vector3.zero;
    }
    
    void DropItem() {
        // Turn on gravity.
        spearRigidbody.useGravity = true;

        // Set weapon head and shaft colliders to no longer be triggers so they interact with other colliders.
        spearShaft.isTrigger = false;
        spearHammerHead.isTrigger = false;

        // Add velocity to dropped item.
        Vector3 addedVelocity = new Vector3(0, 4, 6);
        playerVelocity = playerController.velocity;
        itemDropVelocity = playerVelocity;
        itemDropVelocity += addedVelocity;
        float throwStrength = 20;
        // Add player speed to item.
        spearRigidbody.velocity = playerVelocity;
        // Add extra veocity in the direction the camera is facing.
        spearRigidbody.AddForce(throwStrength * playerCamera.transform.forward, ForceMode.Impulse);
        spearRigidbody.AddForce(throwStrength * playerCamera.transform.up, ForceMode.Impulse);

        // Set spear as child of the player's parent.
        transform.parent = player.transform.parent;

        // Recent percentOrSmth to 0.
        percentOrSmthn = 0f;
    }

    bool RaycastToWeapon() {
        RaycastHit hit;

        /*---------------------------------------------------
        After a day of troubleshooting and a night of sleeping, I figured out that my code was translating cameraForward into global space
        when it was given in global space in the first place.
        ---------------------------------------------------*/

        Vector3 cameraForward = playerCamera.forward;

        // If the cast ray hits an object tagged "Spear," do this.
        if (Physics.Raycast(playerCamera.transform.position, cameraForward, out hit, 4.5f) && hit.transform.tag == "Spear") {
            // If the raycast hits something, draw a red line from the player to where the raycast hit. For this to work,
            // the RaycastToWeapon() function needs to be called in Update()
            Debug.DrawRay(playerCamera.transform.position, cameraForward * hit.distance, Color.red);

            // If e is pressed and the player is not holding or picking up an item, do this.
            if (Input.GetKeyDown("e") && !holdingItem && !pickingUpItem) {
                spearGroundPos = spear.transform.position;
                spearGroundRot = spear.transform.rotation;
                holdingItem = true;
                pickingUpItem = true;
                PickUpItem();
            }

            return true;
        }
        else {
            // If the raycast doesn't hit something, draw a white line from the player to the maximum raycast distance.
            // For this to work, the RaycastToWeapon() function needs to be called in Update()
            Debug.DrawRay(playerCamera.transform.position, cameraForward * 4.5f, Color.white);

            return false;
        }
    }
}
