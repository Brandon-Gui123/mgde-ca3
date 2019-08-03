using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    /// <summary>
    /// The maximum amount of health that this enemy can have.
    /// </summary>
    [Header("Common Enemy Properties")]
    public float maxHealth = 100;

    /// <summary>
    /// The amount of health this enemy has.
    /// The enemy will die if its health reaches 0.
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
    /// Obtains or sets (child classes only) the boolean responsible for giving information
    /// on whether this enemy died or not.
    /// </summary>
    /// <value>Is this enemy dead?</value>
    public bool IsDead { get; protected set; }

    /// <summary>
    /// The reference to a wave manager script component in the scene.
    /// This is used to allow destroyed enemies to decrement the total number of enemies
    /// in the wave.
    /// This is a better alternative to searching for every enemy using GameObject.Find, because
    /// this is more performant.
    /// </summary>
    public WaveManager waveManager;


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

    /// <summary>
    /// Damages the enemy.
    /// </summary>
    /// <param name="damage">The amount of damage to apply to the enemy.</param>
    public void Damage(float damage)
    {
        //no need to damage the enemy since it is already dead
        if (IsDead)
        {
            return;
        }

        if (health - damage <= 0)
        {
            health = 0;

            //the enemy is dead
            IsDead = true;

            //decrement a value from the total number of enemies in the current wave
            if (waveManager)
            {
                waveManager.DecrementEnemyQuantity();
            }

            //execute callback function
            OnDie();
        }
        else
        {
            health -= damage;
        }
    }

    /// <summary>
    /// Callback function that gets called when the enemy's health reaches 0.
    /// </summary>
    protected abstract void OnDie();
}