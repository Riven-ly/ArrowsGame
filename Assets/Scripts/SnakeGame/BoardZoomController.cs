using UnityEngine;

/// <summary>棋盘缩放控制，支持手机双指和编辑器鼠标滚轮。</summary>
public class BoardZoomController : MonoBehaviour
{
    /// <summary>编辑器鼠标滚轮每格的缩放增量。</summary>
    [SerializeField] private float wheelZoomStep = 0.1f;
    /// <summary>手机双指距离变化对应的缩放速度。</summary>
    [SerializeField] private float pinchZoomSpeed = 1f;
    /// <summary>需要缩放的棋盘根节点。</summary>
    private RectTransform boardRoot;
    private float maxZoom = 1f;
    private float currentZoom = 1f;
    /// <summary>上一帧的双指间距。</summary>
    private float previousTouchDistance;
    /// <summary>是否已经记录了上一帧双指间距。</summary>
    private bool hasPreviousTouchDistance;

    private void Awake()
    {
        boardRoot = GetComponent<RectTransform>();
    }

    /// <summary>按蛇部件当前尺寸重置缩放，并计算最大缩放倍率。</summary>
    public void Initialize(Vector3 partScale)
    {
        maxZoom = Mathf.Max(1f, Mathf.Min(1f / partScale.x, 1f / partScale.y));
        currentZoom = 1f;
        boardRoot.localScale = Vector3.one;
        hasPreviousTouchDistance = false;
    }

    private void Update()
    {
        // 手机端使用双指距离变化控制缩放。
        if (Input.touchCount >= 2)
        {
            if (UIManager.Instance.CheckIstheUIopen())
            {
                return;
            }

            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);
            float touchDistance = Vector2.Distance(firstTouch.position, secondTouch.position);
            if (hasPreviousTouchDistance)
            {
                float delta = (touchDistance - previousTouchDistance) / Screen.dpi;
                if (float.IsNaN(delta) || float.IsInfinity(delta))
                {
                    delta = (touchDistance - previousTouchDistance) / Screen.height;
                }
                SetZoom(currentZoom + delta * pinchZoomSpeed);
            }
            previousTouchDistance = touchDistance;
            hasPreviousTouchDistance = true;
            return;
        }

        hasPreviousTouchDistance = false;
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

    /// <summary>限制缩放范围并应用到整个棋盘。</summary>
    private void SetZoom(float value)
    {
        currentZoom = Mathf.Clamp(value, 1f, maxZoom);
        boardRoot.localScale = Vector3.one * currentZoom;
    }
}
