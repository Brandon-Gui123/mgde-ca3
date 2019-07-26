using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamikaze : MonoBehaviour
{
    public float enemy_health;
    public float damagedealt;
    public GameObject player;

    private Player_Health_Script playerhealth;
    //AI
    public float speed;
    public Transform target;
    public float stoppingDistance;


    // private BoxCollider2D collider;


    // Start is called before the first frame update
    void Start()
    {
        playerhealth = player.GetComponent<Player_Health_Script>();
        //AI
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        //AI
        if (Vector2.Distance(transform.position, target.position) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        print("Player has been Kamikazed!");
        if (collider.gameObject.CompareTag("Player"))
        {

            playerhealth.playerDamaged(damagedealt);
            Destroy(this.gameObject);
        }
        
    }
}
