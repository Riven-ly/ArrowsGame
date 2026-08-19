using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 蛇玩法的 UGUI 视图，负责棋盘、蛇节点与动画显示。
/// </summary>
public class SnakeGameView : MonoBehaviour
{
    public ScrollRect scrollRect;
    /// <summary>棋盘背景颜色。</summary>
    private static readonly Color BoardColor = new Color(0.97f, 0.96f, 0.91f, 1f);
    /// <summary>当前模型。</summary>
    private SnakeGameModel model;
    /// <summary>可由面板配置尺寸的棋盘根节点。</summary>
    [SerializeField] private RectTransform boardRoot;
    /// <summary>蛇头预制体。</summary>
    [SerializeField] private SnakeHeadView headPrefab;
    /// <summary>蛇身预制体。</summary>
    [SerializeField] private SnakeBodyView bodyPrefab;
    /// <summary>蛇尾预制体。</summary>
    [SerializeField] private SnakeTailView tailPrefab;
    /// <summary>蛇占格痕迹预制体。</summary>
    [SerializeField] private SnakeTraceView tracePrefab;
    /// <summary>路径阻挡器预制体。</summary>
    [SerializeField] private SnakeWayBlockerView wayBlockerPrefab;
    /// <summary>黑洞预制体。</summary>
    [SerializeField] private SnakeBlackHoleView blackHolePrefab;
    /// <summary>按蛇类型配置的头身尾图片。</summary>
    [SerializeField] private SnakeSpriteSet[] snakeSpriteSets;
    /// <summary>屏幕上边界点。</summary>
    [SerializeField] private RectTransform topBoundary;
    /// <summary>屏幕下边界点。</summary>
    [SerializeField] private RectTransform bottomBoundary;
    /// <summary>屏幕左边界点。</summary>
    [SerializeField] private RectTransform leftBoundary;
    /// <summary>屏幕右边界点。</summary>
    [SerializeField] private RectTransform rightBoundary;
    /// <summary>运行时痕迹节点容器。</summary>
    private RectTransform tracesRoot;
    /// <summary>运行时蛇节点容器。</summary>
    private RectTransform snakesRoot;
    /// <summary>运行时障碍节点容器。</summary>
    private RectTransform obstaclesRoot;
    /// <summary>闲置路径阻挡器对象池。</summary>
    private readonly List<SnakeWayBlockerView> wayBlockerPool = new List<SnakeWayBlockerView>();
    /// <summary>闲置黑洞对象池。</summary>
    private readonly List<SnakeBlackHoleView> blackHolePool = new List<SnakeBlackHoleView>();
    /// <summary>部件对象池根节点。</summary>
    private RectTransform poolRoot;
    private BoardZoomController boardZoomController;
    /// <summary>闲置蛇头对象池。</summary>
    private readonly List<SnakeHeadView> headPool = new List<SnakeHeadView>();
    /// <summary>闲置蛇身对象池。</summary>
    private readonly List<SnakeBodyView> bodyPool = new List<SnakeBodyView>();
    /// <summary>闲置蛇尾对象池。</summary>
    private readonly List<SnakeTailView> tailPool = new List<SnakeTailView>();
    /// <summary>闲置痕迹对象池。</summary>
    private readonly List<SnakeTraceView> tracePool = new List<SnakeTraceView>();
    /// <summary>蛇视图列表。</summary>
    private readonly Dictionary<int, SnakeVisual> visuals = new Dictionary<int, SnakeVisual>();
    /// <summary>移动单格动画时长。</summary>
    private const float StepDuration = 0.09f;
    /// <summary>蛇部件预制体基础宽度。</summary>
    private float partBaseWidth = 100f;
    /// <summary>蛇部件预制体基础高度。</summary>
    private float partBaseHeight = 100f;
    /// <summary>棋盘左右边距。</summary>
    private const float BoardPadding = 0f;

    /// <summary>蛇的显示对象集合。</summary>
    private class SnakeVisual
    {
        /// <summary>蛇的节点根物体。</summary>
        public RectTransform root;
        /// <summary>蛇身图片列表。</summary>
        public readonly List<Image> bodyImages = new List<Image>();
        /// <summary>蛇头图片。</summary>
        public Image headImage;
        /// <summary>蛇头视图。</summary>
        public SnakeHeadView headView;
        /// <summary>真实蛇部件对象列表。</summary>
        public readonly List<GameObject> parts = new List<GameObject>();
        /// <summary>相邻真实部件之间的视觉补间蛇身。</summary>
        public readonly List<GameObject> fillers = new List<GameObject>();
    }

