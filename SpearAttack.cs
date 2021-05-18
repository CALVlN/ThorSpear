using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearAttack : MonoBehaviour
{
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
        if (Input.GetMouseButtonDown(0)/* && holdingItem from Weapons.css is true */) {
            primaryAttacking = true;

            PrimaryAttack();
            // Debug.Log("ATTAAAAAACK!");
        }
        if (primaryAttacking == true) {
            // Debug.Log("PRIMARY ATTACKING");
            primaryAttackingTime += Time.deltaTime;

            if (primaryAttackingTime >= 1.5f) {
                primaryAttacking = false;
                primaryAttackingTime = 0f;
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
        
    }
}
