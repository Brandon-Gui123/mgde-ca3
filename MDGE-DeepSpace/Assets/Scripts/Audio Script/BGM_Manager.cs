using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class BGM_Manager : MonoBehaviour
{

    public AudioSource MainmenuBGM;
    public AudioSource GamePlayBGM;
    private AudioSource BGM;
    public int scene;

    // Start is called before the first frame update
    void Start()
    {
        scene = 0;
        if (scene == 0)
        {
            BGM = MainmenuBGM;
            BGM.Play();
        }
        if (scene == 1)
        {
            BGM = GamePlayBGM;
            BGM.Play();
        }



        
        BGM.volume = PlayerPrefs.GetFloat("MusicVolume", 0.2f);
        Debug.Log("music " + BGM.volume);
        Debug.Log("BGM " + BGM);
    }

    // Update is called once per frame
    void Update()
    {
        if (scene == 0)
        {
            GamePlayBGM.Stop();
            BGM = MainmenuBGM;
            

        }
        if (scene == 1)
        {
            MainmenuBGM.Stop();
            BGM = GamePlayBGM;
            

        }
        if(BGM.isPlaying == false)
        {
            BGM.Play();
            Debug.Log("Scene " + scene);
            Debug.Log("BGM " + BGM);
        }
        
        

        BGM.volume = PlayerPrefs.GetFloat("MusicVolume", 0.2f);
    }
    //public void PlayGame(int levelindex)
    //{

    //    SceneManager.LoadScene(levelindex); // use scene manager to load scene based on scene index


    //}

   public void SceneInt0()
    {
        scene = 0;
    }

    public void SceneInt1()
    {
        scene = 1;
    }
}
