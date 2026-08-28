using DG.Tweening;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class OtherRewardTaskView : MonoBehaviour
{
    public Image icon;
    public Image icon2;

    [SerializeField] private Slider bonusSlider;
    [SerializeField] private Text bonusText;
    [SerializeField] private Text bonusRewardText;
    [SerializeField] private Text bonusNextRewardText;
    [SerializeField] private Slider rewardSlider;
    [SerializeField] private Text rewardText;
    [SerializeField] private Text rewardTextLabel;

    public void Refresh()
    {
        GameManager.Instance.UpdateAppATTToDiamond(icon);
        GameManager.Instance.UpdateAppATTToDiamond(icon2);

        RefreshBonusTask();
        RefreshRewardTask();
    }


    private void RefreshBonusTask()
    {
        int count = Mathf.Min(OtherRewardTask.Instance.BonusAdsCount, OtherRewardTask.BonusTaskMaxCount);
        int nextPercent = GetNextBonusPercent(count);
        int stageMaxCount = GetBonusStageMaxCount(count);
        bonusText.text = count + "/" + stageMaxCount;
        bonusRewardText.text = "+" + BonusPercent(count) + "%" + LanguageManager.Instance.GetText("Extras");

        //bonusNextRewardText.gameObject.SetActive(nextPercent > 0);
        //if (nextPercent > 0)
        //{
           // bonusNextRewardText.text = "+" + nextPercent + "%";
        //}
        bonusNextRewardText.text = "+" + nextPercent + "%" + LanguageManager.Instance.GetText("Extras");

        bonusSlider.value = GetBonusSliderValue(count);
 
    }

    private void RefreshRewardTask()
    {
        int count = OtherRewardTask.Instance.RewardAdsCount;
        rewardText.text = count + "/50";
        string unit = LanguageManager.Instance.GetText_Encrypt("Special_Diamond_unit");
        string wd = LanguageManager.Instance.GetText_Encrypt("wd");
        rewardTextLabel.text = string.Format(LanguageManager.Instance.GetText("otherrewardEx2"),50, wd, $"{unit}{510}");
        float targetValue = count / 50f;
        rewardSlider.value = targetValue;
        if (count >= 50)
        {
            UIManager.Instance.OpenUIMask();
            DOTween.Sequence().AppendInterval(0.2f).AppendCallback(() =>
            {
                UIManager.Instance.HideUIMask();
                ClaimReward();
            });
        }
    }

    public int BonusPercent(int tagetV)
    {
        if (tagetV >= 50) return 20;
        if (tagetV >= 30) return 10;
        if (tagetV >= 10) return 5;
        return 0;
    }

    private int GetNextBonusPercent(int count)
    {
        if (count >= 50) return 20;
        if (count >= 30) return 20;
        if (count >= 10) return 10;
        return 5;
    }

    private int GetBonusStageMaxCount(int count)
    {
        if (count >= 30) return 50;
        if (count >= 10) return 30;
        return 10;
    }

    private float GetBonusSliderValue(int count)
    {
        return Mathf.Clamp01(count / (float)GetBonusStageMaxCount(count));
    }

    private void ClaimReward()
    {
        List<ItemData> itemDatas = new List<ItemData>
        {
            new ItemData(ItemType.GoldDui, 51000)
        };
        UIManager.Instance.OpenUI<GeneralRewardPanel2>(itemDatas, () =>
        {
            OtherRewardTask.Instance.ResetRewardAdsCount();
            RefreshRewardTask();
        });

    }
}
