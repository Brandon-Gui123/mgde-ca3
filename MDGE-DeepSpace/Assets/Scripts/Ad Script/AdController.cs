using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdController : MonoBehaviour
{
    private string store_id = "3242057"; // Android ID for "Deep Space"

    //private string video_ad = "video";
    //private string rewarded_video_ad = "rewardedVideo";
    private string banner_ad = "bannerAd";
    private string interstitial_ad = "insterstitialVideo";

    

    // Start is called before the first frame update
    void Start()
    {
        Advertisement.Initialize(store_id, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            if (Advertisement.Banner.isLoaded)
            {
                Advertisement.Banner.Hide();
            }
           
        }
    }


    public void ShowBannerAdBtmCtr()
    {
        if (Advertisement.IsReady(banner_ad))
        {
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
            Advertisement.Banner.Show(banner_ad);
        }
    }
    public void ShowBannerAdTopRight()
    {
        if (Advertisement.IsReady(banner_ad))
        {
            Advertisement.Banner.SetPosition(BannerPosition.TOP_RIGHT);
            Advertisement.Banner.Show(banner_ad);
        }
    }
    public void ShowBannerAdCenter()
    {
        if (Advertisement.IsReady(banner_ad))
        {
            Advertisement.Banner.SetPosition(BannerPosition.CENTER);
            Advertisement.Banner.Show(banner_ad);
        }
    }

    public void HideBannerAd()
    {
        if (Advertisement.Banner.isLoaded)
        {
            Advertisement.Banner.Hide();
        }
        
        
    }

    public void ShowInterstitialAd()
    {
        if (Advertisement.IsReady(interstitial_ad))
        {

            Advertisement.Show(interstitial_ad);

        }
    }
}
