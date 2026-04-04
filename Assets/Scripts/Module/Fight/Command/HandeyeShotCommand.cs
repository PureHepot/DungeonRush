using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandeyeShotCommand : BaseCommand
{
    
    class Point
    {
        public Point(int row, int col)
        {
            this.row = row; this.col = col;
        }

        public int row;
        public int col;
    }

    private List<Point> actualAttackPoints = new List<Point>();

    public List<int> GetRandomNumbers(int n)
    {
        List<int> numbers = new List<int>();
        HashSet<int> uniqueNumbers = new HashSet<int>(); // 用于确保每个数字唯一

        // 继续抽取，直到得到 n 个不同的数
        while (uniqueNumbers.Count < n)
        {
            int number = Random.Range(0, 5); // 生成1到5之间的随机数
            uniqueNumbers.Add(number); // HashSet 会自动避免重复
        }

        // 将唯一的数字添加到列表中
        numbers.AddRange(uniqueNumbers);
        return numbers;
    }

    public HandeyeShotCommand(ModelBase model) : base(model)
    {
        this.model = model;
    }

    public override void Do()
    {
        List<Point> points = new List<Point>
        {
            new Point(GameApp.PlayerManager.playerRow, GameApp.PlayerManager.playerCol),
            new Point(GameApp.PlayerManager.playerRow - 1, GameApp.PlayerManager.playerCol),
            new Point(GameApp.PlayerManager.playerRow, GameApp.PlayerManager.playerCol + 1),
            new Point(GameApp.PlayerManager.playerRow + 1, GameApp.PlayerManager.playerCol),
            new Point(GameApp.PlayerManager.playerRow, GameApp.PlayerManager.playerCol - 1)
        };

        int count = Random.Range(1, 4);
        List<int> nums = GetRandomNumbers(count);

        foreach(int i in nums)
        {

            Point p = points[i];

            // 【新增防错 1】：防止玩家站在地图边缘时，计算出的攻击点超出数组边界引发报错
            if (p.row < 0 || p.row >= GameApp.MapManager.TotalRowCount ||
                p.col < 0 || p.col >= GameApp.MapManager.TotalColCount)
            {
                continue; // 超出地图边界，跳过该点
            }
            // ==========================================

            Block b = GameApp.MapManager.GetBlockByPos(p.row, p.col);

            if (b != null)
            {
                // ==========================================
                // 【核心修复 2】：如果该地块是墙体（障碍物）或空地块，则不生成攻击判定！
                if (b.Type == BlockType.obstacle || b.originType == BlockType.obstacle || b.Type == BlockType.empty)
                {
                    continue; // 是墙体，跳过该点，不让其变红
                }

                

          
                b.isdamage = true;
             
                b.ShowGrid(Color.red);
            }
                
            
        }
    }

    public override bool Update(float dt)
    {
        

        return true; // 指令结束
    }
}
