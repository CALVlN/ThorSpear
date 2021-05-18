using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearController : MonoBehaviour
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
            Debug.Log("ATTAAAAAACK!");
        }
        if (primaryAttacking == true) {
            Debug.Log("PRIMARY ATTACKING");
            primaryAttackingTime += Time.deltaTime;

            if (primaryAttackingTime >= 1.5f) {
                primaryAttacking = false;
                primaryAttackingTime = 0f;
            }
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy") && primaryAttacking) {
            Debug.Log(other);
            Destroy(other.gameObject);
        }
    }
    void PrimaryAttack() {
        
        // Do 10 damage to the first enemy hit by the hammer.
    }
}
