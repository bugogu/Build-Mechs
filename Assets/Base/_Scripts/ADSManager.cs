using UnityEngine;
using GoogleMobileAds.Api;

public class ADSManager : MonoSing<ADSManager>
{
    private InterstitialAd _interstitialAd;
    private RewardedAd _rewardedAd;

    private static int _adsIndex
    {
        get => PlayerPrefs.GetInt("ADS", 0);
        set => PlayerPrefs.SetInt("ADS", value);
    }

    private void Start()
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            Debug.Log("Ads Initialized !");
        });

        LoadInterstitialAd();
        LoadRewardedAd();

        _adsIndex++;
    }

#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-9373723896315528/6517042933";
    // private string _adUnitId = "ca-app-pub-3940256099942544/1033173712"; // Test ID
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-9373723896315528/6517042933";
#else
    private string _adUnitId = "unused";
#endif

#if UNITY_ANDROID
    private string _adUnitId2 = "ca-app-pub-9373723896315528/2310204618";
    //private string _adUnitId2 = "ca-app-pub-3940256099942544/5224354917"; // Test ID
#elif UNITY_IPHONE
  private string _adUnitId2 = "ca-app-pub-9373723896315528/2310204618";
#else
    private string _adUnitId2 = "unused";
#endif

    public void ShowInterstitialAd()
    {
        if (_adsIndex % 2 == 0)
        {
            if (_interstitialAd != null && _interstitialAd.CanShowAd())
            {
                _interstitialAd.Show();
            }
        }
    }

    public void ShowRewardedAd(bool moneyReward)
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                Debug.Log(System.String.Format(rewardMsg, reward.Type, reward.Amount));

                if (moneyReward)
                {
                    UIManager.Instance.UpdateCoin(((GameManager.Prestige * 15 + (GameManager.Level) + 1)) * 100);
                    UIManager.Instance.UpdateDiamond((GameManager.Prestige * 15) + GameManager.Level + 10);
                    UIManager.Instance.CloseRewardPanel();
                }

                else
                {
                    UIManager.Instance.CloseExtraDamagePanel();
                    GameManager.damageBoost += .50f;
                }

                GameManager.CollectedPrize++;

                if (GameManager.CollectedPrize >= 10)
                    GameManager.Instance.UnlockAchievement(GameManager.Instance.prizeOrientedAchievementID);

                LoadRewardedAd();
            });
        }
        else
            UIManager.Instance.RewardAdsNotReady();
    }

    public void LoadInterstitialAd()
    {
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        var adRequest = new AdRequest();

        InterstitialAd.Load(_adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                _interstitialAd = ad;
            });
    }

    public void LoadRewardedAd()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        var adRequest = new AdRequest();

        RewardedAd.Load(_adUnitId2, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedAd = ad;
            });
    }

}
