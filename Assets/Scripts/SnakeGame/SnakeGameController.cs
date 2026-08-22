using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 蛇玩法控制器，负责输入、移动、占格、碰撞检测和动画流程。
/// </summary>
public class SnakeGameController : MonoBehaviour
{
    /// <summary>蛇玩法视图。</summary>
    [SerializeField] private SnakeGameView view;
    /// <summary>当前玩法数据模型。</summary>
    private SnakeGameModel model;
    /// <summary>是否正在处理蛇的移动。</summary>
    private bool isMoving;
    /// <summary>是否允许继续接受玩家输入。</summary>
    private bool inputEnabled;
    /// <summary>当前并行动画中的蛇编号。</summary>
    private readonly HashSet<int> movingSnakeIds = new HashSet<int>();
    /// <summary>通知面板发生碰撞。</summary>
    public Action collisionEvent;
    /// <summary>通知面板全部蛇已经离场。</summary>
    public Action victoryEvent;
    /// <summary>通知面板刷新蛇数量。</summary>
    public Action snakeCountChangedEvent;
    /// <summary>通知面板一条蛇成功完成移动。</summary>
    public Action snakeMoveSuccessEvent;

    /// <summary>初始化首版示例关卡。</summary>
    public void Initialize()
    {
        StopAllCoroutines();
        if (view == null)
        {
            view = GetComponent<SnakeGameView>();
        }
        model = CreateLevelFromPlayerLevel();
        isMoving = false;
        inputEnabled = true;
        movingSnakeIds.Clear();
        view.Build(model, OnSnakeClicked);
    }

    /// <summary>获取当前未离场蛇数量。</summary>
    public int GetRemainingSnakeCount()
    {
        int count = 0;
        for (int i = 0; i < model.snakes.Count; i++)
        {
            if (!model.snakes[i].removed) count++;
        }
        return count;
    }

    /// <summary>获取当前关卡蛇总数。</summary>
    public int GetTotalSnakeCount()
    {
        return model.snakes.Count;
    }

    /// <summary>停止继续接受玩家输入。</summary>
    public void StopInput()
    {
        inputEnabled = false;
    }

    /// <summary>恢复玩家输入。</summary>
    public void ResumeInput()
    {
        inputEnabled = true;
    }

    /// <summary>自动点击一条当前未移动且不会撞到阻挡的蛇。</summary>
    public bool TryAutoClickSafeSnake()
    {
        if (!inputEnabled)
        {
            return false;
        }
        for (int i = 0; i < model.snakes.Count; i++)
        {
            SnakeGameModel.SnakeData snake = model.snakes[i];
            if (snake.removed || movingSnakeIds.Contains(snake.id))
            {
                continue;
            }
            List<List<Vector2Int>> layouts;
            if (!BuildMoveTrack(snake, out layouts))
            {
                OnSnakeClicked(snake.id);
                return true;
            }
        }
        return false;
    }

    /// <summary>显示所有当前可安全移动蛇头的提示线。</summary>
    public bool TryShowSafeSnakeHints(float duration)
    {
        if (!inputEnabled)
        {
            return false;
        }
        List<int> safeSnakeIds = new List<int>();
        for (int i = 0; i < model.snakes.Count; i++)
        {
            SnakeGameModel.SnakeData snake = model.snakes[i];
            if (snake.removed || movingSnakeIds.Contains(snake.id))
            {
                continue;
            }
            List<List<Vector2Int>> layouts;
            if (!BuildMoveTrack(snake, out layouts))
            {
                safeSnakeIds.Add(snake.id);
            }
        }
        if (safeSnakeIds.Count == 0)
        {
            return false;
        }
        view.ShowSnakeHints(safeSnakeIds, duration);
        return true;
    }

    /// <summary>重新开始首版示例关卡。</summary>
    public void ResetGame()
    {
        StopAllCoroutines();
        Initialize();
    }

    /// <summary>响应玩家点击蛇身。</summary>
    private void OnSnakeClicked(int snakeId)
    {
        if (!inputEnabled)
        {
            return;
        }
        SnakeGameModel.SnakeData snake = FindSnake(snakeId);
        if (snake == null || snake.removed || movingSnakeIds.Contains(snake.id))
        {
            return;
        }
        movingSnakeIds.Add(snake.id);
        List<Vector2Int> snapshot = new List<Vector2Int>(snake.cells);
        List<List<Vector2Int>> layouts;
        bool collided = BuildMoveTrack(snake, out layouts);
        StartCoroutine(MoveSnake(snake, snapshot, layouts, collided));
    }

