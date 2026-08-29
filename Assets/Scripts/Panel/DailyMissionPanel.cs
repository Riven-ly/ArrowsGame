using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyMissionPanel : UIBase
{
    private const int RequiredVideoCount = 30;
    private const string DateKey = "DailyMissionDate";
    private const string VideoCountKey = "DailyMissionVideoCount";
    private const string ClaimedKey = "DailyMissionClaimed";
    private const string RevenueSumKey = "DailyMissionRevenueSum";
    private const string RevenueCountKey = "DailyMissionRevenueCount";

    public Text progressText;
    public Text nextUpdateText;
    public Text Subtitle;

    public RewardAdButton watchButton;
    public Button closeButton;

    private Coroutine countdownCoroutine;
    private DateTime currentTime;

    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            Hide();
        });
        Subtitle.text = string.Format(LanguageManager.Instance.GetText("SubtitleEx"), LanguageManager.Instance.GetText_Encrypt("cht"));
    }

    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        currentTime = GameManager.Instance.GetNowTime();
        ResetForNewDay();
        watchButton.Init(OnVideoRewarded, "DailyMissionPanel", true);
        RefreshView();
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }
        countdownCoroutine = StartCoroutine(Countdown());
    }

    public override void Hide()
    {
        AddCallback(() =>
        {
            if (countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
                countdownCoroutine = null;
            }
        });
        base.Hide();
    }

    private void OnVideoRewarded()
    {
        OtherSdkManager.Instance.CustomEvent("dailytask_ad");
        if (IsClaimed() || GetVideoCount() >= RequiredVideoCount)
        {
            return;
        }
        RecordAdRevenue(AdManager.Instance.GetJustNowAdRevenue());
        PlayerPrefs.SetInt(VideoCountKey, Mathf.Min(GetVideoCount() + 1, RequiredVideoCount));
        if (GetVideoCount() >= RequiredVideoCount)
        {
            ClaimReward();
            return;
        }

        DOTween.Sequence().AppendInterval(0.1f).AppendCallback(() =>
        {
            watchButton.Init(OnVideoRewarded, "DailyMissionPanel", true);
            RefreshView();
        });
    }

    private void RefreshView()
    {
        int videoCount = GetVideoCount();
        string DailyMissionEx2 = LanguageManager.Instance.GetText("DailyMissionEx2");
        string unit = LanguageManager.Instance.GetText_Encrypt("Special_Diamond_unit");
        progressText.text = string.Format(DailyMissionEx2, RequiredVideoCount, $"{unit}{AdManager.Instance.ConvertGoldToLocalCurrency(GetGoldReward())}", $"{videoCount}/{RequiredVideoCount}");
        watchButton.UpdateAdsBtnState(!IsClaimed() && videoCount < RequiredVideoCount);
        RefreshNextUpdateTime();
    }

    private void RefreshNextUpdateTime()
    {
        TimeSpan remaining = currentTime.Date.AddDays(1d) - currentTime;
        string DailyMissionEx3 = LanguageManager.Instance.GetText("DailyMissionEx3");
        string timestr = remaining.Hours.ToString("00") + ":" + remaining.Minutes.ToString("00") + ":" + remaining.Seconds.ToString("00");
        nextUpdateText.text = string.Format(DailyMissionEx3, timestr);
    }

    private System.Collections.IEnumerator Countdown()
    {
        while (true)
        {
            RefreshNextUpdateTime();
            yield return new WaitForSecondsRealtime(1f);
            currentTime = currentTime.AddSeconds(1d);
            if (ResetForNewDay())
            {
                RefreshView();
            }
        }
    }

    private bool ResetForNewDay()
    {
        string today = currentTime.ToString("yyyyMMdd");
        if (PlayerPrefs.GetString(DateKey, string.Empty) == today)
        {
            return false;
        }

        PlayerPrefs.SetString(DateKey, today);
        PlayerPrefs.SetInt(VideoCountKey, 0);
        PlayerPrefs.SetInt(ClaimedKey, 0);
        PlayerPrefs.SetFloat(RevenueSumKey, 0f);
        PlayerPrefs.SetInt(RevenueCountKey, 0);
        PlayerPrefs.Save();
        return true;
    }

    private int GetVideoCount()
    {
        return PlayerPrefs.GetInt(VideoCountKey, 0);
    }

    private bool IsClaimed()
    {
        return PlayerPrefs.GetInt(ClaimedKey, 0) != 0;
    }

    public float GetGoldReward()
    {
        return GetGoldRewardInternal() / (float)PlayerInfo.CurrencyUnitScale;
    }

    private int GetGoldRewardInternal()
    {
        float averageRevenue = PlayerPrefs.GetInt(RevenueCountKey, 0) > 0
            ? PlayerPrefs.GetFloat(RevenueSumKey, 0f) / PlayerPrefs.GetInt(RevenueCountKey, 0)
            : 0.005f;
        return Mathf.RoundToInt(averageRevenue * 100f * RequiredVideoCount * PlayerInfo.CurrencyUnitScale);
    }

    private void RecordAdRevenue(float revenue)
    {
        PlayerPrefs.SetFloat(RevenueSumKey, PlayerPrefs.GetFloat(RevenueSumKey, 0f) + revenue);
        PlayerPrefs.SetInt(RevenueCountKey, PlayerPrefs.GetInt(RevenueCountKey, 0) + 1);
        PlayerPrefs.Save();
    }

    public void ClaimReward()
    {
        if (IsClaimed() || GetVideoCount() < RequiredVideoCount)
        {
            return;
        }

        PlayerPrefs.SetInt(ClaimedKey, 1);
        List<ItemData> itemDatas = new List<ItemData>
        {
            new ItemData(ItemType.GoldDui, GetGoldRewardInternal())
        };
        UIManager.Instance.OpenUI<GeneralRewardPanel2>(itemDatas);
        RefreshView();
    }
}
