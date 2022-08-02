using UnityEngine;
using GoogleMobileAds.Api;
using System;
using Firebase.Analytics;

public class AdMobRealtek : MonoBehaviour
{
    private InterstitialAd interstitial;

    void Start()
    {
        if (GetComponent<SaveAndLoadGameData>().GetAdsBuy())
        {
            RequestInterstitial();
        }
    }
    private void RequestInterstitial()
    {
        string adUnitId = "ca-app-pub-7826298636457656/3605141934";
        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitial.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;


        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);

    }

    public void AdShow()
    {
        if (GetComponent<SaveAndLoadGameData>().GetAdsBuy())
        {
            if (this.interstitial.IsLoaded())
            {
                this.interstitial.Show();
            }
        }else if(GetComponent<Game>().isTowerDifficuly && !GetComponent<SaveAndLoadGameData>().GetAdsBuy())
            GetComponent<Game>().BackToHome();
    }


    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        FirebaseAnalytics.LogEvent("ad_click", new Parameter("type", "AdLoad"));
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {

    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }
}