    /// <summary>构建棋盘与全部运行时 UI。</summary>
    public void Build(SnakeGameModel gameModel, UnityEngine.Events.UnityAction<int> clickAction)
    {
        model = gameModel;
        ClearView();
        scrollRect.horizontalNormalizedPosition = 0.5f;
        CreateBoard();
        if (boardZoomController == null)
        {
            boardZoomController = boardRoot.transform.parent.GetComponent<BoardZoomController>();
        }
        boardZoomController.Initialize(GetPartScale());
        CreateObstacles();
        for (int i = 0; i < model.snakes.Count; i++)
        {
            CreateSnakeTraces(model.snakes[i]);
        }
        for (int i = 0; i < model.snakes.Count; i++)
        {
            CreateSnakeVisual(model.snakes[i], clickAction);
        }
        obstaclesRoot.SetAsLastSibling();
    }

    /// <summary>播放指定蛇的前进动画。</summary>
    public IEnumerator PlayForward(SnakeGameModel.SnakeData snake, List<List<Vector2Int>> layouts)
    {
        SnakeVisual visual = visuals[snake.id];
        for (int i = 1; i < layouts.Count; i++)
        {
            yield return MoveSnakeTo(visual, snake, layouts[i]);
        }
    }

    /// <summary>按完整布局逆序播放指定蛇的倒车动画。</summary>
    public IEnumerator PlayBackward(SnakeGameModel.SnakeData snake, List<List<Vector2Int>> layouts)
    {
        SnakeVisual visual = visuals[snake.id];
        for (int i = layouts.Count - 2; i >= 0; i--)
        {
            yield return MoveSnakeTo(visual, snake, layouts[i]);
        }
    }

    /// <summary>按完整布局播放蛇离场动画。</summary>
    public IEnumerator PlayExit(SnakeGameModel.SnakeData snake, List<List<Vector2Int>> layouts)
    {
        SnakeVisual visual = visuals[snake.id];
        for (int i = 1; i < layouts.Count; i++)
        {
            yield return MoveSnakeTo(visual, snake, layouts[i]);
            HidePartsBeyondBoundary(visual, snake.direction);
            HidePartsAtBlackHole(visual, snake);
        }
        while (!IsTailBeyondBoundary(visual, snake.direction) && !AllPartsHidden(visual))
        {
            List<Vector2Int> nextCells = new List<Vector2Int>(snake.cells);
            Vector2Int offset = SnakeGameModel.DirectionOffset(snake.direction);
            for (int i = nextCells.Count - 1; i > 0; i--)
            {
                nextCells[i] = nextCells[i - 1];
            }
            nextCells[0] += offset;
            yield return MoveSnakeTo(visual, snake, nextCells);
            HidePartsBeyondBoundary(visual, snake.direction);
            HidePartsAtBlackHole(visual, snake);
        }
        visual.root.gameObject.SetActive(false);
    }

    /// <summary>隐藏当前已经进入黑洞格的蛇部件。</summary>
    private void HidePartsAtBlackHole(SnakeVisual visual, SnakeGameModel.SnakeData snake)
    {
        for (int i = 0; i < snake.cells.Count; i++)
        {
            for (int j = 0; j < model.blackHoles.Count; j++)
            {
                if (snake.cells[i] == model.blackHoles[j].position)
                {
                    visual.parts[i].SetActive(false);
                    break;
                }
            }
        }
        for (int i = 0; i < visual.fillers.Count; i++)
        {
            Vector2 fillerPosition = visual.fillers[i].GetComponent<RectTransform>().anchoredPosition;
            for (int j = 0; j < model.blackHoles.Count; j++)
            {
                Vector2 holePosition = CellToPosition(model.blackHoles[j].position);
                if (Vector2.Distance(fillerPosition, holePosition) <= CellSize())
                {
                    visual.fillers[i].SetActive(false);
                    break;
                }
            }
        }
    }

