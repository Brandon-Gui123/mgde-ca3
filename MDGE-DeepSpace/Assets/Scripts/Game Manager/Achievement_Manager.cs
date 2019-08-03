using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Achievement_Manager : MonoBehaviour
{//This class is for setting up the achivement page and setting values according


    public bool isWinAchievement1 = false;
    public bool isWinAchievement2 = false;
    public bool isWinAchievement3 = false;

    public GameObject frame1;
    public GameObject frame2;
    public GameObject frame3;

    public void Start()
    {
        //When the canvas starts up load information about the save files
        Achievement_data data = Achievement_Sava.AchievementLoad();
        isWinAchievement1 = data.framevalue1;
        isWinAchievement2 = data.framevalue2;
        isWinAchievement3 = data.framevalue3;
    }

    public void SaveAchievementInfo() //a method that will be append to the exit button to save info when leaving the game.
    {
        Achievement_Sava.AchievementSave(this);
    }

    public void setAchievementFrames()
    {
        if (isWinAchievement1 == true)
        {
            frame1.SetActive(true);
        }
        else { frame1.SetActive(false); }

        if (isWinAchievement2 == true)
        {
            frame2.SetActive(true);
        }
        else { frame2.SetActive(false); }

        if (isWinAchievement3 == true)
        {
            frame3.SetActive(true);
        }
        else { frame3.SetActive(false); }
    }


    public void AchievementChangeOnWin(int levelwon, bool value) //used to change achievement frame, to be used after winning the game. 
                                                                 //Levelwon indicate scene number, value set to true to show completion
    {
        switch (levelwon)
        {
            case 1: isWinAchievement1 = value;
                break;
            case 2: isWinAchievement2 = value;
                break;
            case 3: isWinAchievement3 = value;
                break;
        }

    }

}
