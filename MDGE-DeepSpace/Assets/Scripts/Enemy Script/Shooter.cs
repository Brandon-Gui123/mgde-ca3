using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : EnemyController
{
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
        Purusing,

        /// <summary>
        /// The enemy is currently firing at the player.
        /// </summary>
        Firing,

        /// <summary>
        /// The enemy is dead.
        /// </summary>
        Dead
    }

    /// <summary>
    /// The current state of this AI.
    /// The current state is what determines the behaviour of this AI.
    /// </summary>
    private AIState currentAIState;

    /// <summary>
    /// The distance at which the enemy would stop moving and
    /// start firing at the player.
    /// </summary>
    [Header("Shooter Properties")]
    public float firingDistance;

    /// <summary>
    /// The number of times the enemy will fire per second.
    /// </summary>
    public float firingRate;

    /// <summary>
    /// The amount of time left before the enemy fires.
    /// </summary>
    private float firingCountdown;

    /// <summary>
    /// The Animator component responsible for handling animations in this enemy.
    /// </summary>
    [SerializeField]
    private Animator shooterAnimator;

    public GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
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

        //reset the countdown for the firing
        //we want to get the time taken to fire one bullet (period)
        //so we take the reciprocal of the firing rate
        //(period = 1 / frequency)
        firingCountdown = 1 / firingRate;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentAIState)
        {
            //the enemy isn't doing anything big
            case AIState.Idle:
                if (player)
                {
                    //go on to pursuing state to chase the player
                    currentAIState = AIState.Purusing;
                }
                break;

            //the enemy is pursuing the player
            case AIState.Purusing:

                //look at the player
                AimAtTarget();

                //move towards the player
                transform.Translate(Vector3.up * speed * Time.deltaTime); //this is relative to the object

                if (Vector2.Distance(transform.position, player.position) <= firingDistance)
                {
                    //move on to the next state to fire at the player
                    currentAIState = AIState.Firing;
                }

                break;

            //the enemy is firing at the player
            case AIState.Firing:

                //look at the player
                AimAtTarget();

                if (Vector2.Distance(transform.position, player.position) > firingDistance)
                {
                    //player is out of range; pursue
                    currentAIState = AIState.Purusing;

                    //reset countdown
                    firingCountdown = 1 / firingRate;
                }

                //countdown the fire
                firingCountdown -= Time.deltaTime;

                //fire at the player when the countdown expires
                if (firingCountdown <= 0)
                {

                    //trigger the animation to show that the enemy is firing
                    shooterAnimator.SetTrigger("Fire");

                    //tester bullet
                    GameObject bulletInstance = Instantiate(bullet, transform.position, transform.rotation);
                    bulletInstance.GetComponent<Rigidbody2D>().velocity = transform.up * 4;

                    //reset countdown
                    firingCountdown = 1 / firingRate;
                }

                break;

            //the enemy is dead
            case AIState.Dead:

                //obtain information about the animator's current state
                AnimatorStateInfo currentAnimatorStateInfo = shooterAnimator.GetCurrentAnimatorStateInfo(0);

                //destroy the enemy object after its animation is complete
                if (currentAnimatorStateInfo.normalizedTime >= 1 && currentAnimatorStateInfo.IsName("Alien Dies"))
                {
                    Destroy(gameObject);
                }

                break;
        }
    }

    /// <summary>
    /// Callback function called when the Shooter dies.
    /// </summary>
    protected override void OnDie()
    {
        //transition to explosion animation
        shooterAnimator.SetTrigger("Explosion");

        //move to state: being dead
        currentAIState = AIState.Dead;
    }
}
