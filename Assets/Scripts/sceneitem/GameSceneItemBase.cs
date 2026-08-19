using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum SceneItemType
{
    /// <summary>
    /// 提示
    /// </summary>
    item_hint,
    /// <summary>
    /// 自动点击
    /// </summary>
    item_AutoClick,
}
public class GameSceneItemBase : MonoBehaviour
{
    public SceneItemType type;
    public Button clickBtn;
    public Button lockBtn;
    public Text cntStr;
    public Canvas canvas;
    public Text lockStr;

    public Transform unLockTrans;
    public Transform lockTrans;

    protected int cnt;
    protected int lockLv = 1;
    protected int eachRoundItemUseCnt = 0;
    protected int eachRoundItemUseCntMax = 3;

    private void Start()
    {
        clickBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            OnClick();
        });
        lockBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            //string str = string.Format(LanguageManager.Instance.GetText("LockLvTips"), lockLv);
            //UIManager.Instance.OpenUI<GeneralTipsPanel>(str);
        });
    }

    public void ResetEachRoundItemUseCnt()
    {
        eachRoundItemUseCnt = 0;
    }

    public void CanvasTop()
    {
        canvas.sortingOrder = 505;
    }
    public void CanvasRecover()
    {
        canvas.sortingOrder = 99;
    }

    public virtual void Refresh()
    {
    }
    public virtual void OnClick()
    {
    }

    public virtual bool TryUseItem()
    {
        return true;
    }
}
