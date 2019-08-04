using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamikaze : EnemyController
{

    public AudioSource DeadKamikazeSFX;

    /// <summary>
    /// An enumeration containing the possible AI states that this AI can go into.
    /// </summary>
    private enum AIState
    {
        /// <summary>
        /// The enemy isn't doing anything important.
        /// </summary>
        Idle,

        /// <summary>
        /// The enemy is following a player.
        /// </summary>
        Pursuing,

        /// <summary>
        /// The enemy has started its countdown timer and is charging itself up.
        /// The enemy will then explode after.
        /// </summary>
        ChargingUp,

        /// <summary>
        /// The enemy is dead.
        /// This is the state where the enemy explodes.
        /// </summary>
        Dead
    }

    /// <summary>
    /// The current state of this AI.
    /// The current state is what determines the behaviour of this AI.
    /// </summary>
    private AIState currentAIState;

    /// <summary>
    /// The distance that the enemy will stop at before it blows up.
    /// </summary>
    [Header("Kamikaze Properties")]
    public float stoppingDistance;

    /// <summary>
    /// The duration before the enemy explodes.
    /// The countdown is reset to this value if the player
    /// travels far away enough from the enemy.
    /// </summary>
    public float fuseTime = 2.0f;

    /// <summary>
    /// The amount of time left in this instance before
    /// the enemy explodes.
    /// </summary>
    private float fuseCountdown;

    /// <summary>
    /// The Animator component responsible for animating the enemy.
    /// </summary>
    [SerializeField]
    private Animator kamikazeAnimator;

    /// <summary>
    /// How far will the explosion affect the player?
    /// If the player is outside of this distance, the explosion damage would be minimal.
    /// </summary>
    public float explosionRadius;

    /// <summary>
    /// The maximum amount of damage that the Kamikaze can deal to the player.
    /// This is the damage that would be dealt if the player is at the same position as the kamikaze.
    /// </summary>
    public float maxDamage;

    // Start is called before the first frame update
    void Start()
    {
        
        DeadKamikazeSFX.volume = PlayerPrefs.GetFloat("SFXVolume", 0.2f);
        if (!player)
        {
            //obtain the Transform of the player
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        if (!playerHealthScript)
        {

            //get the script component responsible for handling player health
            //caching the result in a variable
            playerHealthScript = player.GetComponent<Player_Health_Script>();

        }

        //all kamikazes have their fuse timings at their initial values
        fuseCountdown = fuseTime;

        //adjust the fuse time of the animator
        kamikazeAnimator.SetFloat("fuseTime", 1 / fuseTime);
    }

    // Update is called once per frame
    void Update()
    {

        DeadKamikazeSFX.volume = PlayerPrefs.GetFloat("SFXVolume", 0.2f);
        switch (currentAIState)
        {

            //the AI isn't doing anything huge
            case AIState.Idle:

                //because we are always pursuing our target, simply move on to the next state
                if (player)
                {
                    
                    currentAIState = AIState.Pursuing;
                }
                break;

            //the AI is currently pursuing the player
            case AIState.Pursuing:

                //aim at the target and move towards it
                AimAtTarget();
                transform.Translate(Vector3.up * speed * Time.deltaTime); //this is relative to the object

                //calculate distance between player and enemy
                if (Vector2.Distance(transform.position, player.position) <= stoppingDistance)
                {
                    //move on to next state
                    currentAIState = AIState.ChargingUp;

                    //start the animation for charging up
                    kamikazeAnimator.SetTrigger("Die");
                }

                break;

            //the AI is about to explode
            case AIState.ChargingUp:

                //countdown the explosion
                fuseCountdown -= Time.deltaTime;

                if (fuseCountdown <= 0)
                {
                    //move on to the dying state
                    currentAIState = AIState.Dead;

                    //deal damage to the player based on how close he is
                    float distFromPlayer = Vector3.Distance(transform.position, player.position);

                    if (distFromPlayer <= explosionRadius)
                    {
                        float damageDealt = Mathf.Lerp(maxDamage, 0, distFromPlayer / explosionRadius);
                    }

                    //play explosion sound
                    DeadKamikazeSFX.Play();
                }

                break;

            //the AI is exploding
            case AIState.Dead:
               
                AnimatorStateInfo currentAnimatorStateInfo = kamikazeAnimator.GetCurrentAnimatorStateInfo(0);

                if (currentAnimatorStateInfo.normalizedTime >= 1 && currentAnimatorStateInfo.IsName("Alien 2 Explosion"))
                {
                    Destroy(gameObject);
                }

                break;

        }
    }

    /// <summary>
    /// Callback function called when the Kamikaze dies.
    /// </summary>
    protected override void OnDie()
    {
        //transit the state where it charges up and explodes
        currentAIState = AIState.ChargingUp;
        
        //trigger the animation for charging up
        //(after which it will transit to the dying animation)
        kamikazeAnimator.SetTrigger("Die");

    }
}