    /// <summary>判断头身尾是否已经全部隐藏。</summary>
    private bool AllPartsHidden(SnakeVisual visual)
    {
        for (int i = 0; i < visual.parts.Count; i++)
        {
            if (visual.parts[i].activeSelf) return false;
        }
        for (int i = 0; i < visual.fillers.Count; i++)
        {
            if (visual.fillers[i].activeSelf) return false;
        }
        return true;
    }

    /// <summary>隐藏已经越过边界的单个蛇部件。</summary>
    private void HidePartsBeyondBoundary(SnakeVisual visual, SnakeGameModel.MoveDirection direction)
    {
        for (int i = 0; i < visual.parts.Count; i++)
        {
            if (IsPartBeyondBoundary(visual.parts[i].GetComponent<RectTransform>(), direction))
            {
                visual.parts[i].SetActive(false);
            }
        }
        for (int i = 0; i < visual.fillers.Count; i++)
        {
            if (IsPartBeyondBoundary(visual.fillers[i].GetComponent<RectTransform>(), direction))
            {
                visual.fillers[i].SetActive(false);
            }
        }
    }

    /// <summary>判断单个蛇部件是否越过对应边界点。</summary>
    private bool IsPartBeyondBoundary(RectTransform part, SnakeGameModel.MoveDirection direction)
    {
        Vector3[] corners = new Vector3[4];
        part.GetWorldCorners(corners);
        return IsBeyondBoundary(corners, direction);
    }

    /// <summary>判断蛇尾整体是否越过对应边界点。</summary>
    private bool IsTailBeyondBoundary(SnakeVisual visual, SnakeGameModel.MoveDirection direction)
    {
        RectTransform tail = visual.parts[visual.parts.Count - 1].GetComponent<RectTransform>();
        Vector3[] corners = new Vector3[4];
        tail.GetWorldCorners(corners);
        return IsBeyondBoundary(corners, direction);
    }

    /// <summary>根据移动方向判断矩形是否越过对应边界。</summary>
    private bool IsBeyondBoundary(Vector3[] corners, SnakeGameModel.MoveDirection direction)
    {
        float left = RectTransformUtility.WorldToScreenPoint(null, corners[0]).x;
        float right = RectTransformUtility.WorldToScreenPoint(null, corners[2]).x;
        float bottom = RectTransformUtility.WorldToScreenPoint(null, corners[0]).y;
        float top = RectTransformUtility.WorldToScreenPoint(null, corners[2]).y;
        float topY = RectTransformUtility.WorldToScreenPoint(null, topBoundary.position).y;
        float bottomY = RectTransformUtility.WorldToScreenPoint(null, bottomBoundary.position).y;
        float leftX = RectTransformUtility.WorldToScreenPoint(null, leftBoundary.position).x;
        float rightX = RectTransformUtility.WorldToScreenPoint(null, rightBoundary.position).x;
        switch (direction)
        {
            case SnakeGameModel.MoveDirection.Up: return bottom > topY;
            case SnakeGameModel.MoveDirection.Down: return top < bottomY;
            case SnakeGameModel.MoveDirection.Left: return right < leftX;
            case SnakeGameModel.MoveDirection.Right: return left > rightX;
            case SnakeGameModel.MoveDirection.UpRight: return bottom > topY || left > rightX;
            case SnakeGameModel.MoveDirection.UpLeft: return bottom > topY || right < leftX;
            case SnakeGameModel.MoveDirection.DownRight: return top < bottomY || left > rightX;
            default: return top < bottomY || right < leftX;
        }
    }

    /// <summary>清理视图创建的节点。</summary>
    public void ClearView()
    {
        StopAllCoroutines();
        visuals.Clear();
        if (obstaclesRoot != null)
        {
            for (int i = obstaclesRoot.childCount - 1; i >= 0; i--)
            {
                GameObject obstacle = obstaclesRoot.GetChild(i).gameObject;
                SnakeWayBlockerView blocker = obstacle.GetComponent<SnakeWayBlockerView>();
                SnakeBlackHoleView hole = obstacle.GetComponent<SnakeBlackHoleView>();
                obstacle.transform.SetParent(poolRoot, false);
                if (blocker != null) { blocker.Recycle(); wayBlockerPool.Add(blocker); }
                if (hole != null) { hole.Recycle(); blackHolePool.Add(hole); }
            }
        }        if (tracesRoot != null)
        {
            for (int i = tracesRoot.childCount - 1; i >= 0; i--)
            {
                SnakeTraceView trace = tracesRoot.GetChild(i).GetComponent<SnakeTraceView>();
                trace.transform.SetParent(poolRoot, false);
                trace.Recycle();
                tracePool.Add(trace);
            }
        }
        if (snakesRoot != null)
        {
            for (int i = snakesRoot.childCount - 1; i >= 0; i--)
            {
                RecycleSnakeParts(snakesRoot.GetChild(i));
            }
            Destroy(snakesRoot.gameObject);
            snakesRoot = null;
        }
    }

