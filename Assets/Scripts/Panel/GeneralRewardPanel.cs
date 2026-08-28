using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralRewardPanel : UIBase
{
    public Transform itemRoot;

    private List<ItemData> itemDatas;
    private List<ItemBase> itemBase;

    private string page_id = "GeneralRewardPanel";

    private void OnEnable()
    {
        isOpen = true;
    }
    private void OnDisable()
    {
        isOpen = false;
        ResetPanel();
    }
    public override void Refresh(object data = null)
    {
        base.Refresh(data);

        itemDatas = data as List<ItemData>;
        itemBase = GameManager.Instance.CreatItems(itemDatas, itemRoot);

        CollectClick();
    }

    private void CollectClick()
    {
        DOTween.Sequence().AppendInterval(0.8f).AppendCallback(() =>
        {
            AudioManager.Instance.PlaySceneSingleMusic("GetItem");
        });

        PlayerInfoUI playerInfoUI = UIManager.Instance.GetUI<PlayerInfoUI>();
        UIManager.Instance.OpenUIMask();
        float awaitTime = 0.1f;
        foreach (var item in itemBase)
        {
            if (item.itemType == ItemType.Gold || item.itemType == ItemType.GoldDui)
            {
                awaitTime = 2f;
                playerInfoUI.GoldCanvasTop();
            }
            else if (item.itemType == ItemType.Diamond || item.itemType == ItemType.DiamondDui)
            {
                awaitTime = 2f;
                playerInfoUI.DiamondCanvasTop();
            }
            item.GetItemReward();
            item.PlayItemAnim();
        }
        //¶¯»­
        DOTween.Sequence().AppendInterval(awaitTime).AppendCallback(() =>
        {
            UIManager.Instance.HideUIMask();
            playerInfoUI.GoldCanvasRecover();
            playerInfoUI.DiamondCanvasRecover();
            Hide();
        });
    }

    public override void Hide()
    {
        AddCallback(() =>
        {
            GameManager.Instance.SavePlayerInfo();
        });
        base.Hide();
    }

    private void ResetPanel()
    {
        foreach (Transform item in itemRoot)
        {
            Destroy(item.gameObject);
        }
        itemDatas = null;
        itemBase = null;
    }
}
