using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 路径阻挡器视图，显示关卡中的锁定阻挡格。
/// </summary>
public class SnakeWayBlockerView : MonoBehaviour
{
    /// <summary>阻挡器图片。</summary>
    [SerializeField] private Image blockerImage;
    /// <summary>阻挡器剩余蛇数文本。</summary>
    [SerializeField] private Text timerText;
    /// <summary>剩余需要消除的蛇数量。</summary>
    private int remainingSnakeCount;
    /// <summary>消失完成回调。</summary>
    private Action expireCallback;
    /// <summary>阻挡器消失补间。</summary>
    private Sequence disappearSequence;

    /// <summary>初始化阻挡器显示。</summary>
    public void Initialize(int snakeCount, Action onExpired)
    {
        blockerImage.raycastTarget = false;
        remainingSnakeCount = snakeCount;
        expireCallback = onExpired;
        timerText.text = remainingSnakeCount.ToString();
        disappearSequence?.Kill();
        transform.localScale = Vector3.one;
        blockerImage.color = new Color(blockerImage.color.r, blockerImage.color.g, blockerImage.color.b, 1f);
        timerText.color = new Color(timerText.color.r, timerText.color.g, timerText.color.b, 1f);
        gameObject.SetActive(true);
    }

    /// <summary>处理一条蛇成功消除。</summary>
    public void OnSnakeRemoved()
    {
        if (remainingSnakeCount <= 0) return;
        remainingSnakeCount--;
        timerText.text = remainingSnakeCount.ToString();
        if (remainingSnakeCount == 0) PlayDisappear();
    }

    /// <summary>使用 DOTween 播放缩小淡出动画并通知回收。</summary>
    private void PlayDisappear()
    {
        disappearSequence = DOTween.Sequence().SetUpdate(true);
        disappearSequence.Join(transform.DOScale(Vector3.zero, 0.25f));
        disappearSequence.Join(blockerImage.DOFade(0f, 0.25f));
        disappearSequence.Join(timerText.DOFade(0f, 0.25f));
        disappearSequence.OnComplete(() =>
        {
            Action callback = expireCallback;
            expireCallback = null;
            callback?.Invoke();
        });
    }

    /// <summary>回收阻挡器对象。</summary>
    public void Recycle()
    {
        disappearSequence?.Kill();
        disappearSequence = null;
        expireCallback = null;
        remainingSnakeCount = 0;
        timerText.text = "0";
        gameObject.SetActive(false);
    }
}
