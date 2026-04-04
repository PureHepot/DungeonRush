using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlashCommand : BaseCommand
{
    private int damage = 3; // 斩击的伤害值
    private GameObject slashVfxPrefab;

    // 定义四个攻击方向
    private enum SlashDirection { Up, Down, Left, Right }

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
            GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, "OnSlashEnergyChange", GameApp.PlayerManager.slashEnergy);
        }

        // 播放玩家攻击动作和音效
        model.PlayAni("Attack");
        GameApp.SoundManager.PlayEffect("slash", model.transform.position);


        //  获取鼠标在世界坐标系中的位置，并与玩家位置比对
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 playerPos = model.transform.position;

        float dx = mouseWorldPos.x - playerPos.x;
        float dy = mouseWorldPos.y - playerPos.y;

        // 根据“X”型区域划分判定四个方向
        SlashDirection slashDir;
        if (dy > Mathf.Abs(dx))
            slashDir = SlashDirection.Up;
        else if (dy < -Mathf.Abs(dx))
            slashDir = SlashDirection.Down;
        else if (dx > Mathf.Abs(dy))
            slashDir = SlashDirection.Right;
        else
            slashDir = SlashDirection.Left;

   
        // 处理特效 (VFX) 的生成位置、旋转和翻转
        if (slashVfxPrefab != null)
        {
            Vector3 vfxPos = model.transform.position;
            Quaternion vfxRot = Quaternion.identity;
            Vector3 vfxScale = new Vector3(1, 1, 1);

            switch (slashDir)
            {
                case SlashDirection.Right:
                    vfxPos += new Vector3(1.5f, 0, 0);
                    vfxScale = new Vector3(-1, 1, 1); // 默认向左，x为-1翻转至向右
                    break;
                case SlashDirection.Left:
                    vfxPos += new Vector3(-1.5f, 0, 0);
                    vfxScale = new Vector3(1, 1, 1);  // 默认向左
                    break;
                case SlashDirection.Up:
                    vfxPos += new Vector3(0, 1.5f, 0);
                    vfxRot = Quaternion.Euler(0, 0, -90); // 旋转特效朝上 (如果你的特效素材偏了，可尝试修改为 90)
                    vfxScale = new Vector3(1, 1, 1);
                    break;
                case SlashDirection.Down:
                    vfxPos += new Vector3(0, -1.5f, 0);
                    vfxRot = Quaternion.Euler(0, 0, 90);  // 旋转特效朝下 (可尝试修改为 -90)
                    vfxScale = new Vector3(1, 1, 1);
                    break;
            }

            GameObject vfxInstance = Object.Instantiate(slashVfxPrefab, vfxPos, vfxRot);
            vfxInstance.transform.localScale = vfxScale;
        }

        
        // 计算 3x3 伤害格子的范围
        int minCol = model.ColIndex;
        int maxCol = model.ColIndex;
        int minRow = model.RowIndex;
        int maxRow = model.RowIndex;

        // 根据不同方向扩展攻击网格 (玩家自身向外延伸2格，侧边各延伸1格，形成3x3)
        switch (slashDir)
        {
            case SlashDirection.Right:
                maxCol += 2; minRow -= 1; maxRow += 1;
                break;
            case SlashDirection.Left:
                minCol -= 2; minRow -= 1; maxRow += 1;
                break;
            case SlashDirection.Up:
                maxRow += 2; minCol -= 1; maxCol += 1;
                break;
            case SlashDirection.Down:
                minRow -= 2; minCol -= 1; maxCol += 1;
                break;
        }

        // 4. 遍历伤害区域，扣除敌人血量
        List<Enemy> hitEnemies = new List<Enemy>();

        for (int c = minCol; c <= maxCol; c++)
        {
            for (int r = minRow; r <= maxRow; r++)
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

        // 对所有被砍中的敌人造成伤害
        foreach (var enemy in hitEnemies)
        {
            enemy.EnemyBeAttacked(damage);
        }
    }
    public override bool Update(float dt)
    {
        return true;
    }
}
