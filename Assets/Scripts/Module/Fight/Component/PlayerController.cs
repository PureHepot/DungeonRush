using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ModelBase
{

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            PlayerMove(RowIndex + 1, ColIndex);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            PlayerMove(RowIndex - 1, ColIndex);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerMove(RowIndex, ColIndex - 1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            PlayerMove(RowIndex, ColIndex + 1);
        }
    }

    private void PlayerMove(int targetRow, int targetCol)
    {
        if(targetRow < 0 || targetCol < 0 || targetRow > GameApp.MapManager.TotalRowCount || targetCol > GameApp.MapManager.TotalColCount) { return; }

        if (isMoving) return;

        GameApp.CommandManager.AddCommand(new MoveCommand(this, targetRow, targetCol));
    }
}
