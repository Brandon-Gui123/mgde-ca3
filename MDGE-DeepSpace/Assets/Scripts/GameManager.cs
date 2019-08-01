using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    CanvasManager sceneController;
    // Start is called before the first frame update
    void Start()
    {
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