    /// <summary>处理单条蛇的独立移动流程。</summary>
    private IEnumerator MoveSnake(SnakeGameModel.SnakeData snake, List<Vector2Int> snapshot, List<List<Vector2Int>> layouts, bool collided)
    {
        isMoving = true;
        if (collided)
        {
            yield return view.PlayForward(snake, layouts);
            collisionEvent?.Invoke();
            yield return view.PlayBackward(snake, layouts);
            snake.cells.Clear();
            snake.cells.AddRange(snapshot);
        }
        else
        {
            if (SettingPanel.IsVibrateEnabled)
            {
                Handheld.Vibrate();
            }
            yield return view.PlayExit(snake, layouts);
            snake.removed = true;
            view.NotifySnakeRemoved();
            snakeCountChangedEvent?.Invoke();
            snakeMoveSuccessEvent?.Invoke();
            if (AllSnakesRemoved())
            {
                inputEnabled = false;
                victoryEvent?.Invoke();
            }
        }
        movingSnakeIds.Remove(snake.id);
        isMoving = movingSnakeIds.Count > 0;
    }

    /// <summary>预演单条蛇的独立轨迹并检测静止蛇占格。</summary>
    private bool BuildMoveTrack(SnakeGameModel.SnakeData snake, out List<List<Vector2Int>> layouts)
    {
        layouts = new List<List<Vector2Int>>();
        layouts.Add(new List<Vector2Int>(snake.cells));
        List<Vector2Int> plannedCells = new List<Vector2Int>(snake.cells);
        Vector2Int offset = SnakeGameModel.DirectionOffset(snake.direction);
        while (HasInsideCell(plannedCells))
        {
            Vector2Int nextHead = plannedCells[0] + offset;
            if (IsBlackHole(nextHead))
            {
                for (int i = plannedCells.Count - 1; i > 0; i--) plannedCells[i] = plannedCells[i - 1];
                plannedCells[0] = nextHead;
                layouts.Add(new List<Vector2Int>(plannedCells));
                return false;
            }
            if (IsOccupiedByOtherSnake(nextHead, snake)) return true;
            for (int i = plannedCells.Count - 1; i > 0; i--) plannedCells[i] = plannedCells[i - 1];
            plannedCells[0] = nextHead;
            layouts.Add(new List<Vector2Int>(plannedCells));
        }
        return false;
    }

