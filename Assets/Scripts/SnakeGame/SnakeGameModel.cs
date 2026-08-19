using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 蛇玩法的纯数据模型，保存棋盘、蛇、生命与移动快照。
/// </summary>
public class SnakeGameModel
{
    /// <summary>蛇的图片类型。</summary>
    public enum SnakeType
    {
        Type0,
        Type1,
        Type2,
        Type3,
        Type4,
        Type5,
        Type6,
        Type7,
        Type8,
        Type9,
        Type10,
        Type11
    }

    /// <summary>单格方向。</summary>
    public enum MoveDirection
    {
        Up,
        Down,
        Left,
        Right,
        UpRight,
        UpLeft,
        DownRight,
        DownLeft
    }

    /// <summary>一条蛇的运行时数据。</summary>
    public class SnakeData
    {
        /// <summary>蛇的唯一编号。</summary>
        public int id;
        /// <summary>蛇的图片类型。</summary>
        public SnakeType type;
        /// <summary>从蛇头到蛇尾的棋盘坐标。</summary>
        public List<Vector2Int> cells = new List<Vector2Int>();
        /// <summary>蛇头当前朝向。</summary>
        public MoveDirection direction;
        /// <summary>蛇是否已经离场。</summary>
        public bool removed;
    }

    /// <summary>棋盘宽度。</summary>
    public readonly int boardWidth;
    /// <summary>棋盘高度。</summary>
    public readonly int boardHeight;
    /// <summary>当前关卡蛇列表。</summary>
    public readonly List<SnakeData> snakes = new List<SnakeData>();
    /// <summary>路径阻挡器数据。</summary>
    public readonly List<WayBlockerData> wayBlockers = new List<WayBlockerData>();
    /// <summary>黑洞数据。</summary>
    public readonly List<BlackHoleData> blackHoles = new List<BlackHoleData>();
    /// <summary>移动前的蛇格子快照。</summary>
    public readonly List<Vector2Int> moveSnapshot = new List<Vector2Int>();
    /// <summary>本次移动每一格的完整蛇身布局。</summary>
    public readonly List<List<Vector2Int>> moveLayouts = new List<List<Vector2Int>>();
    /// <summary>路径阻挡器运行时数据。</summary>
    public class WayBlockerData
    {
        /// <summary>阻挡器坐标。</summary>
        public Vector2Int position;
        /// <summary>剩余需要消除的蛇数量。</summary>
        public int remainingSnakeCount;
        /// <summary>阻挡器是否仍然有效。</summary>
        public bool active = true;
    }

    /// <summary>黑洞运行时数据。</summary>
    public class BlackHoleData
    {
        /// <summary>黑洞坐标。</summary>
        public Vector2Int position;
    }

    /// <summary>创建指定尺寸的棋盘模型。</summary>
    public SnakeGameModel(int width, int height)
    {
        boardWidth = width;
        boardHeight = height;
    }

    /// <summary>判断坐标是否位于棋盘内。</summary>
    public bool IsInside(Vector2Int cell)
    {
        return cell.x >= 0 && cell.x < boardWidth && cell.y >= 0 && cell.y < boardHeight;
    }

    /// <summary>返回方向对应的坐标增量。</summary>
    public static Vector2Int DirectionOffset(MoveDirection direction)
    {
        switch (direction)
        {
            case MoveDirection.Up: return Vector2Int.up;
            case MoveDirection.Down: return Vector2Int.down;
            case MoveDirection.Left: return Vector2Int.left;
            case MoveDirection.UpRight: return new Vector2Int(1, 1);
            case MoveDirection.UpLeft: return new Vector2Int(-1, 1);
            case MoveDirection.DownRight: return new Vector2Int(1, -1);
            case MoveDirection.DownLeft: return new Vector2Int(-1, -1);
            default: return Vector2Int.right;
        }
    }

    /// <summary>保存选中蛇的移动前状态。</summary>
    public void SaveSnapshot(SnakeData snake)
    {
        moveSnapshot.Clear();
        moveSnapshot.AddRange(snake.cells);
        moveLayouts.Clear();
        moveLayouts.Add(new List<Vector2Int>(snake.cells));
    }
}