    /// <summary>创建棋盘背景、网格和标题区域。</summary>
    private void CreateBoard()
    {
        if (tracesRoot == null)
        {
            GameObject tracesObject = CreateUIObject("Traces", boardRoot);
            tracesRoot = tracesObject.GetComponent<RectTransform>();
            tracesRoot.anchorMin = Vector2.zero;
            tracesRoot.anchorMax = Vector2.one;
            tracesRoot.offsetMin = Vector2.zero;
            tracesRoot.offsetMax = Vector2.zero;
        }
        GameObject snakesObject = CreateUIObject("Snakes", boardRoot);
        snakesRoot = snakesObject.GetComponent<RectTransform>();
        snakesRoot.anchorMin = Vector2.zero;
        snakesRoot.anchorMax = Vector2.one;
        snakesRoot.offsetMin = Vector2.zero;
        snakesRoot.offsetMax = Vector2.zero;
        if (obstaclesRoot == null)
        {
            GameObject obstaclesObject = CreateUIObject("Obstacles", boardRoot);
            obstaclesRoot = obstaclesObject.GetComponent<RectTransform>();
            obstaclesRoot.anchorMin = Vector2.zero;
            obstaclesRoot.anchorMax = Vector2.one;
            obstaclesRoot.offsetMin = Vector2.zero;
            obstaclesRoot.offsetMax = Vector2.zero;
            obstaclesRoot.SetAsLastSibling();
        }
        if (poolRoot == null)
        {
            GameObject poolObject = CreateUIObject("Pool", boardRoot);
            poolRoot = poolObject.GetComponent<RectTransform>();
            poolRoot.anchorMin = Vector2.zero;
            poolRoot.anchorMax = Vector2.one;
            poolRoot.offsetMin = Vector2.zero;
            poolRoot.offsetMax = Vector2.zero;
            poolRoot.gameObject.SetActive(false);
        }
    }

    /// <summary>显示指定蛇头的提示线。</summary>
    public void ShowSnakeHints(List<int> snakeIds, float duration)
    {
        for (int i = 0; i < snakeIds.Count; i++)
        {
            if (visuals.ContainsKey(snakeIds[i]))
            {
                visuals[snakeIds[i]].headView.ShowHint(duration);
            }
        }
    }

    /// <summary>创建关卡中的路径阻挡器和黑洞。</summary>
    private void CreateObstacles()
    {
        for (int i = 0; i < model.wayBlockers.Count; i++)
        {
            SnakeWayBlockerView blocker = TakeWayBlocker();
            Vector2Int blockerPosition = model.wayBlockers[i].position;
            blocker.Initialize(model.wayBlockers[i].remainingSnakeCount, () => UIManager.Instance.GetUI<GameScenePanel>().snakeGameController.OnWayBlockerExpired(blockerPosition));
            RectTransform rect = blocker.GetComponent<RectTransform>();
            rect.anchoredPosition = CellToPosition(model.wayBlockers[i].position);
            rect.localScale = GetPartScale();
        }
        for (int i = 0; i < model.blackHoles.Count; i++)
        {
            SnakeBlackHoleView hole = TakeBlackHole();
            hole.Initialize();
            RectTransform rect = hole.GetComponent<RectTransform>();
            rect.anchoredPosition = CellToPosition(model.blackHoles[i].position);
            rect.localScale = GetPartScale();
        }
        obstaclesRoot.SetAsLastSibling();
    }

    /// <summary>通知所有有效阻挡器一条蛇已经消除。</summary>
    public void NotifySnakeRemoved()
    {
        for (int i = 0; i < obstaclesRoot.childCount; i++)
        {
            SnakeWayBlockerView blocker = obstaclesRoot.GetChild(i).GetComponent<SnakeWayBlockerView>();
            if (blocker != null) blocker.OnSnakeRemoved();
        }
    }

