using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ModelBase
{
    protected override void OnStart()
    {
        base.OnStart();

        Attack = 1;
    }

    protected override void OnUpdate()
    {
        // 摄像机跟随玩家
        GameApp.CameraManager.SetPos(transform.position);

        // 【新增】：在这里监听隐藏斩击技能的输入
        // ==========================================
        // 确保玩家存活、当前没有正在执行其他指令、且不是敌人的回合
        if (!GameApp.PlayerManager.isDead && !GameApp.CommandManager.isRunningCommand && !GameApp.CommandManager.isStop)
        {
            // 检测是否按下了 J 键 或 鼠标左键，并且已经解锁了该技能 (hasSlash 需要你在 PlayerManager 里定义)
            if ((Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0)) && GameApp.PlayerManager.hasSlash)
            {
                // 可选：如果你想消耗能量，在这里加上判断
                // if (GameApp.PlayerManager.PlayerEnergy >= 2) {
                //     GameApp.PlayerManager.PlayerEnergy -= 2;

                // 生成斩击指令并交给管理器执行
                GameApp.CommandManager.AddCommand(new PlayerSlashCommand(this));

                // }
            }
        }

    }

    protected override void OnFixedUpdate()
    {
        if (GameApp.PlayerManager.isDead) return;
        if (Input.GetKey(KeyCode.W))
        {
            PlayerMove(RowIndex + 1, ColIndex);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            PlayerMove(RowIndex - 1, ColIndex);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            PlayerMove(RowIndex, ColIndex - 1);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            PlayerMove(RowIndex, ColIndex + 1);
        }
    }

    private void PlayerMove(int targetRow, int targetCol)
    {
        if(targetRow < 0 || targetCol < 0 || targetRow >= GameApp.MapManager.TotalRowCount || targetCol >= GameApp.MapManager.TotalColCount) { return; }

        if(GameApp.MapManager.GetBlockType(targetRow,targetCol) == BlockType.empty || GameApp.MapManager.GetBlockOriginType(targetRow, targetCol) == BlockType.obstacle) { return; }

        if (GameApp.CommandManager.isRunningCommand) return;

        if (GameApp.CommandManager.isStop) return;

        Enemy enemy = FindEnemyInPos(targetRow, targetCol);
        if (enemy)
        {
            PlayerAttack(enemy);
            return;
        }

        GameApp.CommandManager.AddCommand(new MoveCommand(this, targetRow, targetCol));
    }

    private Enemy FindEnemyInPos(int row, int col)
    {
        return GameApp.EnemyManager.GetEnemybyPos(row, col);
    }

    private void PlayerAttack(Enemy enemy)
    {
        if(isAttacking) return;
        GameApp.CommandManager.AddCommand(new AttackCommand(this, enemy, Attack));
    }

    
}
