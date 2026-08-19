using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏场景面板及页面导航入口。
/// </summary>
public class GameScenePanel : UIBase
{
    public Transform root;
    public Text title;
    public Button settingBtn;
    public Button resetBtn;
    public GameSceneItem_AutoClick gameSceneItem_AutoClick;
    public GameSceneItem_Hint gameSceneItem_Hint;

    /// <summary>蛇玩法模块控制器。</summary>
    public SnakeGameController snakeGameController;
    /// <summary>当前剩余生命。</summary>
    private int currentLives;
    /// <summary>面板上的三个生命图片。</summary>
    [SerializeField] private Image[] lifeImages;
    /// <summary>是否已经结束当前关卡。</summary>
    private bool isGameOver;
    /// <summary>游戏总时长，单位为秒。</summary>
    private float gameDuration = 600f;
    /// <summary>剩余游戏时间文本。</summary>
    [SerializeField] private Text gameTimeText;
    /// <summary>蛇数量文本。</summary>
    [SerializeField] private Text snakeCountText;
    /// <summary>蛇撞到阻挡时显示的错误提示图片。</summary>
    [SerializeField] private Image snakeError;
    /// <summary>错误提示淡入淡出时长。</summary>
    private float snakeErrorFadeDuration = 0.15f;
    /// <summary>错误提示每次闪烁时长。</summary>
    private float snakeErrorFlashDuration = 0.1f;
    /// <summary>当前剩余游戏时间。</summary>
    private float remainingGameTime;

    /// <summary>
    /// 初始化安全区与生命显示引用。
    /// </summary>
    private void Awake()
    {
        RectTransform rect = root.GetComponent<RectTransform>();
        snakeError.gameObject.SetActive(false);
        float topBlockHeight = Screen.height - Screen.safeArea.yMax;
        rect.offsetMax = new Vector2(0, -topBlockHeight);
        snakeGameController.collisionEvent += OnSnakeCollision;
        snakeGameController.victoryEvent += OnSnakeVictory;
        snakeGameController.snakeCountChangedEvent += RefreshSnakeCount;
    }

    private void OnEnable()
    {
        isOpen = false;
    }

    private void OnDisable()
    {
        isOpen = false;
    }

    /// <summary>解除玩法事件订阅。</summary>
    private void OnDestroy()
    {
        DOTween.Kill(snakeError);
        if (snakeGameController != null)
        {
            snakeGameController.collisionEvent -= OnSnakeCollision;
            snakeGameController.victoryEvent -= OnSnakeVictory;
            snakeGameController.snakeCountChangedEvent -= RefreshSnakeCount;
        }
    }
    /// <summary>
    /// 注册设置按钮事件。
    /// </summary>
    private void Start()
    {
        settingBtn.onClick.AddListener(() =>
        {

        });
        resetBtn.onClick.AddListener(() =>
        {
            ResetGame();
        });
    }

    /// <summary>按秒更新游戏倒计时。</summary>
    private System.Collections.IEnumerator GameTimer()
    {
        while (!isGameOver && remainingGameTime > 0f)
        {
            yield return new WaitForSeconds(1f);
            if (GameManager.IsGamePause) continue;
            remainingGameTime--;
            RefreshGameTime();
        }
        if (!isGameOver)
        {
            GameOver();
        }
    }

 

    /// <summary>
    /// 根据玩家等级读取关卡配置并开始新游戏。
    /// </summary>
    public override void Refresh(object data = null)
    {
        base.Refresh(data);
        StopCoroutine("GameTimer");
        DOTween.Kill(snakeError);
        snakeError.gameObject.SetActive(false);
        title.text = LanguageManager.Instance.GetText("Level") + $" {GameManager.Instance.playerInfo.level}";
        currentLives = 3;
        remainingGameTime = gameDuration;
        snakeGameController.Initialize();
        RefreshLifeImages();
        RefreshGameTime();
        RefreshSnakeCount();
        isGameOver = false;
        GameManager.IsGamePause = false;
        gameSceneItem_AutoClick.Refresh();
        gameSceneItem_AutoClick.ResetEachRoundItemUseCnt();
        gameSceneItem_Hint.Refresh();
        gameSceneItem_Hint.ResetEachRoundItemUseCnt();
        StartCoroutine(GameTimer());
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

    /// <summary>刷新剩余时间文本。</summary>
    private void RefreshGameTime()
    {
        int totalSeconds = Mathf.FloorToInt(Mathf.Max(0f, remainingGameTime));
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        gameTimeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    /// <summary>刷新当前剩余蛇数量文本。</summary>
    private void RefreshSnakeCount()
    {
        snakeCountText.text = snakeGameController.GetRemainingSnakeCount() + "/" + snakeGameController.GetTotalSnakeCount();
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
        StartSnakeErrorEffect();
        Debug.Log("蛇撞到阻挡，扣除 1 点生命并原路倒车。");
        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    /// <summary>播放蛇撞到阻挡时的错误提示效果。</summary>
    private void StartSnakeErrorEffect()
    {
        if (SettingPanel.IsVibrateEnabled)
        {
            Handheld.Vibrate();
        }
        DOTween.Kill(snakeError);
        snakeError.gameObject.SetActive(true);
        snakeError.color = new Color(snakeError.color.r, snakeError.color.g, snakeError.color.b, 0f);
        Sequence sequence = DOTween.Sequence().SetTarget(snakeError);
        sequence.Append(snakeError.DOFade(1f, snakeErrorFadeDuration));
        sequence.Append(snakeError.DOFade(0f, snakeErrorFlashDuration));
        sequence.Append(snakeError.DOFade(1f, snakeErrorFlashDuration));
        sequence.Append(snakeError.DOFade(0f, snakeErrorFlashDuration));
        sequence.OnComplete(() => snakeError.gameObject.SetActive(false));
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

    private void GameOver()
    {
        isGameOver = true;
        snakeGameController.StopInput();
        Debug.Log("关卡失败！");
    }
}