    /// <summary>隐藏并回收指定位置的路径阻挡器。</summary>
    public void HideWayBlocker(Vector2Int position)
    {
        for (int i = obstaclesRoot.childCount - 1; i >= 0; i--)
        {
            SnakeWayBlockerView blocker = obstaclesRoot.GetChild(i).GetComponent<SnakeWayBlockerView>();
            if (blocker != null && blocker.GetComponent<RectTransform>().anchoredPosition == CellToPosition(position))
            {
                blocker.Recycle();
                blocker.transform.SetParent(poolRoot, false);
                wayBlockerPool.Add(blocker);
                return;
            }
        }
    }

    /// <summary>取用一个路径阻挡器对象。</summary>
    private SnakeWayBlockerView TakeWayBlocker()
    {
        if (wayBlockerPool.Count == 0) return Instantiate(wayBlockerPrefab, obstaclesRoot);
        SnakeWayBlockerView result = wayBlockerPool[wayBlockerPool.Count - 1];
        wayBlockerPool.RemoveAt(wayBlockerPool.Count - 1);
        result.transform.SetParent(obstaclesRoot, false);
        result.gameObject.SetActive(true);
        return result;
    }

    /// <summary>取用一个黑洞对象。</summary>
    private SnakeBlackHoleView TakeBlackHole()
    {
        if (blackHolePool.Count == 0) return Instantiate(blackHolePrefab, obstaclesRoot);
        SnakeBlackHoleView result = blackHolePool[blackHolePool.Count - 1];
        blackHolePool.RemoveAt(blackHolePool.Count - 1);
        result.transform.SetParent(obstaclesRoot, false);
        result.gameObject.SetActive(true);
        return result;
    }

    /// <summary>创建一条蛇初始占格的静态痕迹。</summary>
    private void CreateSnakeTraces(SnakeGameModel.SnakeData snake)
    {
        for (int i = 0; i < snake.cells.Count; i++)
        {
            SnakeTraceView trace = TakeTrace();
            trace.Initialize();
            RectTransform traceRect = trace.GetComponent<RectTransform>();
            trace.transform.SetParent(tracesRoot, false);
            trace.name = "Trace_" + snake.id + "_" + i;
            traceRect.anchoredPosition = CellToPosition(snake.cells[i]);
            traceRect.localScale = GetPartScale();
        }
    }

    /// <summary>从痕迹对象池取用一个对象。</summary>
    private SnakeTraceView TakeTrace()
    {
        if (tracePool.Count == 0) return Instantiate(tracePrefab, poolRoot);
        SnakeTraceView result = tracePool[tracePool.Count - 1];
        tracePool.RemoveAt(tracePool.Count - 1);
        result.gameObject.SetActive(true);
        return result;
    }

    /// <summary>从三类部件对象池创建一条蛇的显示对象。</summary>
    private void CreateSnakeVisual(SnakeGameModel.SnakeData snake, UnityEngine.Events.UnityAction<int> clickAction)
    {
        SnakeVisual visual = new SnakeVisual();
        GameObject snakeObject = CreateUIObject("Snake_" + snake.id, snakesRoot);
        visual.root = snakeObject.GetComponent<RectTransform>();
        visual.root.anchorMin = new Vector2(0.5f, 0.5f);
        visual.root.anchorMax = new Vector2(0.5f, 0.5f);
        visual.root.sizeDelta = Vector2.zero;
        for (int i = 0; i < snake.cells.Count; i++)
        {
            GameObject part;
            Image image;
            if (i == 0)
            {
                SnakeHeadView head = TakeHead();
                head.Initialize(GetSnakeSprite(snake.type).head, snake.direction, () => clickAction(snake.id));
                part = head.gameObject;
                image = part.GetComponentInChildren<Image>(true);
            }
            else if (i == snake.cells.Count - 1)
            {
                SnakeTailView tail = TakeTail();
                tail.Initialize(GetSnakeSprite(snake.type).tail, () => clickAction(snake.id));
                part = tail.gameObject;
                image = part.GetComponentInChildren<Image>(true);
            }
            else
            {
                SnakeBodyView body = TakeBody();
                body.Initialize(GetSnakeSprite(snake.type).body, () => clickAction(snake.id));
                part = body.gameObject;
                image = part.GetComponentInChildren<Image>(true);
            }
            part.name = "Segment_" + i;
            part.transform.SetParent(snakeObject.transform, false);
            part.transform.SetSiblingIndex(snake.cells.Count - 1 - i);
            visual.parts.Add(part);
            visual.bodyImages.Add(image);
        }
        for (int i = 0; i < snake.cells.Count - 1; i++)
        {
            SnakeBodyView filler = TakeBody();
            filler.Initialize(GetSnakeSprite(snake.type).body, () => clickAction(snake.id));
            filler.name = "Filler_" + i;
            filler.transform.SetParent(snakeObject.transform, false);
            filler.transform.SetSiblingIndex(i + 1);
            visual.fillers.Add(filler.gameObject);
        }
        SetRealPartSiblingOrder(visual);
        visual.headImage = visual.bodyImages[0];
        visual.headView = visual.parts[0].GetComponent<SnakeHeadView>();
        visuals.Add(snake.id, visual);
        UpdateSnakeVisual(visual, snake);
    }

