using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏场景面板及页面导航入口。
/// </summary>
public class GameScenePanel : UIBase
{
    public Transform root;
    /// <summary>蛇玩法模块控制器。</summary>
    [SerializeField] private SnakeGameController snakeGameController;
    /// <summary>当前剩余生命。</summary>
    private int currentLives;
    /// <summary>面板上的三个生命图片。</summary>
    [SerializeField] private Image[] lifeImages;
    /// <summary>是否已经结束当前关卡。</summary>
    private bool isGameOver;

    /// <summary>
    /// 初始化安全区与生命显示引用。
    /// </summary>
    private void Awake()
    {
        RectTransform rect = root.GetComponent<RectTransform>();
        float topBlockHeight = Screen.height - Screen.safeArea.yMax;
        rect.offsetMax = new Vector2(0, -topBlockHeight);
        snakeGameController.collisionEvent += OnSnakeCollision;
        snakeGameController.victoryEvent += OnSnakeVictory;
    }

    /// <summary>解除玩法事件订阅。</summary>
    private void OnDestroy()
    {
        if (snakeGameController != null)
        {
            snakeGameController.collisionEvent -= OnSnakeCollision;
            snakeGameController.victoryEvent -= OnSnakeVictory;
        }
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
        base.Refresh(data);
        currentLives = 3;
        isGameOver = false;
        snakeGameController.Initialize();
        RefreshLifeImages();
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

    /// <summary>处理控制器上报的碰撞并扣除生命。</summary>
    private void OnSnakeCollision()
    {
        if (isGameOver)
        {
            return;
        }
        currentLives--;
        RefreshLifeImages();
        Debug.Log("蛇撞到阻挡，扣除 1 点生命并原路倒车。");
        if (currentLives <= 0)
        {
            isGameOver = true;
            snakeGameController.StopInput();
            Debug.Log("蛇玩法失败：生命值归零。");
        }
    }

    /// <summary>刷新面板上的三个生命图片。</summary>
    private void RefreshLifeImages()
    {
        for (int i = 0; i < lifeImages.Length; i++)
        {
            lifeImages[i].color = i < currentLives ? new Color(1f, 0.36f, 0.42f, 1f) : new Color(0.72f, 0.72f, 0.72f, 0.35f);
        }
    }

    /// <summary>处理控制器上报的全部蛇离场并打印通关结果。</summary>
    private void OnSnakeVictory()
    {
        if (isGameOver)
        {
            return;
        }
        isGameOver = true;
        Debug.Log("蛇玩法通关：全部蛇已经离场。");
    }
}
