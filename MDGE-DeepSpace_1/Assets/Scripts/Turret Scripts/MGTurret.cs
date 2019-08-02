using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGTurret : Turret
{

    /// <summary>
    /// The number of times the turret attacks in one second.
    /// </summary>
    [Header("MG Turret's Properties")]
    public float firingRate;

    /// <summary>
    /// The duration left before the next turret fire.
    /// </summary>
    private float firingCountdown;

    /// <summary>
    /// The GameObject to be treated as the bullet.
    /// </summary>
    public GameObject bullet;

    /// <summary>
    /// The Animator component responsible for animating this turret.
    /// This is used to set certain animation parameters to allow the turret to
    /// animate as required.
    /// </summary>
    [SerializeField]
    private Animator turretAnimator;

    // Start is called before the first frame update
    void Start()
    {
        //start the coroutine for detecting enemies
        StartCoroutine(DetectEnemies(enemyDetectionPeriod));

        //calculate the time till next turret attack
        //period is the reciprocal of frequency
        firingCountdown = 1 / firingRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            //aim at the target
            AimAtTarget();

            //fire at the target
            FireAtTarget();

            //enable animation
            turretAnimator.SetBool("isFiring", true);
        }
        else
        {
            //reset countdown
            firingCountdown = 1 / firingRate;

            //disable animation
            turretAnimator.SetBool("isFiring", false);
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

    private void FireAtTarget()
    {

        //reduce the countdown
        firingCountdown -= Time.deltaTime;

        //when our countdown hits zero or less...
        if (firingCountdown <= 0)
        {
            //fire a bullet at a specified speed towards the target
            GameObject bulletInstance = Instantiate(bullet, transform.position, transform.rotation);

            //apply a velocity to the bullet
            bulletInstance.GetComponent<Rigidbody2D>().velocity = transform.up * 10;

            //reset countdown
            firingCountdown = 1 / firingRate;
        }
    }
}
