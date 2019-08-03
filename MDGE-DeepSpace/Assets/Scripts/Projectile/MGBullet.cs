using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGBullet : Projectile
{

    /// <summary>
    /// The amount of damage to deal to the enemy that was hit.
    /// </summary>
    [HideInInspector]
    public float damage;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        //destroy itself after 5 seconds
        Destroy(gameObject, 5);
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        //deal damage to the enemy we collided with
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //damage the enemy
            other.GetComponent<EnemyController>().Damage(damage);

            //destroy the bullet
            Destroy(gameObject);
        }
    }

}
