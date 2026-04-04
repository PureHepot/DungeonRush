using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ModelBase
{
    [Header("技能特效预制件")]
    public GameObject slashVfxPrefab; // 【必须加上这一行】

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
            if ((Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0)) && GameApp.PlayerManager.hasSlash)
            {
                // 检测能量是否足够 (满值为100，消耗30)
                if (GameApp.PlayerManager.slashEnergy >= 30)
                {
                    // 【核心修复】：删掉这里的扣除能量和刷新UI的代码！
                    // 只管把带有预制体的指令发出去
                    GameApp.CommandManager.AddCommand(new PlayerSlashCommand(this, slashVfxPrefab));
                }
                else
                {
                    Debug.Log("大剑能量不足，无法释放斩击！");
                }
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

        if(GameApp.MapManager.GetBlockType(targetRow,targetCol) == BlockType.empty ||
           GameApp.MapManager.GetBlockOriginType(targetRow, targetCol) == BlockType.obstacle ||
           GameApp.MapManager.GetBlockType(targetRow,targetCol) == BlockType.constraint ||    // 限制块也算作障碍
           GameApp.MapManager.GetBlockType(targetRow, targetCol) == BlockType.constraint1) { return; }

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
