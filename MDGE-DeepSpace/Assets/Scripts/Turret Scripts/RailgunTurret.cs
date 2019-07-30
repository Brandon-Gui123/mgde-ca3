using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunTurret : MonoBehaviour
{

    /// <summary>
    /// The furthest distance that the turret can detect enemies.
    /// </summary>
    [Header("Turret Properties")]
    public float range;

    /// <summary>
    /// The Transform of the target that this turret is focused on.
    /// </summary>
    private Transform target;

    /// <summary>
    /// The number of times the turret will attempt to charge itself up per second.
    /// Note that if a charge is in progress, it will ignore consecutive attempts to start
    /// charging.
    /// </summary>
    public float firingRate;

    /// <summary>
    /// The duration before the next charging up attempt.
    /// </summary>
    private float firingCountdown;

    /// <summary>
    /// The Animator component responsible for animating this turret.
    /// We need this so as to get the current state of the turret's
    /// animation.
    /// </summary>
    [SerializeField]
    private Animator turretAnimator;

    /// <summary>
    /// Checks for an enemy every specified amount of time.
    /// </summary>
    [Header("Code Properties")]
    public float enemyDetectionPeriod = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //run the coroutine that detects for enemies
        StartCoroutine(DetectEnemies(enemyDetectionPeriod));

        //reset countdown
        firingCountdown = 1 / firingRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            //aim at the target
            AimAtTarget();

            //charge up an attack
            ChargeUp();

            //fire a laser when the animator's state is in the "Fire" state
            if (IsFiringState("Railgun Firing"))
            {
                //TODO: Insert You Jing's line renderer thingy here (once I know how it works)
                Debug.Log("FIRE!");
            }
        }
    }

    /// <summary>
    /// Callback to draw gizmos only if the object is selected.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        //draw a wire sphere around the turret showing its range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    /// <summary>
    /// A coroutine that runs at a specified interval.
    /// This coroutine handles detection of enemies.
    /// The reason we put this in a coroutine is because continuously
    /// doing Physics2D.OverlapCircle can hurt performance.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DetectEnemies(float interval)
    {

        //infinte loop
        while (true)
        {

            //check for the nearest enemy
            target = GetNearestEnemy();

            //log our target
            if (target == null)
            {
                Debug.Log("Target is null!");
            }
            else
            {
                Debug.Log("Target: " + target);
            }

            //suspend execution for 0.5 seconds
            yield return new WaitForSeconds(interval);
        }
    }

    /// <summary>
    /// Gets the Transform of the enemy that is nearest to the turret.
    /// </summary>
    /// <returns>The Transform of the enemy that is nearest to the turret.</returns>
    private Transform GetNearestEnemy()
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
    private float DetermineAngle(Vector2 origin, Vector2 target)
    {

        float horizontal = target.x - origin.x;
        float vertical = target.y - origin.y;

        return Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg - 90;

    }

    /// <summary>
    /// Causes the turret to look at the target.
    /// </summary>
    private void AimAtTarget()
    {
        transform.rotation = Quaternion.Euler(0, 0, DetermineAngle(transform.position, target.position));
    }

    private void ChargeUp()
    {
        //start counting down
        firingCountdown -= Time.deltaTime;

        //when our countdown hits zero or less...
        if (firingCountdown <= 0)
        {

            //set a trigger on the animator, which charges up an attack
            turretAnimator.SetTrigger("Charge");
        }
    }

    private bool IsFiringState(string stateName)
    {
        return turretAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
}
