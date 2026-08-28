using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 黑洞视图，显示关卡中的黑洞障碍格。
/// </summary>
public class SnakeBlackHoleView : MonoBehaviour
{
    /// <summary>黑洞图片。</summary>
    [SerializeField] private Image holeImage;

    /// <summary>初始化黑洞显示。</summary>
    public void Initialize()
    {
        DOTween.Kill(holeImage.transform);
        holeImage.transform.localRotation = Quaternion.identity;
        holeImage.raycastTarget = false;
        holeImage.transform
            .DOLocalRotate(new Vector3(0f, 0f, 360f), 2f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetTarget(holeImage.transform)
            .SetLoops(-1);
        gameObject.SetActive(true);
    }

    /// <summary>回收黑洞对象。</summary>
    public void Recycle()
    {
        DOTween.Kill(holeImage.transform);
        holeImage.transform.localRotation = Quaternion.identity;
        gameObject.SetActive(false);
    }
}
