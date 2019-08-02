using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaveManager : MonoBehaviour
{

    //TODO: Add documentation

    public static WaveManager waveManager;

    public Queue<EnemyWave> enemyWaves = new Queue<EnemyWave>();

    private EnemyWave currentEnemyWave;

    [SerializeField]
    private EnemySpawner enemySpawner;

    public int numEnemiesInCurrentWave = 0;

    // Start is called before the first frame update
    void Start()
    {
        
        waveManager = this;

        foreach (EnemyWave enemyWave in GetComponentsInChildren<EnemyWave>(false))
        {
            enemyWaves.Enqueue(enemyWave);
        }

        //start the wave (typically the first wave is started here)
        StartWave();
    }

    // Update is called once per frame
    void Update()
    {
        //start the next wave once we get the signal to do so
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
