using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum SceneItemType
{
    /// <summary>
    /// Ã· æ
    /// </summary>
    item_hint,
    /// <summary>
    /// œ¥≈∆
    /// </summary>
    item_Exchange,
    /// <summary>
    /// ≥∑ªÿ
    /// </summary>
    item_Return,
}
public class GameSceneItemBase : MonoBehaviour
{
    public SceneItemType type;
    public Button clickBtn;
    public Button lockBtn;
    public Text cntStr;
    public Canvas canvas;

    public Transform unLockTrans;
    public Transform lockTrans;

    public RewardAdButton rewardAdButton;

    protected int cnt;
    protected int lockLv = 1;

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

    public void CanvasTop()
    {
        canvas.sortingOrder = 505;
    }
    public void CanvasRecover()
    {
        canvas.sortingOrder = 99;
    }

    public void ScaleAnim()
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
        DOTween.Sequence()
               .Append(transform.DOScale(1.1f, 0.2f))
               .Append(transform.DOScale(0.9f, 0.1f))
               .Append(transform.DOScale(1f, 0.1f))
               .SetTarget(transform);
    }


    public virtual void AdsCallback()
    {

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
