using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    //TODO: Add stuff common in all enemies

    /// <summary>
    /// The amount of health this enemy has.
    /// </summary>
    public float health = 100;

    /// <summary>
    /// The Transform component of the player.
    /// This will be the target of all enemies.
    /// We use Transform so we can access the player's position directly.
    /// </summary>
    public Transform player;

    /// <summary>
    /// The distace that this enemy will travel every second.
    /// </summary>
    public float speed;

    /// <summary>
    /// The script component responsible for handling player health calculations.
    /// </summary>
    protected Player_Health_Script playerHealthScript;

    /// <summary>
    /// Determines the angle that the turret has to rotate in order to look at a given point.
    /// </summary>
    /// <param name="origin">The position of the turret.</param>
    /// <param name="target">The position of the target.</param>
    /// <returns></returns>
    protected float DetermineAngle(Vector2 origin, Vector2 target)
    {

        float horizontal = target.x - origin.x;
        float vertical = target.y - origin.y;

        return Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg - 90;

    }

    /// <summary>
    /// Causes the turret to look at the target.
    /// </summary>
    protected void AimAtTarget()
    {
        transform.rotation = Quaternion.Euler(0, 0, DetermineAngle(transform.position, player.position));
    }
}