    /// <summary>固定真实头身尾的显示层级，蛇尾在最下、蛇头在最上。</summary>
    private void SetRealPartSiblingOrder(SnakeVisual visual)
    {
        int lastPartIndex = visual.parts.Count - 1;
        for (int i = 0; i < visual.parts.Count; i++)
        {
            visual.parts[i].transform.SetSiblingIndex((lastPartIndex - i) * 2);
        }
        for (int i = 0; i < visual.fillers.Count; i++)
        {
            visual.fillers[i].transform.SetSiblingIndex((visual.fillers.Count - 1 - i) * 2 + 1);
        }
    }

    /// <summary>按蛇类型取得头身尾图片组。</summary>
    private SnakeSpriteSet GetSnakeSprite(SnakeGameModel.SnakeType type)
    {
        return snakeSpriteSets[(int)type];
    }

    /// <summary>取用一个蛇头对象。</summary>
    private SnakeHeadView TakeHead()
    {
        if (headPool.Count == 0) return Instantiate(headPrefab, poolRoot);
        SnakeHeadView result = headPool[headPool.Count - 1];
        headPool.RemoveAt(headPool.Count - 1);
        result.gameObject.SetActive(true);
        return result;
    }

    /// <summary>取用一个蛇身对象。</summary>
    private SnakeBodyView TakeBody()
    {
        if (bodyPool.Count == 0) return Instantiate(bodyPrefab, poolRoot);
        SnakeBodyView result = bodyPool[bodyPool.Count - 1];
        bodyPool.RemoveAt(bodyPool.Count - 1);
        result.gameObject.SetActive(true);
        return result;
    }

    /// <summary>取用一个蛇尾对象。</summary>
    private SnakeTailView TakeTail()
    {
        if (tailPool.Count == 0) return Instantiate(tailPrefab, poolRoot);
        SnakeTailView result = tailPool[tailPool.Count - 1];
        tailPool.RemoveAt(tailPool.Count - 1);
        result.gameObject.SetActive(true);
        return result;
    }

    /// <summary>回收蛇根节点下的全部部件。</summary>
    private void RecycleSnakeParts(Transform snakeRoot)
    {
        for (int i = snakeRoot.childCount - 1; i >= 0; i--)
        {
            Transform part = snakeRoot.GetChild(i);
            SnakeHeadView head = part.GetComponent<SnakeHeadView>();
            SnakeBodyView body = part.GetComponent<SnakeBodyView>();
            SnakeTailView tail = part.GetComponent<SnakeTailView>();
            part.SetParent(poolRoot, false);
            if (head != null) { head.Recycle(); headPool.Add(head); }
            else if (body != null) { body.Recycle(); bodyPool.Add(body); }
            else { tail.Recycle(); tailPool.Add(tail); }
        }
    }

