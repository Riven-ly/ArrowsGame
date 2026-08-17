using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏场景面板及页面导航入口。
/// </summary>
public class GameScenePanel : UIBase
{
    public Transform root;

    /// <summary>
    /// 初始化安全区与生命显示引用。
    /// </summary>
    private void Awake()
    {
        RectTransform rect = root.GetComponent<RectTransform>();
        float topBlockHeight = Screen.height - Screen.safeArea.yMax;
        rect.offsetMax = new Vector2(0, -topBlockHeight);
    }
    /// <summary>
    /// 注册设置按钮事件。
    /// </summary>
    private void Start()
    {
       
    }

 

    /// <summary>
    /// 根据玩家等级读取关卡配置并开始新游戏。
    /// </summary>
    public override void Refresh(object data = null)
    {
        base.Refresh();
    }

    /// <summary>
    /// 隐藏主玩法界面并停止当前界面的全部补间动画。
    /// </summary>
    public override void Hide()
    {
        base.Hide();
    }

    /// <summary>
    /// 重置游戏
    /// </summary>
    public void ResetGame()
    {
        Refresh();
    }
}
