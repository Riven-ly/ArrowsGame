using DG.Tweening;
using System;
using UnityEngine;


public class AdManager : MonoBehaviour
{
    public static AdManager Instance;

    public static bool ShowAdIcon = true;
    //-----------------------------------------------------------
    public ApplovinMaxRewardOperator applovinMaxRewardOperator;
    public ApplovinMaxInterstitialOperator applovinMaxInterstitialOperator;
    //private string SDK_key = "PbbJng_h8aD16wZWrSaHN5gtVDExorX-b1ywfx8Gal1WlU7kvbWVDpzsPARTTLwex_cbeU8SGZanUXSoA1WDMx";//测试
    private string SDK_key = "vbAhRh/XXEdxApkGSJLXJVce+fSh1+/94j9P7LzNTYgtV0ukP77sxULxX42BJ1uYMzy3E8fzhS4/+JFwbui3IaHkoZGo6I8k6/Al0ZHXXSIAzcowAgrQV+5MBAqe4wEf2FXpDxMR64Y=";
    //------
    private float AdRevenue;
    private void Awake()
    {
        Instance = this;
        Init();
    }

    public void Init()
    {
        Debug.Log("Max SDK init");

        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdk.SdkConfiguration sdkConfiguration) =>
        {
            Debug.Log("Max SDK succes");
            applovinMaxRewardOperator.Init();
            applovinMaxInterstitialOperator.Init();
        };

        string decryptedSdkKey = EncryptSDKKey.DecryptWithRandomSalt(SDK_key);
        //Debug.Log("解密结果（还原原值）：" + decryptedSdkKey);
        MaxSdk.SetSdkKey(decryptedSdkKey);
        MaxSdk.InitializeSdk();
    }

    /// <summary>
    /// 激励广告(有)
    /// </summary>
    public void ShowRewardedAd(string _page_id, Action _rewardCallback, Action _displayErrorCallback)
    {
        DOTween.Sequence().AppendInterval(0.5F).AppendCallback(() =>
        {
            SetAdRevenue(UnityEngine.Random.Range(0.0002f, 0.008f));
            EventManager.Instance.TriggerEvent(GameEvent.PlayAds, AdRevenue);
            _rewardCallback?.Invoke();

        });
        // applovinMaxRewardOperator.RewardReceivedCallback = _rewardCallback;
        // applovinMaxRewardOperator.RewardDisplayErrorCallback = _displayErrorCallback;
        //applovinMaxRewardOperator.ShowRewardedAd();
    }

    /// <summary>
    /// 激励广告(无)
    /// </summary>
    public void ShowRewardedAd2(string _page_id, Action _rewardCallback, Action _displayErrorCallback)
    {
        DOTween.Sequence().AppendInterval(0.5F).AppendCallback(() =>
        {
            _rewardCallback?.Invoke();

        });
    }

    /// <summary>
    /// 插屏广告
    /// </summary>
    public void OnClickInterstitialAd(string _page_id, bool isClick = true)
    {
        applovinMaxInterstitialOperator.OnClickInterstitialAd(isClick);
    }

    public void SetAdRevenue(float _AdRevenue)
    {
        AdRevenue = _AdRevenue;
    }

    public float GetJustNowAdRevenue()
    {
        return AdRevenue;
    }

    public float ConvertGoldToLocalCurrency(float gold)
    {
        if (LanguageManager.Instance.type == MultilingualType.Portuguese)
        {
            return MathF.Round(gold * 0.026f, 2);
        }

        if (LanguageManager.Instance.type == MultilingualType.Indonesian)
        {
            return MathF.Round(gold * 89f, 2);
        }
        return MathF.Round(gold * 0.005f, 2);
    }

    public float ConvertUsdToLocalCurrency(float usd)
    {
        if (LanguageManager.Instance.type == MultilingualType.Portuguese)
        {
            return MathF.Round(usd * 5.2f, 2);
        }

        if (LanguageManager.Instance.type == MultilingualType.Indonesian)
        {
            return MathF.Round(usd * 17800f, 2);
        }

        return MathF.Round(usd, 2);
    }

    public int GetJustNowAdRevenueToGold()
    {
        float unitGold = 200f;
        float ConversionRatio = 0.5f;
        float targetGold = AdRevenue * ConversionRatio * unitGold * PlayerInfo.CurrencyUnitScale;
        int gameGold = Mathf.RoundToInt(targetGold);
        if (gameGold < 1)
        {
            gameGold = 1;
        }
        return gameGold;
    }
}