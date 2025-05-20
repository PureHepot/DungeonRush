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
        GameApp.CameraManager.SetPos(transform.position);

        
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

        GameApp.MapManager.HideStepGrid(GameApp.PlayerManager.Player, int.Parse(GameApp.PlayerManager.datas[1002]["Range"]));

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
