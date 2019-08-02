using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player_Health_Script : MonoBehaviour
{

    public float start_health = 100; //starting health of the player
    public bool playerIsDead = false;
    public Image healthbar;

    public float current_health; //to initate the health of the player at start
    // Start is called before the first frame update
    void Start()
    {
        current_health = start_health; //initialise current_health, this is to prevent health from being touched.
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            playerDamaged(10);
        }

        if (playerIsDead == true)
        {
            SpriteRenderer Sprite = gameObject.GetComponent<SpriteRenderer>();
            Sprite.enabled = false;
        }

    }

    public void playerDamaged(float damage_value) //insert value to deal damage to the player
    {
        print("damage taken" + damage_value);
        current_health -= damage_value; //set the health after damage taken
        if (current_health <= 0) { current_health = 0; playerIsDead = true;  print("player is dead"); } //health cannot be negative, player is now dead

        updateHealthUI(current_health, start_health  ); //update health bar
    }

    private void updateHealthUI( float currentHealth , float maxHealth)
    {
       
            float ratio = currentHealth / maxHealth;
            healthbar.rectTransform.localScale = new Vector3(ratio, 1, 1);
        
        
    }

}
