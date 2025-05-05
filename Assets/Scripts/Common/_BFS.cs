using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 广度优先搜索算法
/// </summary>
public class _BFS
{
    public class Point
    {
        public int RowIndex;//行坐标
        public int ColIndex;//纵坐标
        public Point Father;//父节点 用来查找路径

        public Point(int row, int col)
        {
            this.RowIndex = row;
            this.ColIndex = col;
        }
        public Point(int row, int col, Point father)
        {
            this.RowIndex = row;
            this.ColIndex = col;
            this.Father = father;
        }

    }


    public int RowCount;//行总数
    public int ColCount;//列总数

    public _BFS(int rowCount, int colCount)
    {
        RowCount = rowCount;
        ColCount = colCount;
        finds = new Dictionary<string, Point>();
    }

    public Dictionary<string, Point> finds;//存储找到的点的字典（key:点的行列拼接字符串 value:搜索点）

    /// <summary>
    /// 搜索行走区域
    /// </summary>
    /// <param name="row">开始点的行坐标</param>
    /// <param name="col">开始点的列坐标</param>
    /// <param name="step">步数</param>
    /// <returns></returns>
    public List<Point> Search(int row, int col, int step)
    {
        //定义搜索集合
        List<Point> searchs = new List<Point>();
        //开始点
        Point startPoint = new Point(row, col);
        //将开始点存储到搜索集合
        searchs.Add(startPoint);
        //开始点默认开始已经找到，存储到已经找到的点的集合
        finds.Add($"{row}_{col}", startPoint);
        for (int i = 0; i < step; i++)
        {
            List<Point> temps = new List<Point>();
            //遍历搜索合集
            for (int j = 0; j < searchs.Count; j++)
            {
                Point current = searchs[j];
                //查询当前点四周的点
                FindAroundPoints(current, temps);
            }
            if (temps.Count == 0)
            {
                break;
            }
            //清空要搜索的集合
            searchs.Clear();
            //将临时集合的点添加到搜索集合中
            searchs.AddRange(temps);
        }
        return finds.Values.ToList();
    }


    public void FindAroundPoints(Point current, List<Point> temps)
    {
        //上
        if (current.RowIndex - 1 >= 0)
        {
            AddFinds(current.RowIndex - 1, current.ColIndex, current, temps);
        }
        //下
        if (current.RowIndex + 1 < RowCount)
        {
            AddFinds(current.RowIndex + 1, current.ColIndex, current, temps);
        }
        //左
        if (current.ColIndex - 1 >= 0)
        {
            AddFinds(current.RowIndex, current.ColIndex - 1, current, temps);
        }
        //右
        if (current.ColIndex + 1 < ColCount)
        {
            AddFinds(current.RowIndex, current.ColIndex + 1, current, temps);
        }
    }

    public void AddFinds(int row, int col, Point father, List<Point> temps)
    {
        //不在查找的节点 且 对应地图格子不是障碍物 才能加入 查找字典
        if (finds.ContainsKey($"{row}_{col}") == false && GameApp.MapManager.GetBlockType(row, col) != BlockType.obstacle)
        {
            Point p = new Point(row, col, father);
            //添加到查询到的字典
            finds.Add($"{row}_{col}", p);
            //添加临时集合用于下次查找
            temps.Add(p);
        }
    }

    //寻找可移动的点 离终点最近的点的路径集合
    public List<Point> FindMinPath(ModelBase model, int step, int endRowIndex, int endColIndex)
    {
        List<Point> results = Search(model.RowIndex, model.ColIndex, step);
        if (results.Count == 0)
        {
            return null;
        }
        else
        {
            Point minPoint = results[0];
            int min_dis = Mathf.Abs(minPoint.RowIndex- endRowIndex)+Mathf.Abs(minPoint.ColIndex-endColIndex);
            for(int i = 1;i<results.Count;i++)
            {
                int temp_dis = Mathf.Abs(results[i].RowIndex - endRowIndex) + Mathf.Abs(results[i].ColIndex - endColIndex);
                if (temp_dis < min_dis)
                {
                    min_dis = temp_dis;
                    minPoint = results[i];
                }
            }
            List<Point> path = new List<Point>();
            Point current = minPoint.Father;
            path.Add(minPoint);
            while (current != null)
            {
                path.Add(current);
                current = current.Father;
            }
            path.Reverse();//反转得到路径
            return path;
        }
    }
}
