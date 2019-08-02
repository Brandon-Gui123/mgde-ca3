using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public Queue<EnemyWave> enemyWaves = new Queue<EnemyWave>();

    public EnemyWave currentEnemyWave;

    // Start is called before the first frame update
    void Start()
    {
        foreach (EnemyWave enemyWave in GetComponentsInChildren<EnemyWave>(false))
        {
            enemyWaves.Enqueue(enemyWave);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
