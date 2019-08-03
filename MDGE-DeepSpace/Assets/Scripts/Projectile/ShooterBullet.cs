using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBullet : Projectile
{

    /// <summary>
    /// The amount of damage this bullet will deal to the player.
    /// </summary>
    [HideInInspector]
    public float damage;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Destroy(gameObject, 8);
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        //deal damage to the player when it collides with it
        if (other.CompareTag("Player"))
        {
            Debug.Log("Dealed " + damage + " to player.");
            Destroy(gameObject);
        }
    }

}
