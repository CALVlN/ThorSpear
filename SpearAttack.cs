using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearAttack : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform playerCamera;
    //[SerializeField] Transform weaponTargetPosition;
    [SerializeField] Transform spear;
    [SerializeField] Rigidbody spearRigidbody;
    // Collider variables
    [SerializeField] Collider spearShaft;
    [SerializeField] Collider spearHammerHead;

    float weaponDamage = 10f;
    bool primaryAttacking = false;
    float primaryAttackingTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(CanAttack());
        if (Input.GetMouseButtonDown(0) && CanAttack()) {
            primaryAttacking = true;

            PrimaryAttack();
        }
        if (primaryAttacking == true) {
            Debug.Log("PRIMARY ATTACKING");
            primaryAttackingTime += Time.deltaTime;

            if (primaryAttackingTime >= 1.5f && !gameObject.GetComponent<SummonSpear>().pickingUpItem && !gameObject.GetComponent<SummonSpear>().holdingItem) {
                primaryAttacking = false;
                primaryAttackingTime = 0f;

                // Turn on gravity.
                spearRigidbody.useGravity = true;
            }
            else if (gameObject.GetComponent<SummonSpear>().pickingUpItem) {
                primaryAttacking = false;
                primaryAttackingTime = 0f;
                Debug.Log("ran");
            }
        }
    }

    // Return true if below conditions are met, in which case attacking should be allowed.
    bool CanAttack() {
        bool holdingItem = gameObject.GetComponent<SummonSpear>().holdingItem;
        bool pickingUpItem = gameObject.GetComponent<SummonSpear>().pickingUpItem;

        // If holdingItem and !pickingUpItem from SpearPickup.cs, and !attacking from this script, return true. Else, return false.
        if (holdingItem && !pickingUpItem && !primaryAttacking) {
            return true;
        }
        else {
            return false;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy") && primaryAttacking) {
            Debug.Log(other);
            Destroy(other.gameObject);
        }
    }

    // Attack and do 10 damage to all enemies hit by the hammer.
    void PrimaryAttack() {
        float throwStrength = gameObject.GetComponent<SummonSpear>().throwStrength;
        gameObject.GetComponent<SummonSpear>().holdingItem = false;
        gameObject.GetComponent<SummonSpear>().pickingUpItem = false;

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
        spearRigidbody.AddForce(throwStrength * playerCamera.transform.forward, ForceMode.Impulse);
        spearRigidbody.AddForce(throwStrength/30 * playerCamera.transform.up, ForceMode.Impulse);

        // Set spear as child of the player's parent.
        transform.parent = player.transform.parent;
    }
}