    /// <summary>判断一组计划格子是否还有棋盘内部分。</summary>
    private bool HasInsideCell(System.Collections.Generic.List<Vector2Int> cells)
    {
        for (int i = 0; i < cells.Count; i++)
        {
            if (model.IsInside(cells[i]))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>处理路径阻挡器到期。</summary>
    public void OnWayBlockerExpired(Vector2Int position)
    {
        for (int i = 0; i < model.wayBlockers.Count; i++)
        {
            if (model.wayBlockers[i].position == position)
            {
                model.wayBlockers[i].active = false;
                view.HideWayBlocker(position);
                return;
            }
        }
    }

    /// <summary>判断坐标是否是黑洞。</summary>
    private bool IsBlackHole(Vector2Int cell)
    {
        for (int i = 0; i < model.blackHoles.Count; i++)
        {
            if (model.blackHoles[i].position == cell) return true;
        }
        return false;
    }

    /// <summary>判断坐标是否被其他尚未消除的蛇占据。</summary>
    private bool IsOccupiedByOtherSnake(Vector2Int cell, SnakeGameModel.SnakeData movingSnake)
    {
        if (!model.IsInside(cell))
        {
            return false;
        }
        for (int i = 0; i < model.wayBlockers.Count; i++)
        {
            if (model.wayBlockers[i].active && model.wayBlockers[i].position == cell) return true;
        }
        for (int i = 0; i < model.snakes.Count; i++)
        {
            SnakeGameModel.SnakeData otherSnake = model.snakes[i];
            if (otherSnake == movingSnake || otherSnake.removed || movingSnakeIds.Contains(otherSnake.id))
            {
                continue;
            }
            for (int j = 0; j < otherSnake.cells.Count; j++)
            {
                if (otherSnake.cells[j] == cell)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>按编号查找蛇数据。</summary>
    private SnakeGameModel.SnakeData FindSnake(int snakeId)
    {
        for (int i = 0; i < model.snakes.Count; i++)
        {
            if (model.snakes[i].id == snakeId)
            {
                return model.snakes[i];
            }
        }
        return null;
    }

    /// <summary>判断是否所有蛇都已经离场。</summary>
    private bool AllSnakesRemoved()
    {
        for (int i = 0; i < model.snakes.Count; i++)
        {
            if (!model.snakes[i].removed)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>根据玩家等级加载关卡配置。</summary>
    private SnakeGameModel CreateLevelFromPlayerLevel()
    {
        int level = GameManager.Instance.playerInfo.level;
        if (level > 1000)
        {
            level = UnityEngine.Random.Range(501, 1001);
        }
        TextAsset levelAsset = Resources.Load<TextAsset>("Level/lv" + level);
        SnakeLevelConfig config = JsonUtility.FromJson<SnakeLevelConfig>(levelAsset.text);
        SnakeGameModel result = new SnakeGameModel(config.size.x, config.size.y);
        if (config.wayBlockers == null) config.wayBlockers = new List<SnakeWayBlockerConfig>();
        if (config.blackHoles == null) config.blackHoles = new List<SnakeBlackHoleConfig>();
        for (int i = 0; i < config.wayBlockers.Count; i++)
        {
            result.wayBlockers.Add(new SnakeGameModel.WayBlockerData
            {
                position = config.wayBlockers[i].position.ToCell(),
                remainingSnakeCount = Mathf.RoundToInt(config.wayBlockers[i].lockTime)
            });
        }
        for (int i = 0; i < config.blackHoles.Count; i++)
        {
            result.blackHoles.Add(new SnakeGameModel.BlackHoleData
            {
                position = config.blackHoles[i].position.ToCell()
            });
        }
        for (int i = 0; i < config.arrows.Count; i++)
        {
            List<Vector2Int> cells = ExpandNodes(config.arrows[i].nodes);
            cells.Reverse();
            SnakeGameModel.SnakeData snake = new SnakeGameModel.SnakeData
            {
                id = i,
                type = GetSnakeType(config.arrows[i].color),
                direction = GetHeadDirection(cells.ToArray())
            };
            snake.cells.AddRange(cells);
            result.snakes.Add(snake);
        }
        return result;
    }

    /// <summary>将配置关键点展开为连续棋盘格。</summary>
    private List<Vector2Int> ExpandNodes(List<SnakeLevelNode> nodes)
    {
        List<Vector2Int> cells = new List<Vector2Int>();
        cells.Add(nodes[0].ToCell());
        for (int i = 1; i < nodes.Count; i++)
        {
            Vector2Int current = cells[cells.Count - 1];
            Vector2Int target = nodes[i].ToCell();
            while (current != target)
            {
                current += new Vector2Int(
                    Mathf.Clamp(target.x - current.x, -1, 1),
                    Mathf.Clamp(target.y - current.y, -1, 1));
                cells.Add(current);
            }
        }
        return cells;
    }

    /// <summary>将配置颜色名称映射到蛇图片类型。</summary>
    private SnakeGameModel.SnakeType GetSnakeType(string color)
    {
        switch (color)
        {
            case "None":
            case "Orange": return SnakeGameModel.SnakeType.Type0;
            case "Pink":
            case "JPPink": return SnakeGameModel.SnakeType.Type1;
            case "Red":
            case "Lipstick_darkRed":
            case "Lipstick_red": return SnakeGameModel.SnakeType.Type2;
            case "Yellow":
            case "JPYellow": return SnakeGameModel.SnakeType.Type3;
            case "Blue":
            case "Lipstick_naviDark":
            case "JPBlue": return SnakeGameModel.SnakeType.Type4;
            case "Green":
            case "JPGreen": return SnakeGameModel.SnakeType.Type5;
            case "Lipstick_lightRed": return SnakeGameModel.SnakeType.Type6;
            case "Lipstick_lightYellow": return SnakeGameModel.SnakeType.Type7;
            case "Lipstick_navilight":
            case "Lipstick_navi":
            case "Lipstick_navipale":
            case "Gray": return SnakeGameModel.SnakeType.Type8;
            case "Lime": return SnakeGameModel.SnakeType.Type9;
            case "DarkViolet": return SnakeGameModel.SnakeType.Type10;
            case "Cyan":
            case "JPCyan": return SnakeGameModel.SnakeType.Type11;
            case "Purple":
            case "JPViolet": return SnakeGameModel.SnakeType.Type12;
            case "Black":
            case "Brown":
            case "JPBlack": return SnakeGameModel.SnakeType.Type13;
            case "DarkGreen":
            case "JPDarkGreen": return SnakeGameModel.SnakeType.Type5;
            case "White": return SnakeGameModel.SnakeType.Type7;
        }
        return SnakeGameModel.SnakeType.Type0;
    }

    /// <summary>根据蛇头和第二节身体计算蛇头朝向。</summary>
    private SnakeGameModel.MoveDirection GetHeadDirection(Vector2Int[] cells)
    {
        Vector2Int offset = cells[0] - cells[1];
        int x = Mathf.Clamp(offset.x, -1, 1);
        int y = Mathf.Clamp(offset.y, -1, 1);
        if (x > 0 && y > 0) return SnakeGameModel.MoveDirection.UpRight;
        if (x < 0 && y > 0) return SnakeGameModel.MoveDirection.UpLeft;
        if (x > 0 && y < 0) return SnakeGameModel.MoveDirection.DownRight;
        if (x < 0 && y < 0) return SnakeGameModel.MoveDirection.DownLeft;
        if (x > 0) return SnakeGameModel.MoveDirection.Right;
        if (x < 0) return SnakeGameModel.MoveDirection.Left;
        if (y > 0) return SnakeGameModel.MoveDirection.Up;
        return SnakeGameModel.MoveDirection.Down;
    }
}
