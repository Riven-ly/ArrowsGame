using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Resources/Level 文本配置的根结构。
/// </summary>
[Serializable]
public class SnakeLevelConfig
{
    /// <summary>棋盘尺寸。</summary>
    public SnakeLevelSize size;
    /// <summary>关卡时间限制。</summary>
    public int timeLimit;
    /// <summary>关卡名称。</summary>
    public string name;
    /// <summary>配置中的蛇路径列表。</summary>
    public List<SnakeArrowConfig> arrows;
    /// <summary>路径阻挡器列表。</summary>
    public List<SnakeWayBlockerConfig> wayBlockers;
    /// <summary>黑洞列表。</summary>
    public List<SnakeBlackHoleConfig> blackHoles;
}

/// <summary>
/// 关卡棋盘尺寸。
/// </summary>
[Serializable]
public class SnakeLevelSize
{
    /// <summary>棋盘宽度。</summary>
    public int x;
    /// <summary>棋盘高度。</summary>
    public int y;
}

/// <summary>
/// 配置中的一条蛇。
/// </summary>
[Serializable]
public class SnakeArrowConfig
{
    /// <summary>蛇路径节点。</summary>
    public List<SnakeLevelNode> nodes;
    /// <summary>配置中的颜色名称。</summary>
    public string color;
}

/// <summary>
/// 路径阻挡器配置。
/// </summary>
[Serializable]
public class SnakeWayBlockerConfig
{
    /// <summary>阻挡器坐标。</summary>
    public SnakeLevelNode position;
    /// <summary>阻挡器锁定时间。</summary>
    public float lockTime;
    /// <summary>阻挡器尺寸字符串。</summary>
    public string size;
}

/// <summary>
/// 黑洞配置。
/// </summary>
[Serializable]
public class SnakeBlackHoleConfig
{
    /// <summary>黑洞坐标。</summary>
    public SnakeLevelNode position;
    /// <summary>黑洞尺寸字符串。</summary>
    public string size;
}

/// <summary>
/// 配置路径坐标节点。
/// </summary>
[Serializable]
public class SnakeLevelNode
{
    /// <summary>横坐标。</summary>
    public int x;
    /// <summary>纵坐标。</summary>
    public int y;

    /// <summary>转换为棋盘坐标。</summary>
    public Vector2Int ToCell()
    {
        return new Vector2Int(x, y);
    }
}
