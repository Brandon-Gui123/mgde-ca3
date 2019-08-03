using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// A static reference to the GameManager script component.
    /// </summary>
    public static GameManager gameManager;

    /// <summary>
    /// The amount of money the player has.
    /// Used for upgrading and building turrets, and is awarded when enemies are killed.
    /// </summary>
    public int money = 0;

    CanvasManager sceneController;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = this;
        
        sceneController = FindObjectOfType<CanvasManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReloadScene() // Reloads the Scene
    {
        sceneController.ReloadScene();
    }

    public void ToMenu() //Goes back to Start Menu
    {
       sceneController.UpdateLevelChange(0);
        sceneController.LevelSelection(0);

    }
}
