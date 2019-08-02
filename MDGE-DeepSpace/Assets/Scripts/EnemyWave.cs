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
    public EnemySet[] waveSets;

}
