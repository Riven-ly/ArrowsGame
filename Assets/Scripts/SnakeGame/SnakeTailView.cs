using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 蛇尾视图，负责蛇尾图片和点击事件。
/// </summary>
public class SnakeTailView : MonoBehaviour
{
    /// <summary>蛇尾子节点图片。</summary>
    [SerializeField] private Image tailImage;
    /// <summary>蛇尾点击按钮。</summary>
    [SerializeField] private Button clickButton;

    /// <summary>初始化蛇尾图片和点击回调。</summary>
    public void Initialize(Sprite sprite, Action clickAction)
    {
        tailImage.sprite = sprite;
        tailImage.raycastTarget = true;
        clickButton.onClick.RemoveAllListeners();
        clickButton.onClick.AddListener(() => clickAction());
        gameObject.SetActive(true);
    }

    /// <summary>回收蛇尾并清除点击事件。</summary>
    public void Recycle()
    {
        clickButton.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }
}
