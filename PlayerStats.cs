using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float health = 100f;
    public bool hitByTurret = false;
    public HealthController healthBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hitByTurret) {
            health -= 10;
            hitByTurret = false;
            healthBar.SetHealth(health);
        }
        if (health <= 0) {
            //Debug.Log("Game Over (at least it should be)");
        }
    }
}
