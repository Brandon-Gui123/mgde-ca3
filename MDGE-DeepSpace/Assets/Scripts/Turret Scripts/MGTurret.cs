using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGTurret : Turret
{

    public AudioSource MGTurretSFX;

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
    /// The amount of damage that each round of this bullet can deal to an enemy.
    /// </summary>
    public float damage = 8;

    /// <summary>
    /// The GameObject to be treated as the bullet.
    /// </summary>
    public MGBullet bullet;

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

        MGTurretSFX.volume = PlayerPrefs.GetFloat("SFXVolume", 0.2f);

        //start the coroutine for detecting enemies
        StartCoroutine(DetectEnemies(enemyDetectionPeriod));

        //calculate the time till next turret attack
        //period is the reciprocal of frequency
        firingCountdown = 1 / firingRate;

    }

    // Update is called once per frame
    void Update()
    {

        MGTurretSFX.volume = PlayerPrefs.GetFloat("SFXVolume", 0.2f);
        if (target && !target.IsDead)
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
            
            MGBullet bulletInstance = Instantiate(bullet, transform.position, transform.rotation);
            MGTurretSFX.Play();
            //apply a velocity to the bullet
            bulletInstance.gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * 10;

            //apply damage
            bulletInstance.damage = damage;

            //reset countdown
            firingCountdown = 1 / firingRate;
        }
    }
}
