using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunBullet : MonoBehaviour
{

    /// <summary>
    /// The speed at which the railgun bullet leaves its turret.
    /// </summary>
    public float speed;

    /// <summary>
    /// The position where it started.
    /// </summary>
    public Vector2 startPosition;

    /// <summary>
    /// How far the bullet can go before it is being deactivated.
    /// </summary>
    public float range;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {

    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        //move the bullet on its up vector (this is relative to object)
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        //once a certain distance is exceeded, deactivate it
        if (Vector2.Distance(startPosition, transform.position) >= range * 5)
        {
            Destroy(gameObject);
        }
    }

}
