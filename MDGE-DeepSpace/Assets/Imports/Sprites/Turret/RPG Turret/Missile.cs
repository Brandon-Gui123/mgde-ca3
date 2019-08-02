using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Projectile
{
    //ProjectileProperties
    public float projectileDamage = 20f;
    public float projectileDamageRate = 5f;
    public float projectileLifetime = 5f;
    public float projectileSpeed = 10f;
    public Animator animator;

    //to be set by turret;
    public Transform target;

    public float explosionRadius = 5f;
    public float explosionLifetime = 0.5f;

    void Update() {
        if (target) {
            transform.rotation = Quaternion.Euler(0, 0, DetermineAngle(transform.position, target.position));
        }
        transform.position += transform.up * projectileSpeed * Time.deltaTime;
    }
    float DetermineAngle(Vector2 origin, Vector2 target) {

        float horizontal = target.x - origin.x;
        float vertical = target.y - origin.y;
        return Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg - 90;
    }
    void ExplodeMissile() {
        AnimateTheProjectile(animator, "MissileExplode");
        StartCoroutine(DamageEnemy(ScanRadius(transform.position, explosionRadius, 30f), projectileDamage));
        Destroy(gameObject, explosionLifetime);
    }
}
