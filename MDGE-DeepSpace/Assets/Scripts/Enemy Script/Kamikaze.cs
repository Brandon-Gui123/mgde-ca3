using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamikaze : EnemyController
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
        /// The enemy has started its countdown timer and is about to explode
        /// if the player doesn't move away (or if some other condition is not met).
        /// </summary>
        AboutToExplode
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

        //all kamikazes have their fuse timings at their initial values
        fuseCountdown = fuseTime;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentAIState)
        {

            //the AI isn't doing anything huge
            case AIState.Idle:
                //because we are always pursuing our target, simply move on to the next state
                if (player)
                {
                    currentAIState = AIState.Purusing;
                }
                break;

            //the AI is currently pursuing the player
            case AIState.Purusing:
                //aim at the target and move towards it
                AimAtTarget();
                transform.Translate(Vector3.up * speed * Time.deltaTime); //this is relative to the object

                //calculate distance between player and enemy
                if (Vector2.Distance(transform.position, player.position) <= stoppingDistance)
                {
                    //move on to next state
                    currentAIState = AIState.AboutToExplode;
                }

                break;

            //the AI is about to explode
            case AIState.AboutToExplode:

                //did the player move out of our stopping distance?
                if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
                {
                    //go back to previous state to move closer to the player
                    currentAIState = AIState.Purusing;

                    //reset countdown
                    fuseCountdown = fuseTime;
                }

                //countdown the explosion
                fuseCountdown -= Time.deltaTime;

                if (fuseCountdown <= 0)
                {
                    //we now explode
                    Debug.Log("Explode!");
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
        //code here for when this guy dies
    }
}
