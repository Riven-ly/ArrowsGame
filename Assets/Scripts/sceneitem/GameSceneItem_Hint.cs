using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneItem_Hint : GameSceneItemBase
{
    public override void Refresh()
    {
        base.Refresh();
        type = SceneItemType.item_hint;
        lockLv = 10;
        bool isLock = GameManager.Instance.playerInfo.level < lockLv;

        string firstGuid = PlayerPrefs.GetString("GameSceneItem_Hint");
        if (!isLock && string.IsNullOrEmpty(firstGuid))
        {
            GameManager.Instance.playerInfo.Add_item_hint(1);
            PlayerPrefs.SetString("GameSceneItem_Hint", "yes");
        }

        cnt = GameManager.Instance.playerInfo.gameSceneItem_Hint;
        unLockTrans.gameObject.SetActive(!isLock);
        lockTrans.gameObject.SetActive(isLock);

        cntStr.text = GameManager.Instance.playerInfo.gameSceneItem_Hint.ToString();
        cntStr.gameObject.SetActive(cnt > 0);
        addTrans.gameObject.SetActive(cnt <= 0);
        lockStr.text = $"Lv.{lockLv}";
    }

    public override void OnClick()
    {
        base.OnClick();
        if (cnt <= 0)
        {
            UIManager.Instance.OpenUI<AddSceneItemPanel>(this);
            return;
        }

        if (eachRoundItemUseCnt >= eachRoundItemUseCntMax)
        {
            string str = string.Format(LanguageManager.Instance.GetText("ItemLimit"), eachRoundItemUseCntMax);
            UIManager.Instance.OpenUI<GeneralTipsPanel>(str);
            return;
        }

        EventManager.Instance.TriggerEvent(GameEvent.StopHintAnim);
        bool isUseItemSucceed = TryUseItem();
        if (isUseItemSucceed)
        {
            AudioManager.Instance.PlaySceneSingleMusic("UseItem");
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
