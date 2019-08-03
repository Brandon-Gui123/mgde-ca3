using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Achievement_data 
{
    public bool framevalue1; //storing frame value of first frame
    public bool framevalue2; //storing frame value of second frame
    public bool framevalue3; //storing frame value of second frame

    public  Achievement_data(Achievement_Manager achievement)
    {
        framevalue1 = achievement.isWinAchievement1;
        framevalue2 = achievement.isWinAchievement2;
        framevalue3 = achievement.isWinAchievement3;
    }
}
