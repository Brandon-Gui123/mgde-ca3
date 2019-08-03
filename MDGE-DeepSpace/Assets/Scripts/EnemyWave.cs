using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An enemy wave consists of multiple sets of enemies that will spawn.
/// </summary>
[System.Serializable]
public class EnemyWave : MonoBehaviour
{

    /// <summary>
    /// A collection of the sets of enemies that will be spawning in this wave.
    /// </summary>
    public EnemySet[] enemySets;

    /// <summary>
    /// Obtains the total number of enemies from this wave.
    /// </summary>
    /// <returns>An integer representing the number of enemies in this wave.</returns>
    public int GetTotalNumberOfEnemies()
    {
        int numEnemies = 0;

        foreach (EnemySet enemySet in enemySets)
        {
            numEnemies += enemySet.quantity;
        }

        return numEnemies;
    }

}
