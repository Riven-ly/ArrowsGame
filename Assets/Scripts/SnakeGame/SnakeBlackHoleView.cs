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
        holeImage.raycastTarget = false;
        DOTween.Sequence().Append(holeImage.transform.DOLocalRotate(
                  new Vector3(0, 0, 360f),
                  2f,
                  RotateMode.FastBeyond360
              ).SetEase(Ease.Linear))
              .SetTarget(holeImage.transform)
              .SetLoops(-1);
        gameObject.SetActive(true);

    }

    /// <summary>回收黑洞对象。</summary>
    public void Recycle()
    {
        holeImage.transform.DOKill();
        gameObject.SetActive(false);
    }
}
