using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamikaze : MonoBehaviour
{
    public float enemy_health;
    public float damagedealt;
    public GameObject player;

    private Player_Health_Script playerhealth;

   // private BoxCollider2D collider;


    // Start is called before the first frame update
    void Start()
    {
        playerhealth = player.GetComponent<Player_Health_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    void OnTriggerEnter2D(Collider2D collider)
    {/*
        print("Player has been Kamikazed!");
        if (collider.gameObject.CompareTag( "Player"))
        {

            playerhealth.playerDamaged(damagedealt);
          //  Destroy(this.gameObject);
        }
        */
    }
}
