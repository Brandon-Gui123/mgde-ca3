using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class Audio_Manager : MonoBehaviour
{
    public AudioMixer audioMixer;
    
    public Slider BGMVolumeSlider;
    public Slider SFXVolumeSlider;
    

    private void Start()
    {
        //BGM.volume = PlayerPrefs.GetFloat("MusicVolume",0.2f);
        BGMVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume",0.5f);
        SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume",0.5f);

        //Debug.Log("music " + BGM.volume);
        Debug.Log("volume " + BGMVolumeSlider.value);
        Debug.Log("SFXVolume " + SFXVolumeSlider.value);

    }

    private void Update()
    {
        //BGM.volume = BGMVolumeSlider.value;
        VolumePrefs();
        PlayerPrefs.Save();
        //audioMixer.SetFloat("Music", BGMVolumeSlider.value);
        //audioMixer.SetFloat("SFX", SFXVolumeSlider.value);
        
    }

    public void VolumePrefs()
    {
        PlayerPrefs.SetFloat("MusicVolume", BGMVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolumeSlider.value);
    }

    public void SetVolumeMusic(float bgmvolume)
    {
        audioMixer.SetFloat("Music", bgmvolume);
    }

    public void SetVolumeSFX(float sfxvolume)
    {
        audioMixer.SetFloat("SFX", sfxvolume);
    }

    public void ResetAudio()
    {
        PlayerPrefs.SetFloat("MusicVolume", 100);
        PlayerPrefs.SetFloat("SFXVolume", 100);
        BGMVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
    }
}
