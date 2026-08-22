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

    /// <summary>初始化蛇尾图片、方向和点击回调。</summary>
    public void Initialize(Sprite sprite, SnakeGameModel.MoveDirection direction, Action clickAction)
    {
        tailImage.sprite = sprite;
        SetDirection(direction);
        tailImage.raycastTarget = true;
        clickButton.onClick.RemoveAllListeners();
        clickButton.onClick.AddListener(() => clickAction());
        gameObject.SetActive(true);
    }

    /// <summary>更新蛇尾朝向。</summary>
    public void SetDirection(SnakeGameModel.MoveDirection direction)
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, DirectionAngle(direction));
    }

    /// <summary>以图片默认朝下为基准，将蛇尾方向转换为旋转角度。</summary>
    private float DirectionAngle(SnakeGameModel.MoveDirection direction)
    {
        switch (direction)
        {
            case SnakeGameModel.MoveDirection.Up: return 180f;
            case SnakeGameModel.MoveDirection.Left: return -90f;
            case SnakeGameModel.MoveDirection.Right: return 90f;
            case SnakeGameModel.MoveDirection.UpRight: return 135f;
            case SnakeGameModel.MoveDirection.UpLeft: return -135f;
            case SnakeGameModel.MoveDirection.DownRight: return 45f;
            case SnakeGameModel.MoveDirection.DownLeft: return -45f;
            default: return 0f;
        }
    }

    /// <summary>回收蛇尾并清除点击事件。</summary>
    public void Recycle()
    {
        clickButton.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }
}
