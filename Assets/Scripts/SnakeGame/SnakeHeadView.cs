using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 蛇头视图，负责蛇头图片、箭头方向和点击事件。
/// </summary>
public class SnakeHeadView : MonoBehaviour
{
    public CanvasGroup hintCanvasGroup;
    public Transform hintItemEffect;
    public Animation anim;
    /// <summary>蛇头子节点图片。</summary>
    [SerializeField] private Image headImage;
    /// <summary>蛇头点击按钮。</summary>
    [SerializeField] private Button clickButton;

    private float timer;
    private float animTime = 2f;
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > animTime)
        {
            timer = 0f;
            anim.Play("snakeHeadAnim");
        }
    }

    /// <summary>初始化蛇头图片、方向和点击回调。</summary>
    public void Initialize(Sprite sprite, SnakeGameModel.MoveDirection direction, Action clickAction)
    {
        HideHint();
        headImage.sprite = sprite;
        SetDirection(direction);
        headImage.raycastTarget = true;
        clickButton.onClick.RemoveAllListeners();
        clickButton.onClick.AddListener(() => clickAction());
        gameObject.SetActive(true);

        animTime = UnityEngine.Random.Range(2f, 6f);
        timer = 0f;
    }

    /// <summary>显示预制体中已经配置好方向的提示线。</summary>
    public void ShowHint(float duration)
    {
        DOTween.Kill(hintCanvasGroup);
        hintItemEffect.gameObject.SetActive(true);
        hintCanvasGroup.alpha = 0f;
        hintCanvasGroup.DOFade(1f, 0.2f).SetTarget(hintCanvasGroup);
        DOTween.Sequence().SetTarget(hintCanvasGroup).AppendInterval(duration).AppendCallback(() =>
        {
            hintCanvasGroup.DOFade(0f, 0.15f).SetTarget(hintCanvasGroup).OnComplete(() => hintItemEffect.gameObject.SetActive(false));
        });
    }

    /// <summary>关闭提示线并终止其补间动画。</summary>
    public void HideHint()
    {
        DOTween.Kill(hintCanvasGroup);
        hintItemEffect.gameObject.SetActive(false);
    }

    /// <summary>回收蛇头并清除点击事件。</summary>
    public void Recycle()
    {
        HideHint();
        clickButton.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }

    /// <summary>更新蛇头朝向。</summary>
    public void SetDirection(SnakeGameModel.MoveDirection direction)
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, DirectionAngle(direction));
    }

    /// <summary>以图片默认朝下为基准，将蛇方向转换为旋转角度。</summary>
    private float DirectionAngle(SnakeGameModel.MoveDirection direction)
    {
        switch (direction)
        {
            case SnakeGameModel.MoveDirection.Up: return 180f;
            case SnakeGameModel.MoveDirection.Down: return 0f;
            case SnakeGameModel.MoveDirection.Left: return -90f;
            case SnakeGameModel.MoveDirection.Right: return 90f;
            case SnakeGameModel.MoveDirection.UpRight: return 135f;
            case SnakeGameModel.MoveDirection.UpLeft: return -135f;
            case SnakeGameModel.MoveDirection.DownRight: return 45f;
            case SnakeGameModel.MoveDirection.DownLeft: return -45f;
            default: return 0f;
        }
    }
}
