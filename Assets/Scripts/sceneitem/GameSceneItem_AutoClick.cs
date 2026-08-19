using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneItem_AutoClick : GameSceneItemBase
{
    private bool isUseing = false;
    public override void Refresh()
    {
        base.Refresh();
        cnt = GameManager.Instance.playerInfo.gameSceneItem_AutoClick;
        type = SceneItemType.item_AutoClick;
        lockLv = 3;
        bool isLock = GameManager.Instance.playerInfo.level < lockLv;

        string firstGuid = PlayerPrefs.GetString("GameSceneItem_AutoClick");
        if (!isLock && string.IsNullOrEmpty(firstGuid))
        {
            GameManager.Instance.playerInfo.Add_item_autoClick(1);
            PlayerPrefs.SetString("GameSceneItem_AutoClick","yes");
        }

        unLockTrans.gameObject.SetActive(!isLock);
        lockTrans.gameObject.SetActive(isLock);

        cntStr.text = cnt <= 0 ? "+" : GameManager.Instance.playerInfo.gameSceneItem_AutoClick.ToString();
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

        if(eachRoundItemUseCnt >= eachRoundItemUseCntMax)
        {
            string str = $"单局道具最多使用 {eachRoundItemUseCntMax} 次!";
            UIManager.Instance.OpenUI<GeneralTipsPanel>(str);
            return;
        }

        if(isUseing)
        {
            return;
        }

        bool isUseItemSucceed = TryUseItem();
        if (isUseItemSucceed)
        {
            eachRoundItemUseCnt++;
            StartCoroutine(AutoClickSnakes());
            GameManager.Instance.playerInfo.Minus_item_autoClick(1);
            Refresh();
        }
    }

    /// <summary>以 0.2 秒间隔补充执行后两次自动点击。</summary>
    private IEnumerator AutoClickSnakes()
    {
        isUseing = true;
        SnakeGameController controller = UIManager.Instance.GetUI<GameScenePanel>().snakeGameController;
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(0.5f);
            controller.TryAutoClickSafeSnake();
        }
        isUseing = false;
    }

    public override bool TryUseItem()
    {
        SnakeGameController controller = UIManager.Instance.GetUI<GameScenePanel>().snakeGameController;
        if (!controller.TryAutoClickSafeSnake())
        {
            return false;
        }
        return true;
    }
}
