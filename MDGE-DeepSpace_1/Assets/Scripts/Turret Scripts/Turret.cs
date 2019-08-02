using System.Collections;
using UnityEngine;

public abstract class Turret : MonoBehaviour
{

    /// <summary>
    /// The furthest distance that the turret can detect enemies.
    /// </summary>
    [Header("Common Turret Properties"), Tooltip("The furthest distance that the turret can detect enemies.")]
    public float range;

    /// <summary>
    /// The Transform of the target that this turret is focused on.
    /// </summary>
    protected Transform target;

    /// <summary>
    /// Checks for nearby enemies every specified seconds.
    /// Used in the coroutine for detecting enemies.
    /// Lower values allow for faster enemy detection, but may hurt performance.
    /// </summary>
    [Tooltip("The duration between enemy detections. Lower values mean faster frequencies, but may hurt performance.")]
    public float enemyDetectionPeriod = 0.5f;

    /// <summary>
    /// A coroutine that runs at a specified interval.
    /// This coroutine handles detection of enemies.
    /// The reason we put this in a coroutine is because continuously
    /// doing Physics2D.OverlapCircle can hurt performance.
    /// </summary>
    /// <returns></returns>
    protected IEnumerator DetectEnemies(float interval)
    {

        //infinte loop
        while (true)
        {

            //check for the nearest enemy
            target = GetNearestEnemy();

            //suspend execution for 0.5 seconds
            yield return new WaitForSeconds(interval);
        }
    }

    /// <summary>
    /// Gets the Transform of the enemy that is nearest to the turret.
    /// </summary>
    /// <returns>The Transform of the enemy that is nearest to the turret.</returns>
    protected Transform GetNearestEnemy()
    {
        //get all enemy colliders in the overlapping circle
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(transform.position, range, LayerMask.GetMask("Enemy"));

        if (enemyColliders.Length <= 0)
        {
            return null;
        }

        //assume that the first collider in the array is closest to the turret
        Collider2D nearestCollider = enemyColliders[0];

        //calculate the distance between the first collider array and the turret
        float closestDistanceToTurret = Vector3.Distance(transform.position, nearestCollider.transform.position);

        //pick the enemy that is closest to the turret
        foreach (Collider2D enemyCollider in enemyColliders)
        {

            //check the distance between the turret and the enemy collider
            float distanceToTurret = Vector3.Distance(enemyCollider.transform.position, transform.position);

            //is the distance smaller than our calculated distance?
            if (distanceToTurret < closestDistanceToTurret)
            {
                //this collider is the nearest to the turret
                nearestCollider = enemyCollider;

                //set the closest distance variable to this distance
                closestDistanceToTurret = distanceToTurret;
            }
        }

        //return the closest collider as a GameObject
        return nearestCollider.transform;
    }

    /// <summary>
    /// Determines the angle that the turret has to rotate in order to look at a given point.
    /// </summary>
    /// <param name="origin">The position of the turret.</param>
    /// <param name="target">The position of the target.</param>
    /// <returns></returns>
    protected float DetermineAngle(Vector2 origin, Vector2 target)
    {

        float horizontal = target.x - origin.x;
        float vertical = target.y - origin.y;

        return Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg - 90;

    }

    /// <summary>
    /// Causes the turret to look at the target.
    /// </summary>
    protected void AimAtTarget()
    {
        transform.rotation = Quaternion.Euler(0, 0, DetermineAngle(transform.position, target.position));
    }
}