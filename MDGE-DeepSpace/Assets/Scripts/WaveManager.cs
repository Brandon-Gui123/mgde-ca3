using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaveManager : MonoBehaviour
{

    //TODO: Add documentation

    /// <summary>
    /// A static reference to a WaveManager script component in the current scene.
    /// This is used for all enemies to easily refer to this and perform actions
    /// (right now it's just decrementing the number of enemies in the current wave)
    /// which would otherwise be performance-hurting when done in some other way.
    /// </summary>
    public static WaveManager waveManager;

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

        //start the wave (typically the first wave is started here)
        StartWave();

        delayBetweenWavesCountdown = delayBetweenWaves;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyWaves.Count <= 0)
        {
            Debug.Log("Level complete!");
        }
        else
        {

            if (numEnemiesInCurrentWave <= 0)
            {

                delayBetweenWavesCountdown -= Time.deltaTime;

                if (delayBetweenWavesCountdown <= 0)
                {
                    StartWave();
                    delayBetweenWavesCountdown = delayBetweenWaves;
                }
            }
        }
    }

    /// <summary>
    /// Dequeues the next wave and starts in.
    /// </summary>
    void StartWave()
    {
        currentEnemyWave = enemyWaves.Dequeue();

        //get the total number of enemies in that wave
        numEnemiesInCurrentWave = currentEnemyWave.GetTotalNumberOfEnemies();

        //pass the wave to the enemy spawner
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
