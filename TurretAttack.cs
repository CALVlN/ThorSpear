using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAttack : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform turretHead;
    [SerializeField] Rigidbody bulletRB;
    [SerializeField] GameObject spear;

    Vector3 targetPos = new Vector3();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Fire");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (spear.GetComponent<SpearAttack>().turretAlive == true) {
            Aim();
        }
        else if (spear.GetComponent<SpearAttack>().turretAlive == false) {
            StopCoroutine("Fire");
        }
    }
    void Aim() {
        targetPos = player.transform.position;
        // Make turret point at player.
        turretHead.LookAt(targetPos);
        // Fix weird rotation problem.
        transform.rotation *= Quaternion.FromToRotation(Vector3.left, Vector3.back);
    }
    IEnumerator Fire() {
        float bulletSpeed = 25f;
        for (; ; ) {
            Rigidbody bulletClone;
            bulletClone = Instantiate(bulletRB, transform.position, transform.rotation);
            bulletClone.velocity = transform.TransformDirection(Vector3.right * bulletSpeed);
            yield return new WaitForSeconds(1f);
        }
    }
}
