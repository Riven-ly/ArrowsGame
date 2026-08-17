using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneItem_Hint : GameSceneItemBase
{
    public override void Refresh()
    {
        base.Refresh();
        cnt = GameManager.Instance.playerInfo.gameSceneItem_Hint;
        type = SceneItemType.item_hint;
        lockLv = 1;

        bool isLock = GameManager.Instance.playerInfo.level < lockLv;

        unLockTrans.gameObject.SetActive(!isLock);
        lockTrans.gameObject.SetActive(isLock);

        cntStr.text = cnt <= 0 ? "+" : GameManager.Instance.playerInfo.gameSceneItem_Hint.ToString();

        clickBtn.gameObject.SetActive(cnt > 0);
        cntStr.gameObject.SetActive(cnt > 0);
        rewardAdButton.gameObject.SetActive(cnt <= 0);
        rewardAdButton.Init(AdsCallback, "", false);
    }

    public override void AdsCallback()
    {
        base.AdsCallback();

        GameManager.Instance.playerInfo.Add_item_hint(1);
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
            //UIManager.Instance.OpenUI<AddSceneItemPanel>(this);
            return;
        }

        EventManager.Instance.TriggerEvent(GameEvent.StopHintAnim);
        bool isUseItemSucceed = TryUseItem();
        if (isUseItemSucceed)
        {
            GameManager.Instance.playerInfo.Minus_item_hint(1);
            //GameManager.Instance.SavePlayerInfo();
            Refresh();
        }
    }

    public override bool TryUseItem()
    {
        return base.TryUseItem();
    }
}
