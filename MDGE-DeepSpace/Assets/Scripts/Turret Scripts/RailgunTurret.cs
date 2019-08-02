using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunTurret : Turret
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
    /// The duration the turret will wait before it begins charging up a shot.
    /// Specifically, this is the duration between states 
    /// <see cref="TurretState.Idle"/> and <see cref="TurretState.ChargingUp"/>.
    /// </summary>
    [Header("Railgun Turret's Properties"), Tooltip("The duration the turret will wait before it begins charging up. This is essentially the duration between idle and charging up.")]
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
    /// The script component that handles the railgun beam.
    /// </summary>
    [Header("Beam"), SerializeField]
    private RailgunBeam beam;

    /// <summary>
    /// The damage that this turret's beam will apply to enemies.
    /// </summary>
    public float beamDamage;

    /// <summary>
    /// The length of the railgun beam.
    /// Enemies caught in this beam will be damaged.
    /// </summary>
    public float beamRange;

    /// <summary>
    /// How often will enemies in the beam be damaged.
    /// </summary>
    public float beamDamageInterval;

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

        Debug.Log(currentTurretState.ToString());

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
                    //our railgun beam will be able to damage enemies
                    beam.canDamageEnemies = true;
                }
                else
                {
                    //stop firing and go back to idle state
                    currentTurretState = TurretState.Idle;

                    //reset the firing countdown
                    firingCountdown = firingDuration;

                    //set a cooldown time before it can charge up and fire again
                    waitTimeCountdown = waitTimeBeforeCharging;

                    //our railgun beam will not be able to damage enemies
                    beam.canDamageEnemies = false;

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

        Debug.Log("Hello world!");
    }

    /// <summary>
    /// Charges the railgun for its next attack.
    /// </summary>
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
