using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlashCommand : BaseCommand
{
    private int damage = 1; // 斩击伤害值，你可以随便填

    public PlayerSlashCommand(ModelBase model) : base(model)
    {
    }

    public override void Do()
    {
        base.Do();

        // 1. 播放玩家的攻击动画和特殊音效
        model.PlayAni("Attack"); // 如果你有专门的斩击动画，换成比如 "Slash"
        GameApp.SoundManager.PlayEffect("slash", model.transform.position);

        // 2. 确定玩家朝向 (1 为右，-1 为左)
        int dir = model.transform.localScale.x > 0 ? 1 : -1;

        // 3. 计算 2x3 范围
        int centerRow = model.RowIndex;
        // 前方第 1 格和第 2 格的列索引
        int startCol = model.ColIndex + (dir * 1);
        int endCol = model.ColIndex + (dir * 2);

        // 用来记录砍中的敌人，防止同一个敌人被扣多次血
        List<Enemy> hitEnemies = new List<Enemy>();

        // 遍历这 2x3 个格子
        for (int c = Mathf.Min(startCol, endCol); c <= Mathf.Max(startCol, endCol); c++)
        {
            for (int r = centerRow - 1; r <= centerRow + 1; r++)
            {
                // 【可选】在这里生成一个酷炫的刀光粒子特效预制体
                // Vector3 pos = GameApp.MapManager.GetBlockPos(r, c);
                // Object.Instantiate(Resources.Load("Prefabs/Effects/SlashVFX"), pos, Quaternion.identity);

                // 检查这个格子上是否有活着的敌人
                foreach (var enemy in GameApp.EnemyManager.enemies)
                {
                    if (enemy.RowIndex == r && enemy.ColIndex == c && enemy.CurHp > 0)
                    {
                        if (!hitEnemies.Contains(enemy))
                        {
                            hitEnemies.Add(enemy);
                        }
                    }
                }
            }
        }

        // 4. 对所有被砍中的敌人统一造成伤害
        foreach (var enemy in hitEnemies)
        {
            enemy.EnemyBeAttacked(damage);
        }

        Debug.Log($"【斩击】释放成功！击中了 {hitEnemies.Count} 个敌人！");
    }

    public override bool Update(float dt)
    {
        // 瞬间完成的指令
        return true;
    }
}
