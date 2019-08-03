using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    //This canvas creates a simpleton which guides canvas through all scenes, handles event logics such as scene change, 
    //set UI active and disable UI
    private static CanvasManager Instance;

    public GameObject pauseMenuUI; //pause menu in game level
    public GameObject pauseButtonUI; //button to pause
    public GameObject mainMenuUI; //main menu in start menu

    public GameObject gameOverUI; //game over when player is dead 
    public GameObject OptionsMenuUI; //options menu


    public bool GameIsPause = false;
    //public GameObject joystickUI;
    private bool isVibrationON;
   
    
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);

        }
        else
        {
            print("Instance set");
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }



    public void UpdateLevelChange(int level) //use on buttons that will change scene, excluding play button. Disables menu or pause depedning on scene
    {
        if (level != 0)
        {
            mainMenuUI.SetActive(false);
            pauseButtonUI.SetActive(true);

        }
        else
        {
            print("Main Menu : " + mainMenuUI.activeSelf);
            mainMenuUI.SetActive(true);
            pauseButtonUI.SetActive(false);
            print("Main Menu : " + mainMenuUI.activeSelf);
            pauseMenuUI.SetActive(false);
        }

    }

    public void OPTIONSUpdateLevelChange() //Use this one, on buttons that close and open the option menu. 

    {
        //Note: Turning off option page is already set in Option buttion using Unity setactive.
        if (SceneManager.GetActiveScene().buildIndex == 0 && mainMenuUI.activeSelf == false) //in menu and menuPage is turned off
        {

            mainMenuUI.SetActive(true); //turn on menu page and turn off option page

        }

        if (SceneManager.GetActiveScene().buildIndex != 0 && pauseMenuUI.activeSelf == false) //in level and pause page is turned off, we want to set opposite
        {
            pauseMenuUI.SetActive(true); //same as above logic


        }
    }

    public void setGameOverScreen() //sets game over screen to true
    {
        gameOverUI.SetActive(true);
    }

    public void setVibration(bool value)
    {
        isVibrationON = value;
        print("Vibration is: " + value);
    }

    public bool getVibration()
    {
       return isVibrationON; 
    }



    public void setTimeScale(int timeScale) //allows setting of desired timescale.
    {
        Time.timeScale = timeScale;
    }

    public void LevelSelection(string levelname) { SceneManager.LoadScene(levelname); } //change level by name

    public void LevelSelection(int levelindex) { SceneManager.LoadScene(levelindex); } //change level by scene index

    public void QuitGame() { Application.Quit(); } //quit app

    public void ReloadScene() { SceneManager.LoadScene(SceneManager.GetActiveScene().name); pauseMenuUI.SetActive(false); } //reset scne


    public void Resume() // Resumes game play
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPause = false;
        //try
        //{
        //    //joystickUI.SetActive(true);

        //    //GameObject.Find("Variable Joystick").SetActive(true);
        //    joyStickUI.SetActive(true);
        //}
        //catch (System.Exception)
        //{

        //}

    }

    public void Pause() // Pauses game play
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPause = true;
        //joystickUI.SetActive(false);
        //try
        //{
        //    joyStickUI = GameObject.Find("Variable Joystick");
        //    joyStickUI.SetActive(false);
        //}
        //catch (System.Exception)
        //{

        //}


    }

}
