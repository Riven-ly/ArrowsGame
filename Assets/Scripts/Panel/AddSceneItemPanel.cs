using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddSceneItemPanel : UIBase
{
    public Button hideBtn;
    public RewardAdButton rewardAdButton;
    public Text limitText;
    public Text ex;
    public Image icon;

    private GameSceneItemBase gameSceneitemBase;
    private void Start()
    {
        hideBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            Hide();
        });
    }
    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        gameSceneitemBase = data as GameSceneItemBase;

        icon.sprite = gameSceneitemBase.clickBtn.image.sprite;
        icon.SetNativeSize();
        limitText.text = $"{LanguageManager.Instance.GetText("Limit")}:{gameSceneitemBase.eachRoundItemUseCnt}/{gameSceneitemBase.eachRoundItemUseCntMax}";
        switch (gameSceneitemBase.type)
        {
            case SceneItemType.item_hint:
                ex.text = LanguageManager.Instance.GetText("AddSceneItemPanel_ex2");
                break;
            case SceneItemType.item_AutoClick:
                ex.text = LanguageManager.Instance.GetText("AddSceneItemPanel_ex1");
                break;
        }

        rewardAdButton.Init(AdsCallback, "AddSceneItemPanel", false);
    }
    public override void Hide()
    {
        base.Hide();
    }

    private void AdsCallback()
    {
        AddCallback(() =>
        {
            switch (gameSceneitemBase.type)
            {
                case SceneItemType.item_hint:
                    GameManager.Instance.playerInfo.Add_item_hint(1);
                    break;
                case SceneItemType.item_AutoClick:
                    GameManager.Instance.playerInfo.Add_item_autoClick(1);
                    break;
            }
            AudioManager.Instance.PlaySceneSingleMusic("GetItem");
            gameSceneitemBase.Refresh();
            GameManager.Instance.SavePlayerInfo();
        });

        Hide();
    }
}
