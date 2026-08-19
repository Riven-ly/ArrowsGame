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

        string firstGuid = PlayerPrefs.GetString("GameSceneItem_Hint");
        if (!isLock && string.IsNullOrEmpty(firstGuid))
        {
            GameManager.Instance.playerInfo.Add_item_hint(1);
            PlayerPrefs.SetString("GameSceneItem_Hint", "yes");
        }

        unLockTrans.gameObject.SetActive(!isLock);
        lockTrans.gameObject.SetActive(isLock);

        cntStr.text = cnt <= 0 ? "+" : GameManager.Instance.playerInfo.gameSceneItem_Hint.ToString();
        lockStr.text = $"Lv.{lockLv}";
    }

    public override void OnClick()
    {
        base.OnClick();
        if (cnt <= 0)
        {
            //UIManager.Instance.OpenUI<AddSceneItemPanel>(this);
            return;
        }

        if (eachRoundItemUseCnt >= eachRoundItemUseCntMax)
        {
            string str = $"单局道具最多使用 {eachRoundItemUseCntMax} 次!";
            UIManager.Instance.OpenUI<GeneralTipsPanel>(str);
            return;
        }

        EventManager.Instance.TriggerEvent(GameEvent.StopHintAnim);
        bool isUseItemSucceed = TryUseItem();
        if (isUseItemSucceed)
        {
            eachRoundItemUseCnt++;
            GameManager.Instance.playerInfo.Minus_item_hint(1);
            Refresh();
        }
    }

    public override bool TryUseItem()
    {
        return UIManager.Instance.GetUI<GameScenePanel>().snakeGameController.TryShowSafeSnakeHints(10f);
    }
}
