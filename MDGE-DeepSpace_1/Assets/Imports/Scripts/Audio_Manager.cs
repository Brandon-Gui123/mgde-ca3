using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class Audio_Manager : MonoBehaviour
{//this script is for controlling volumme
    public AudioMixer audioMixer;
  

    public void SetVolumeMusic(float volume)
    {
        audioMixer.SetFloat("Music", volume);
    }

    public void SetVolumeSFX(float volume)
    {
        audioMixer.SetFloat("SFX", volume);
    }

    
}
