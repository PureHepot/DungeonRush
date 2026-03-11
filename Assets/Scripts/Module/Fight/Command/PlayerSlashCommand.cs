using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlashCommand : BaseCommand
{
    private int damage = 3; // 斩击的伤害值

    // 用来存放从 PlayerController 传过来的特效预制体
    private GameObject slashVfxPrefab;

    // 【关键修复】：修改构造函数，接收 model 和 vfxPrefab 两个参数！
    public PlayerSlashCommand(ModelBase model, GameObject vfxPrefab) : base(model)
    {
        this.slashVfxPrefab = vfxPrefab;
    }

    public override void Do()
    {
        base.Do();


        // 指令真正落地执行时，才扣除能量并刷新 UI
        if (GameApp.PlayerManager.slashEnergy >= 30)
        {
            GameApp.PlayerManager.slashEnergy -= 30;
            // 通知 UI 界面往下掉一截颜色
            GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, "OnSlashEnergyChange", GameApp.PlayerManager.slashEnergy);
        }

        // 播放玩家攻击动作
        model.PlayAni("Attack");

       
        GameApp.SoundManager.PlayEffect("slash", model.transform.position);

        // 确定玩家朝向 (1 为右，-1 为左)
        int dir = model.transform.localScale.x > 0 ? 1 : -1;

        if (slashVfxPrefab != null)
        {
            // 获取玩家正前方第 1 格的坐标
            Vector3 vfxPos = GameApp.MapManager.GetBlockPos(model.RowIndex, model.ColIndex + dir);

            // 生成这唯一的一个特效
            GameObject vfxInstance = Object.Instantiate(slashVfxPrefab, vfxPos, Quaternion.identity);

            // 处理翻转：
            
            vfxInstance.transform.localScale = new Vector3(-dir, 1, 1);
        }
        //计算3*3格子
        int centerRow = model.RowIndex;

        
        int startCol = model.ColIndex;
        int endCol = model.ColIndex + (dir * 2);

        // 记录砍中的敌人，防止同一个敌人同一刀被扣多次血
        List<Enemy> hitEnemies = new List<Enemy>();

        // 遍历这 9 个格子 (3行 x 3列)
        for (int c = Mathf.Min(startCol, endCol); c <= Mathf.Max(startCol, endCol); c++)
        {
            for (int r = centerRow - 1; r <= centerRow + 1; r++)
            {
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

        // 对所有被砍中的敌人统一造成伤害
        foreach (var enemy in hitEnemies)
        {
            enemy.EnemyBeAttacked(damage);
        }

        Debug.Log($"【斩击】释放成功！击中了 {hitEnemies.Count} 个敌人！");
    }

    public override bool Update(float dt)
    {
        return true;
    }
}
