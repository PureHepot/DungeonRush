using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ModelBase
{

    protected override void OnUpdate()
    {
        GameApp.CameraManager.SetPos(transform.position);
    }

    protected override void OnFixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            PlayerMove(RowIndex + 1, ColIndex);
        }
        if (Input.GetKey(KeyCode.S))
        {
            PlayerMove(RowIndex - 1, ColIndex);
        }
        if (Input.GetKey(KeyCode.A))
        {
            PlayerMove(RowIndex, ColIndex - 1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            PlayerMove(RowIndex, ColIndex + 1);
        }
    }

    private void PlayerMove(int targetRow, int targetCol)
    {
        if(targetRow < 0 || targetCol < 0 || targetRow >= GameApp.MapManager.TotalRowCount || targetCol >= GameApp.MapManager.TotalColCount) { return; }

        if (isMoving) return;

        if (GameApp.CommandManager.isStop) return;

        GameApp.MapManager.HideStepGrid(GameApp.PlayerManager.Player, int.Parse(GameApp.PlayerManager.datas[1002]["Range"]));

        GameApp.MapManager.ChangeBlockType(RowIndex, ColIndex, BlockType.floor);

        GameApp.CommandManager.AddCommand(new MoveCommand(this, targetRow, targetCol));
    }
}
