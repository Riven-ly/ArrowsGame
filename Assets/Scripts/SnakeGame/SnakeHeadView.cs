using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 蛇头视图，负责蛇头图片、箭头方向和点击事件。
/// </summary>
public class SnakeHeadView : MonoBehaviour
{
    /// <summary>蛇头子节点图片。</summary>
    [SerializeField] private Image headImage;
    /// <summary>蛇头点击按钮。</summary>
    [SerializeField] private Button clickButton;

    /// <summary>初始化蛇头图片、方向和点击回调。</summary>
    public void Initialize(Sprite sprite, SnakeGameModel.MoveDirection direction, Action clickAction)
    {
        headImage.sprite = sprite;
        headImage.raycastTarget = true;
        clickButton.onClick.RemoveAllListeners();
        clickButton.onClick.AddListener(() => clickAction());
        gameObject.SetActive(true);
    }

    /// <summary>回收蛇头并清除点击事件。</summary>
    public void Recycle()
    {
        clickButton.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }

    /// <summary>将蛇方向转换为箭头角度。</summary>
    private float DirectionAngle(SnakeGameModel.MoveDirection direction)
    {
        switch (direction)
        {
            case SnakeGameModel.MoveDirection.Up: return 90f;
            case SnakeGameModel.MoveDirection.Down: return -90f;
            case SnakeGameModel.MoveDirection.Left: return 180f;
            case SnakeGameModel.MoveDirection.UpRight: return 45f;
            case SnakeGameModel.MoveDirection.UpLeft: return 135f;
            case SnakeGameModel.MoveDirection.DownRight: return -45f;
            case SnakeGameModel.MoveDirection.DownLeft: return -135f;
            default: return 0f;
        }
    }
}
