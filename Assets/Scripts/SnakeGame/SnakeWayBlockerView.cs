using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 路径阻挡器视图，显示关卡中的锁定阻挡格。
/// </summary>
public class SnakeWayBlockerView : MonoBehaviour
{
    /// <summary>阻挡器图片。</summary>
    [SerializeField] private Image blockerImage;
    /// <summary>阻挡器剩余时间文本。</summary>
    [SerializeField] private Text timerText;
    /// <summary>剩余存在时间。</summary>
    private float remainingTime;
    /// <summary>时间结束回调。</summary>
    private Action expireCallback;

    /// <summary>初始化阻挡器显示。</summary>
    public void Initialize(float lockTime, Action onExpired)
    {
        blockerImage.raycastTarget = false;
        remainingTime = lockTime;
        expireCallback = onExpired;
        timerText.text = Mathf.FloorToInt(remainingTime).ToString();
        gameObject.SetActive(true);
    }

    /// <summary>处理阻挡器自身倒计时。</summary>
    private void Update()
    {
        if (remainingTime <= 0f) return;

        remainingTime -= Time.deltaTime;
        timerText.text = Mathf.Max(0, Mathf.FloorToInt(remainingTime)).ToString();
        if (remainingTime <= 0f)
        {
            Action callback = expireCallback;
            expireCallback = null;
            callback?.Invoke();
        }
    }

    /// <summary>回收阻挡器对象。</summary>
    public void Recycle()
    {
        expireCallback = null;
        remainingTime = 0f;
        timerText.text = "0";
        gameObject.SetActive(false);
    }
}
