using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 蛇身视图，负责蛇身图片和点击事件。
/// </summary>
public class SnakeBodyView : MonoBehaviour
{
    /// <summary>蛇身子节点图片。</summary>
    [SerializeField] private Image bodyImage;
    /// <summary>蛇身点击按钮。</summary>
    [SerializeField] private Button clickButton;

    /// <summary>初始化蛇身图片和点击回调。</summary>
    public void Initialize(Sprite sprite, Action clickAction)
    {
        bodyImage.sprite = sprite;
        bodyImage.raycastTarget = true;
        clickButton.onClick.RemoveAllListeners();
        clickButton.onClick.AddListener(() => clickAction());
        gameObject.SetActive(true);
    }

    /// <summary>回收蛇身并清除点击事件。</summary>
    public void Recycle()
    {
        clickButton.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }
}
