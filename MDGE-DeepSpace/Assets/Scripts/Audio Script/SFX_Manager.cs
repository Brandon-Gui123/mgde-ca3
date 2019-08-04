using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFX_Manager : MonoBehaviour
{

    public AudioSource ClickSFX;
    public AudioSource LevelClickSFX;
    public AudioSource BackSFX;
    

    void Start()
    {
        ClickSFX.volume = PlayerPrefs.GetFloat("SFXVolume", 0.2f);
        LevelClickSFX.volume = PlayerPrefs.GetFloat("SFXVolume", 0.2f);
        BackSFX.volume = PlayerPrefs.GetFloat("SFXVolume", 0.2f);
        Debug.Log("ClickSFX " + ClickSFX.volume);
        Debug.Log("LevelClickSFX " + LevelClickSFX.volume);
        Debug.Log("BackSFX " + BackSFX.volume);
        
    }

    // Update is called once per frame
    void Update()
    {

        ClickSFX.volume = PlayerPrefs.GetFloat("SFXVolume", 0.2f);
        LevelClickSFX.volume = PlayerPrefs.GetFloat("SFXVolume", 0.2f);
        BackSFX.volume = PlayerPrefs.GetFloat("SFXVolume", 0.2f);
    }

    public void playerClickSFX()
    {
        ClickSFX.Play();
    }

    public void levelClickSFX()
    {
        LevelClickSFX.Play();
    }

    public void playBackSFX()
    {
        BackSFX.Play();
    }

    

}
