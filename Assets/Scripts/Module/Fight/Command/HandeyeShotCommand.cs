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
            Block b = GameApp.MapManager.GetBlockByPos(points[i].row, points[i].col);
            b.isdamage = true;
            b.ShowGrid(Color.red);
        }
    }

    public override bool Update(float dt)
    {
        return true;
    }
}
