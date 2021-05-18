using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotStats : MonoBehaviour
{
    public float enemyHealth = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth <= 0) {
            Destroy(gameObject);
            // Possibly add stuff to make the robot explode into pieces and then disappear.
        }
    }
}
