using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunTurret : MonoBehaviour
{

    /// <summary>
    /// An enumeration containing the possible states of this turret.
    /// </summary>
    private enum TurretState
    {
        /// <summary>
        /// The turret is not doing anything.
        /// </summary>
        Idle,

        /// <summary>
        /// The turret is currently charging up a shot.
        /// </summary>
        ChargingUp,

        /// <summary>
        /// The turret is currently firing a shot.
        /// </summary>
        Firing
    }

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
    /// The duration the turret will take before it begins charging up.
    /// </summary>
    [Tooltip("The duration the turret will wait before it begins charging up.")]
    public float waitTimeBeforeCharging;

    /// <summary>
    /// A countdown timer that will charge the turret up when it ends.
    /// </summary>
    private float waitTimeCountdown;

    /// <summary>
    /// How long the turret will take, in seconds, to charge up a shot.
    /// </summary>
    [Tooltip("How long the turret will take when it charges up a shot.")]
    public float chargingDuration;

    /// <summary>
    /// A countdown that lets the turret finish charging at the end.
    /// </summary>
    private float chargingCountdown;

    /// <summary>
    /// How long the turret will fire its shot before stopping.
    /// </summary>
    [Tooltip("How long the turret will fire its shot before stopping.")]
    public float firingDuration;

    /// <summary>
    /// A countdown where, at the end, the turret will fire.
    /// </summary>
    private float firingCountdown;

    /// <summary>
    /// The current state of this turret.
    /// </summary>
    private TurretState currentTurretState = TurretState.Idle;

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

    /// <summary>
    /// Whether the coroutine <see cref="DetectEnemies(float)"/> is running.
    /// </summary>
    private bool enemyDetectionRunning = false;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (enemyDetectionPeriod <= 0.1f)
        {
            //the enemy detection period is too low and hence, the enemy detection coroutine will run frequently
            //this code here helps adjust the enemy detection period back to the default value if the current
            //value is too low.
            //this also may help prevent the Unity Editor from freezing up, though it is unsure if that will ever occur (I, Brandon, have not tried it).
            Debug.LogError("Enemy detection period is too low! It's been set back to the default value (0.5f).");
            enemyDetectionPeriod = 0.5f;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //run the coroutine that detects for enemies
        StartCoroutine(DetectEnemies(enemyDetectionPeriod));

        //reset countdowns
        waitTimeCountdown = 0; //there's no waiting at the start
        chargingCountdown = chargingDuration;
        firingCountdown = firingDuration;

        //assign duration values to the animator parameters (adjusts the speed of the animation)
        //everything here is the reciprocal of the variable because we are adjusting the speed multiplier, instead of the duration
        turretAnimator.SetFloat("chargingDuration", 1 / chargingDuration);
        turretAnimator.SetFloat("firingDuration", 1 / firingDuration);
    }

    // Update is called once per frame
    void Update()
    {

        if (target)
        {
            //aim at the target
            AimAtTarget();
        }

        Debug.Log(currentTurretState);

        switch (currentTurretState)
        {
            //the turret is neither charging up, nor firing, it is just there doing nothing, looking at the enemy (if there's one)
            case TurretState.Idle:

                //start the coroutine, if it isn't running
                if (!enemyDetectionRunning)
                {
                    StartCoroutine(DetectEnemies(enemyDetectionPeriod));
                }

                //move on to next state if there is a target and we have waited long enough
                if (target && waitTimeCountdown <= 0)
                {
                    //begin charging up!
                    currentTurretState = TurretState.ChargingUp;

                    //set the trigger of the animator
                    turretAnimator.SetTrigger("Charge");
                }
                else if (waitTimeCountdown > 0)
                {
                    //countdown
                    waitTimeCountdown -= Time.deltaTime;
                }

                break;

            //the turret is currently charging up a shot
            case TurretState.ChargingUp:

                //stop the enemy detection coroutine from running so that we can lock on our target
                if (enemyDetectionRunning)
                {
                    StopCoroutine(DetectEnemies(enemyDetectionPeriod));
                    enemyDetectionRunning = false;
                }

                //start the charging countdown
                chargingCountdown -= Time.deltaTime;

                if (chargingCountdown <= 0)
                {
                    //reset the charging countdown
                    chargingCountdown = chargingDuration;

                    //move on to the firing state
                    currentTurretState = TurretState.Firing;
                }

                break;

            case TurretState.Firing:

                //start the firing countdown
                firingCountdown -= Time.deltaTime;

                if (firingCountdown > 0)
                {
                    //this is where we fire the enemy I think
                    //TODO: Insert You Jing's line renderer thingy here
                    Debug.Log("Firing at enemy");
                }
                else
                {
                    //stop firing and go back to idle state
                    currentTurretState = TurretState.Idle;

                    //reset the firing countdown
                    firingCountdown = firingDuration;

                    //set a cooldown time before it can charge up and fire again
                    waitTimeCountdown = waitTimeBeforeCharging;
                }

                break;
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

        //this coroutine is running
        enemyDetectionRunning = true;

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
        waitTimeCountdown -= Time.deltaTime;

        //when our countdown hits zero or less...
        if (waitTimeCountdown <= 0)
        {

            //set a trigger on the animator, which charges up an attack
            turretAnimator.SetTrigger("Charge");
        }
    }
}
