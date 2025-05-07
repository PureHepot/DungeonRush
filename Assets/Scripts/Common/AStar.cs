using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 路径点
/// </summary>
public class AStarPoint
{
    public int RowIndex;
    public int ColIndex;
    public int G;//当前节点到开始点的距离
    public int H;//当前节点到终点的距离
    public int F;//F = G + H
    public AStarPoint Parent;//父节点

    public AStarPoint(int row, int col)
    {
        this.RowIndex = row;
        this.ColIndex = col;
        this.Parent = null;
    }
    public AStarPoint(int row, int col, AStarPoint parent)
    {
        this.RowIndex = row;
        this.ColIndex = col;
        this.Parent = parent;
    }

    public int GetG()
    {
        int _g = 0;
        AStarPoint parent = this.Parent;
        while (parent != null)
        {
            _g += 1;
            parent = parent.Parent;
        }
        return _g;
    }

    public int GetH(AStarPoint end)
    {
        return Mathf.Abs(RowIndex - end.RowIndex) + Mathf.Abs(ColIndex - end.ColIndex);
    }
}

public class AStar
{
    private int rowCount;
    private int colCount;
    private List<AStarPoint> open;//open表
    private Dictionary<string, AStarPoint> close;//close表 已经查找过的路径点会存在这个表里
    private AStarPoint start;//起点
    private AStarPoint end;//终点

    public AStar(int rowCount, int colCount)
    {
        this.rowCount = rowCount;
        this.colCount = colCount;
        open = new List<AStarPoint>();
        close = new Dictionary<string, AStarPoint>();
    }

    public AStarPoint IsInOpen(int rowIndex, int colIndex)
    {
        for (int i = 0; i < open.Count; i++)
        {
            if (open[i].RowIndex == rowIndex && open[i].ColIndex == colIndex)
            {
                return open[i];
            }
        }
        return null;
    }

    public bool IsInClose(int rowIndex, int colIndex)
    {
        if (close.ContainsKey($"{rowIndex}_{colIndex}"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //1.将七点加入到open中
    //2.查找open表中 f值最小的路径
    //3.将找到的最小f值的点从open中移除，并添加到close中
    //4.将当前的路径点周围的点添加到open表（上下左右的点）
    //5.判断重点是否在open表中，如果不在就从步骤2继续开始执行
    public bool FindPath(AStarPoint start, AStarPoint end, System.Action<List<AStarPoint>> findCallback)
    {
        this.start = start;
        this.end = end;
        open = new List<AStarPoint>();
        close = new Dictionary<string, AStarPoint>();

        //加入起点
        open.Add(start);
        while (true)
        {
            //执行第2步
            AStarPoint current = GetMinFFromInOpen();
            if (current == null)
                return false;
            else
            {
                //3.1 从open中移除
                open.Remove(current);
                //3.2 添加到close表中
                close.Add($"{current.RowIndex}_{current.ColIndex}", current);
                //第4步
                AddAroundToOpen(current);
                //判断终点是否在open表中
                AStarPoint endPoint = IsInOpen(end.RowIndex, end.ColIndex);
                if (endPoint != null)
                {
                    findCallback(GetPath(endPoint));
                    return true;
                }
                //排序open表 根据F的大小顺序
                open.Sort(OpenSort);
            }
        }
    }

    /// <summary>
    /// 从末位置找到完整的路径
    /// </summary>
    /// <param name="point">末位置，需要有父亲点信息</param>
    /// <returns></returns>
    public List<AStarPoint> GetPath(AStarPoint point)
    {
        List<AStarPoint> path = new List<AStarPoint>();
        path.Add(point);
        AStarPoint parent = point.Parent;
        while (parent != null)
        {
            path.Add(parent);
            parent = parent.Parent;
        }

        path.Reverse();
        return path;
    }

    public int OpenSort(AStarPoint a, AStarPoint b)
    {
        return a.F - b.F;
    }

    public void AddAroundToOpen(AStarPoint current)
    {
        if (current.RowIndex - 1 >= 0)
        {
            AddOpen(current, current.RowIndex - 1, current.ColIndex);
        }
        if (current.RowIndex + 1 < rowCount)
        {
            AddOpen(current, current.RowIndex + 1, current.ColIndex);
        }
        if (current.ColIndex - 1 >= 0)
        {
            AddOpen(current, current.RowIndex, current.ColIndex - 1);
        }
        if (current.ColIndex + 1 < colCount)
        {
            AddOpen(current, current.RowIndex, current.ColIndex + 1);
        }
    }

    public void AddOpen(AStarPoint current, int row, int col)
    {
        //不在open和close 并且不能是障碍物才能加入open
        if (IsInClose(row, col) == false && IsInOpen(row, col) == null)//&& GameApp.MapManager.GetBlockType(row, col) != BlockType.obstacle
        {
            AStarPoint newPoint = new AStarPoint(row, col, current);
            newPoint.G = newPoint.GetG();
            newPoint.H = newPoint.GetH(end);
            newPoint.F = newPoint.G + newPoint.H;
            open.Add(newPoint);
        }
    }

    public AStarPoint GetMinFFromInOpen()
    {
        if(open.Count == 0)
            return null;
        return open[0];//优先队列
    }
}
