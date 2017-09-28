using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsHandler : MonoBehaviour {

    public string AndroidAdId, IOSAdId;
    public int rewardAmount = 25;
    public static AdsHandler instance;
	// Use this for initialization
	void Start () {
        instance = this;
        Initialize();
	}

    public void Initialize() {
#if UNITY_ANDROID
        Advertisement.Initialize(AndroidAdId);
#elif UNITY_IOS
        Advertisement.Initialize(IOSAdId);
#endif
    }

    public void ShowRewardedAd()
    {
        const string RewardedPlacementId = "rewardedVideo";
        print("Here we are");
        if (!Advertisement.IsReady(RewardedPlacementId))
        {
            Debug.Log(string.Format("Ads not ready for placement '{0}'", RewardedPlacementId));
            return;
        }

        var options = new ShowOptions { resultCallback = HandleShowResult };
        Advertisement.Show(RewardedPlacementId, options);

    }

    void RewardNow() {
        PlayerPrefs.SetInt(GlobalVals.coinString, PlayerPrefs.GetInt(GlobalVals.coinString, 100) + rewardAmount);
        MainNetHandler.instance.CoinsTxt.text = PlayerPrefs.GetInt(GlobalVals.coinString).ToString();
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                RewardNow();
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
        Initialize();

    }


}
