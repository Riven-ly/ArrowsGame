using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>棋盘缩放控制，支持手机双指和编辑器鼠标滚轮。</summary>
public class BoardZoomController : MonoBehaviour
{
    /// <summary>编辑器鼠标滚轮每格的缩放增量。</summary>
    [SerializeField] private float wheelZoomStep = 0.1f;
    /// <summary>手机双指距离变化对应的缩放速度。</summary>
    [SerializeField] private float pinchZoomSpeed = 1f;
    /// <summary>需要缩放的棋盘根节点。</summary>
    [SerializeField] private RectTransform boardRoot;
    /// <summary>单指拖动棋盘的滚动视图。</summary>
    [SerializeField] private ScrollRect scrollRect;
    /// <summary>判定为缩放操作所需的最小双指距离变化（像素）。</summary>
    [SerializeField] private float pinchDistanceThreshold = 2f;
    private float maxZoom = 1f;
    private float currentZoom = 1f;
    /// <summary>上一帧的双指间距。</summary>
    private float previousTouchDistance;
    /// <summary>是否已经记录了上一帧双指间距。</summary>
    private bool hasPreviousTouchDistance;
    private int firstTouchId = -1;
    private int secondTouchId = -1;
    private bool isPinching;

    private void Awake()
    {
    }

    /// <summary>按蛇部件当前尺寸重置缩放，并计算最大缩放倍率。</summary>
    public void Initialize(Vector3 partScale)
    {
        maxZoom = Mathf.Max(1f, Mathf.Min(1f / partScale.x, 1f / partScale.y));
        DOTween.Kill(this);
        currentZoom = 1f;
        boardRoot.localScale = Vector3.one;
        hasPreviousTouchDistance = false;
        firstTouchId = -1;
        secondTouchId = -1;
        isPinching = false;
    }

    private void Update()
    {
        // 手机端使用双指距离变化控制缩放。
        if (Input.touchCount >= 2)
        {
            if (UIManager.Instance.CheckIstheUIopen())
            {
                hasPreviousTouchDistance = false;
                firstTouchId = -1;
                secondTouchId = -1;
                return;
            }

            if (!isPinching)
            {
                scrollRect.StopMovement();
                scrollRect.enabled = false;
                isPinching = true;
            }

            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);
            float touchDistance = Vector2.Distance(firstTouch.position, secondTouch.position);
            if (firstTouchId != firstTouch.fingerId || secondTouchId != secondTouch.fingerId ||
                firstTouch.phase == TouchPhase.Began || secondTouch.phase == TouchPhase.Began)
            {
                firstTouchId = firstTouch.fingerId;
                secondTouchId = secondTouch.fingerId;
                previousTouchDistance = touchDistance;
                hasPreviousTouchDistance = true;
                return;
            }

            if (hasPreviousTouchDistance)
            {
                float distanceDelta = touchDistance - previousTouchDistance;
                if (Mathf.Abs(distanceDelta) >= pinchDistanceThreshold)
                {
                    float delta = distanceDelta / Screen.dpi;
                    if (float.IsNaN(delta) || float.IsInfinity(delta))
                    {
                        delta = distanceDelta / Screen.height;
                    }
                    SetZoom(currentZoom + delta * pinchZoomSpeed);
                }
            }
            previousTouchDistance = touchDistance;
            hasPreviousTouchDistance = true;
            return;
        }

        hasPreviousTouchDistance = false;
        firstTouchId = -1;
        secondTouchId = -1;
        if (isPinching)
        {
            scrollRect.enabled = true;
            isPinching = false;
        }

        // 编辑器和 PC 端使用鼠标滚轮控制缩放。
        if (Input.mouseScrollDelta.y != 0f)
        {
            if (UIManager.Instance.CheckIstheUIopen())
            {
                return;
            }

            SetZoom(currentZoom + Input.mouseScrollDelta.y * wheelZoomStep);
        }
    }

    /// <summary>将棋盘平滑放大到固定的提示查看倍率。</summary>
    public void FocusZoom()
    {
        float targetZoom = Mathf.Min(maxZoom, 1.5f);
        DOTween.Kill(this);
        if (Mathf.Approximately(currentZoom, targetZoom))
        {
            SetZoom(targetZoom);
            return;
        }
        DOTween.To(() => currentZoom, SetZoom, targetZoom, 0.35f).SetTarget(this);
    }

    /// <summary>限制缩放范围并应用到整个棋盘。</summary>
    private void SetZoom(float value)
    {
        currentZoom = Mathf.Clamp(value, 1f, maxZoom);
        boardRoot.localScale = Vector3.one * currentZoom;
    }
}
