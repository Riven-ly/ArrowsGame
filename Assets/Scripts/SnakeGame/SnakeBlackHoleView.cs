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
        gameObject.SetActive(true);
    }

    /// <summary>回收黑洞对象。</summary>
    public void Recycle()
    {
        gameObject.SetActive(false);
    }
}