    /// <summary>移动一格并同步整条蛇的节点位置。</summary>
    private IEnumerator MoveSnakeTo(SnakeVisual visual, SnakeGameModel.SnakeData snake, List<Vector2Int> nextCells)
    {
        Vector2Int[] targetCells = nextCells.ToArray();
        Vector2[] startPositions = new Vector2[nextCells.Count];
        Vector2[] endPositions = new Vector2[nextCells.Count];
        for (int i = 0; i < nextCells.Count; i++)
        {
            startPositions[i] = visual.parts[i].GetComponent<RectTransform>().anchoredPosition;
            endPositions[i] = CellToPosition(targetCells[i]);
        }
        float elapsed = 0f;
        while (elapsed < StepDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / StepDuration);
            for (int i = 0; i < visual.bodyImages.Count; i++)
            {
                visual.parts[i].GetComponent<RectTransform>().anchoredPosition = Vector2.LerpUnclamped(startPositions[i], endPositions[i], t);
            }
            UpdateFillers(visual);
            yield return null;
        }
        snake.cells.Clear();
        snake.cells.AddRange(targetCells);
        UpdateSnakeVisual(visual, snake);
    }

    /// <summary>刷新蛇的格子位置、方向和层级。</summary>
    private void UpdateSnakeVisual(SnakeVisual visual, SnakeGameModel.SnakeData snake)
    {
        for (int i = 0; i < snake.cells.Count; i++)
        {
            RectTransform partRect = visual.parts[i].GetComponent<RectTransform>();
            partRect.anchoredPosition = CellToPosition(snake.cells[i]);
            partRect.localScale = GetPartScale();
        }
        UpdateFillers(visual);
    }

    /// <summary>刷新补间蛇身的中点位置与尺寸。</summary>
    private void UpdateFillers(SnakeVisual visual)
    {
        for (int i = 0; i < visual.fillers.Count; i++)
        {
            RectTransform previous = visual.parts[i].GetComponent<RectTransform>();
            RectTransform next = visual.parts[i + 1].GetComponent<RectTransform>();
            RectTransform filler = visual.fillers[i].GetComponent<RectTransform>();
            filler.anchoredPosition = (previous.anchoredPosition + next.anchoredPosition) * 0.5f;
            filler.localScale = GetPartScale();
        }
    }

    /// <summary>根据棋盘格尺寸和部件基础尺寸计算缩放。</summary>
    private Vector3 GetPartScale()
    {
        float size = CellSize();
        return new Vector3(size / partBaseWidth, size / partBaseHeight, 1f);
    }

    /// <summary>将棋盘坐标转换为 UI 局部坐标。</summary>
    private Vector2 CellToPosition(Vector2Int cell)
    {
        float size = CellSize();
        float x = (cell.x - (model.boardWidth - 1) * 0.5f) * size;
        float y = (cell.y - (model.boardHeight - 1) * 0.5f) * size;
        return new Vector2(x, y);
    }

    /// <summary>计算单格 UI 尺寸。</summary>
    private float CellSize()
    {
        Rect rect = boardRoot.rect;
        return Mathf.Min((rect.width - BoardPadding * 2f) / model.boardWidth, (rect.height - BoardPadding * 2f) / model.boardHeight);
    }

    /// <summary>计算箭头旋转角度。</summary>
    private float DirectionAngle(SnakeGameModel.MoveDirection direction)
    {
        switch (direction)
        {
            case SnakeGameModel.MoveDirection.Up: return 90f;
            case SnakeGameModel.MoveDirection.Down: return -90f;
            case SnakeGameModel.MoveDirection.Left: return 180f;
            default: return 0f;
        }
    }

    /// <summary>创建一个可复用的圆形 Sprite。</summary>
    private Sprite CreateCircleSprite()
    {
        Texture2D texture = new Texture2D(32, 32, TextureFormat.RGBA32, false);
        Color[] pixels = new Color[32 * 32];
        for (int y = 0; y < 32; y++)
        {
            for (int x = 0; x < 32; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), new Vector2(15.5f, 15.5f));
                pixels[y * 32 + x] = distance <= 15.5f ? Color.white : Color.clear;
            }
        }
        texture.SetPixels(pixels);
        texture.Apply();
        return Sprite.Create(texture, new Rect(0f, 0f, 32f, 32f), new Vector2(0.5f, 0.5f));
    }

    /// <summary>创建带 RectTransform 的 UI 物体。</summary>
    private GameObject CreateUIObject(string objectName, Transform parent)
    {
        GameObject result = new GameObject(objectName, typeof(RectTransform));
        result.layer = LayerMask.NameToLayer("UI");
        result.transform.SetParent(parent, false);
        return result;
    }
}
