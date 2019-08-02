using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A data type made to group both the enemy type to spawn and the number of enemies
/// of that type to spawn.
/// </summary>
[System.Serializable]
public class EnemySet
{
    /// <summary>
    /// The script component of the enemy that will be spawned.
    /// When this, a component, is instantiated, the entire GameObject would be
    /// instantiated as well.
    /// This allows the field to only be able to accept children classes that
    /// inherit this class, yet still being able to instantiate the enemy.
    /// </summary>
    public EnemyController enemyType;

    /// <summary>
    /// The number of enemies to spawn.
    /// </summary>
    public uint quantity;
}
