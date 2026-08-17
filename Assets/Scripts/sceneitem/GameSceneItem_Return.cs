using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneItem_Return : GameSceneItemBase
{
    /// <summary>
    /// 刷新撤销道具的数量与解锁状态。
    /// </summary>
    public override void Refresh()
    {
        base.Refresh();
        cnt = GameManager.Instance.playerInfo.gameSceneItem_Return;
        type = SceneItemType.item_Return;
        lockLv = 1;

        bool isLock = GameManager.Instance.playerInfo.level < lockLv;

        unLockTrans.gameObject.SetActive(!isLock);
        lockTrans.gameObject.SetActive(isLock);

        cntStr.text = cnt <= 0 ? "+" : GameManager.Instance.playerInfo.gameSceneItem_Return.ToString();

        clickBtn.gameObject.SetActive(cnt > 0);
        cntStr.gameObject.SetActive(cnt > 0);
        rewardAdButton.gameObject.SetActive(cnt <= 0);
        rewardAdButton.Init(AdsCallback, "", false);
    }

    public override void AdsCallback()
    {
        base.AdsCallback();

        GameManager.Instance.playerInfo.Add_item_return(1);
        GameManager.Instance.SavePlayerInfo();
        DOTween.Sequence().AppendInterval(0.1f).AppendCallback(() =>
        {
            Refresh();
        });
    }

    public override void OnClick()
    {
        base.OnClick();
        if (cnt <= 0)
        {
            return;
        }

        bool isUseItemSucceed = TryUseItem();
        if (isUseItemSucceed)
        {
            GameManager.Instance.playerInfo.Minus_item_return(1);
            Refresh();
        }
    }

    public override bool TryUseItem()
    {
        return base.TryUseItem();
    }
}
