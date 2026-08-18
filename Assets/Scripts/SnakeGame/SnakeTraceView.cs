using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 蛇占格痕迹视图，只显示蛇初始化时占用过的棋盘格。
/// </summary>
public class SnakeTraceView : MonoBehaviour
{
    /// <summary>痕迹图片。</summary>
    [SerializeField] private Image traceImage;

    /// <summary>初始化痕迹颜色。</summary>
    public void Initialize()
    {
        traceImage.raycastTarget = false;
        gameObject.SetActive(true);
    }

    /// <summary>回收痕迹对象。</summary>
    public void Recycle()
    {
        gameObject.SetActive(false);
    }
}
