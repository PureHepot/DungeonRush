using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : ModelBase
{
    public BaseCommand lastCommand;
    public BaseCommand current;

    public void GenerateCommand()
    {
        int t = Random.Range(0, 4);
        switch (t)
        {
            case 0:
                EnemyMove(RowIndex + 1, ColIndex);
                break;
            case 1:
                EnemyMove(RowIndex - 1, ColIndex);
                break;
            case 2:
                EnemyMove(RowIndex, ColIndex + 1);
                break;
            case 3:
                EnemyMove(RowIndex, ColIndex - 1);
                break;
        }
    }

    private void EnemyMove(int targetRow, int targetCol)
    {
        if (targetRow < 0 || targetCol < 0 || targetRow >= GameApp.MapManager.TotalRowCount || targetCol >= GameApp.MapManager.TotalColCount) { return; }

        if (isMoving) return;

        GameApp.MapManager.ChangeBlockType(RowIndex, ColIndex, BlockType.floor);

        current = new MoveCommand(this, targetRow, targetCol);
    }
}
