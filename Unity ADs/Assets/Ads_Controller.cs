using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;


public class Ads_Controller : MonoBehaviour
{
    private string store_id = "3223164";

    private string video_ad = "video";
    private string banner_ad = "bannerAd";
    private string rewarded_video_ad = "rewardedVideo";



    // Start is called before the first frame update
    void Start()
    {
        Advertisement.Initialize(store_id, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (Advertisement.IsReady(video_ad))
            {
                Advertisement.Show(video_ad);
            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (Advertisement.IsReady(banner_ad))
            {
                Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
                Advertisement.Banner.Show(banner_ad);
            }

        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            Advertisement.Banner.Hide(true);
        }


    }
}

