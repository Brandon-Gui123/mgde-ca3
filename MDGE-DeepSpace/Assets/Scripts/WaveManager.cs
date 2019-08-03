using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaveManager : MonoBehaviour
{

    /// <summary>
    /// A static reference to a WaveManager script component in the current scene.
    /// This is used for all enemies to easily refer to this and perform actions
    /// (right now it's just decrementing the number of enemies in the current wave)
    /// which would otherwise be performance-hurting when done in some other way.
    /// </summary>
    public static WaveManager waveManager;

    /// <summary>
    /// The current wave number.
    /// </summary>
    /// <value>The current wave number.</value>
    public static int CurrentWaveNumber { get; private set; } = 0;

    /// <summary>
    /// A first-in first-out collection of wave objects that depict what kinds of enemies will appear
    /// on the wave and how many of them will appear.
    /// We use a Queue because it is easy to obtain the next wave by simply dequeuing it.
    /// </summary>
    /// <typeparam name="EnemyWave">
    /// A custom data type containing information about the current wave itself, such
    /// as the various kinds of enemies spawning and how many of that enemy is spawning.
    /// </typeparam>
    /// <returns></returns>
    public Queue<EnemyWave> enemyWaves = new Queue<EnemyWave>();

    /// <summary>
    /// The current wave that the player is handling right now.
    /// </summary>
    private EnemyWave currentEnemyWave;

    /// <summary>
    /// A reference to the enemy spawner we need so that we can pass in information on what to spawn.
    /// </summary>
    [SerializeField]
    private EnemySpawner enemySpawner;

    /// <summary>
    /// The number of enemies in the current wave.
    /// This value is decremented as enemies are being destroyed.
    /// </summary>
    public int numEnemiesInCurrentWave = 0;

    /// <summary>
    /// The delay, in seconds, in between waves.
    /// </summary>
    public float delayBetweenWaves = 5;

    /// <summary>
    /// The time left before the next wave starts right
    /// after the player completed the previous wave and is
    /// waiting for the delay.
    /// </summary>
    private float delayBetweenWavesCountdown;

    /// <summary>
    /// A boolean indicating if we have incremented the current wave number.
    /// Helps to prevent stuff from being called more than once.
    /// </summary>
    private bool currentWaveNumberIncremented;

    /// <summary>
    /// The animator responsible for animating the wave alerter.
    /// Used to trigger animations.
    /// </summary>
    [SerializeField]
    private Animator waveAlerterAnimator;

    // Start is called before the first frame update
    void Start()
    {
        //we now assume we only have one of this script component in our scene
        //the static reference to the wave manager would be this current instance of the wave manager   
        waveManager = this;

        //grab the EnemyWave component from all children and add them to our queue
        foreach (EnemyWave enemyWave in GetComponentsInChildren<EnemyWave>(false))
        {
            enemyWaves.Enqueue(enemyWave);
        }
        
        delayBetweenWavesCountdown = delayBetweenWaves;
    }

    // Update is called once per frame
    void Update()
    {
        //the level is complete when we have no more waves 
        //and there are no more enemies in the current wave
        if (enemyWaves.Count <= 0 && numEnemiesInCurrentWave <= 0)
        {
            Debug.Log("Level complete!");
        }
        else
        {

            if (numEnemiesInCurrentWave <= 0)
            {

                if (!currentWaveNumberIncremented)
                {
                    CurrentWaveNumber++;
                    currentWaveNumberIncremented = true;

                    waveAlerterAnimator.SetTrigger("Alert");
                }

                delayBetweenWavesCountdown -= Time.deltaTime;

                if (delayBetweenWavesCountdown <= 0)
                {
                    StartWave();
                    delayBetweenWavesCountdown = delayBetweenWaves;
                    currentWaveNumberIncremented = false;
                }
            }
        }
    }

    /// <summary>
    /// Dequeues the next wave and starts it.
    /// </summary>
    void StartWave()
    {
        currentEnemyWave = enemyWaves.Dequeue();

        //get the total number of enemies in that wave
        //which will be decremented upon each enemy death
        numEnemiesInCurrentWave = currentEnemyWave.GetTotalNumberOfEnemies();

        //pass the wave to the enemy spawner
        //for it to spawn
        enemySpawner.SetSpawnPool(currentEnemyWave);
    }

    /// <summary>
    /// Decrement the number of enemies in this current wave.
    /// </summary>
    public void DecrementEnemyQuantity()
    {
        numEnemiesInCurrentWave--;
    }
}
