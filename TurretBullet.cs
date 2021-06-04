using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour {
    Transform turret;
    //[SerializeField] GameObject bulletMetalImpact;
    GameObject player;

    // Start is called before the first frame update
    void Start() {
        turret = GameObject.Find("Turret").transform;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update() {
        // distanceIsShort = (Vector3.Distance(transform.position, turret.position) < 1f) ? distanceIsShort : !distanceIsShort;

        // Destroy bullet if it gets 40 units away from the turret.
        if (Vector3.Distance(transform.position, turret.position) > 40f)
            Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other) {
        if (other.name != "Turret" && other.name !="Gun" && other.name != "Pylon" && other.name != "PylonElement" && other.name != "Player") {
            // Handling effect rotation.
            /*Vector3 newRot = new Vector3(transform.rotation.x - 180, transform.rotation.y - 180, transform.rotation.z -180);
            Quaternion oppositeRotation = Quaternion.Euler(newRot);
            // Instantiate effect.
            GameObject hitEffect = Instantiate(bulletMetalImpact, transform.position, oppositeRotation);
            hitEffect.GetComponent<ParticleSystem>().Play();
            // Scale effect.
            hitEffect.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);*/
            // Destroy bullet.

            Destroy(gameObject);
        }
        else if (other.name == "Player") {
            player.GetComponent<PlayerStats>().HitByTurret();
            Destroy(gameObject);
        }
    }
}
