using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotController : MonoBehaviour
{
    GameObject player;
    Rigidbody robotRB;
    [SerializeField] LayerMask EnemyLayer;
    float timeSinceInstantiated = 0f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        timeSinceInstantiated = Time.timeSinceLevelLoad;
    }

    // Update is called once per frame
    void Update()
    {
        robotRB = gameObject.GetComponent<Rigidbody>();
        if (transform.position.y < -150f) {
            Destroy(gameObject);
        }
        // If the enemy is not dead and two seconds has past since it was instantiated
        if (gameObject.tag == "Enemy" && Time.timeSinceLevelLoad - timeSinceInstantiated > 2f && Vector3.Distance(transform.position, player.transform.position) < 10f && robotRB.velocity.magnitude < 1.5f) {
            AttackPlayer();
        }
    }
    void AttackPlayer() {
        Vector3 enemyPos = transform.position;
        enemyPos.y += 0.3f;

        // if nothing is between the player centre and the enemy
        if (!Physics.Linecast(transform.position, player.transform.position, EnemyLayer)) {
            Vector3 moveDirection = (player.transform.position - transform.position).normalized;
            
            robotRB.AddForce(moveDirection * 5f);
        }
    }
}
