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

    /// <summary>初始化首版示例关卡。</summary>
    public void Initialize()
    {
        if (view == null)
        {
            view = GetComponent<SnakeGameView>();
        }
        model = CreateExampleLevel();
        isMoving = false;
        inputEnabled = true;
        movingSnakeIds.Clear();
        view.Build(model, OnSnakeClicked);
    }

    /// <summary>停止继续接受玩家输入。</summary>
    public void StopInput()
    {
        inputEnabled = false;
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
            yield return view.PlayExit(snake, layouts);
            snake.removed = true;
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

    /// <summary>判断坐标是否被其他尚未消除的蛇占据。</summary>
    private bool IsOccupiedByOtherSnake(Vector2Int cell, SnakeGameModel.SnakeData movingSnake)
    {
        if (!model.IsInside(cell))
        {
            return false;
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

    /// <summary>创建 9×13 棋盘上的十二条示例蛇。</summary>
    private SnakeGameModel CreateExampleLevel()
    {
        SnakeGameModel result = new SnakeGameModel(9, 13);
        AddSnake(result, 0, SnakeGameModel.SnakeType.Type0, new Vector2Int(2, 11), new Vector2Int(1, 11), new Vector2Int(0, 11), new Vector2Int(0, 10));
        AddSnake(result, 1, SnakeGameModel.SnakeType.Type1, new Vector2Int(5, 11), new Vector2Int(4, 11), new Vector2Int(3, 11), new Vector2Int(3, 10));
        AddSnake(result, 2, SnakeGameModel.SnakeType.Type2, new Vector2Int(0, 8), new Vector2Int(0, 7), new Vector2Int(1, 7), new Vector2Int(1, 6));
        AddSnake(result, 3, SnakeGameModel.SnakeType.Type3, new Vector2Int(2, 7), new Vector2Int(2, 6), new Vector2Int(3, 6), new Vector2Int(3, 5));
        AddSnake(result, 4, SnakeGameModel.SnakeType.Type4, new Vector2Int(4, 8), new Vector2Int(3, 8), new Vector2Int(3, 9), new Vector2Int(2, 9));
        AddSnake(result, 5, SnakeGameModel.SnakeType.Type5, new Vector2Int(6, 8), new Vector2Int(6, 9), new Vector2Int(5, 9), new Vector2Int(5, 10));
        AddSnake(result, 6, SnakeGameModel.SnakeType.Type6, new Vector2Int(8, 7), new Vector2Int(7, 7), new Vector2Int(7, 8), new Vector2Int(8, 8));
        AddSnake(result, 7, SnakeGameModel.SnakeType.Type7, new Vector2Int(1, 3), new Vector2Int(1, 4), new Vector2Int(2, 4), new Vector2Int(2, 5));
        AddSnake(result, 8, SnakeGameModel.SnakeType.Type8, new Vector2Int(4, 3), new Vector2Int(3, 3), new Vector2Int(2, 3), new Vector2Int(2, 2));
        AddSnake(result, 9, SnakeGameModel.SnakeType.Type9, new Vector2Int(7, 3), new Vector2Int(7, 4), new Vector2Int(6, 4), new Vector2Int(6, 5));
        AddSnake(result, 10, SnakeGameModel.SnakeType.Type10, new Vector2Int(8, 1), new Vector2Int(7, 1), new Vector2Int(7, 2), new Vector2Int(6, 2));
        AddSnake(result, 11, SnakeGameModel.SnakeType.Type11, new Vector2Int(3, 0), new Vector2Int(3, 1), new Vector2Int(4, 1), new Vector2Int(4, 2));
        return result;
    }

    /// <summary>根据蛇头和第二节身体计算蛇头朝向。</summary>
    private SnakeGameModel.MoveDirection GetHeadDirection(Vector2Int[] cells)
    {
        Vector2Int offset = cells[0] - cells[1];
        if (offset.x > 0) return SnakeGameModel.MoveDirection.Right;
        if (offset.x < 0) return SnakeGameModel.MoveDirection.Left;
        if (offset.y > 0) return SnakeGameModel.MoveDirection.Up;
        return SnakeGameModel.MoveDirection.Down;
    }

    /// <summary>向关卡模型添加一条蛇。</summary>
    private void AddSnake(SnakeGameModel target, int id, SnakeGameModel.SnakeType type, params Vector2Int[] cells)
    {
        SnakeGameModel.SnakeData snake = new SnakeGameModel.SnakeData
        {
            id = id,
            type = type,
            direction = GetHeadDirection(cells)
        };
        snake.cells.AddRange(cells);
        target.snakes.Add(snake);
    }
}
