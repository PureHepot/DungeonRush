using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evil : Enemy
{
    public override void GenerateCommand()
    {
        base.GenerateCommand();
        switch (currentState)
        {
            case EnemyState.Idle:
                onIdleState();
                break;
            case EnemyState.Chase:
                onChaseState();
                break;
        }
    }


    private void onIdleState()
    {
        if (Random.Range(0, 3) >= 1)
        {
            current = new EnemyIdleCommand();
        }
        else
        {
            RandomMove();
        }
        if (GameApp.PlayerManager.GetDistance(this) <= VisionDis)
        {
            ChangeEnemyState(EnemyState.Chase);
        }
    }
    private void RandomMove()
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

    private void onChaseState()
    {
        AStar astar = new AStar(GameApp.MapManager.TotalRowCount, GameApp.MapManager.TotalColCount);
        astar.FindPath(new AStarPoint(RowIndex, ColIndex), new AStarPoint(GameApp.PlayerManager.Player.RowIndex, GameApp.PlayerManager.Player.ColIndex),Move2Player);
        if (GameApp.PlayerManager.GetDistance(this) > VisionDis)
        {
            ChangeEnemyState(EnemyState.Idle);
        }
    }

    private void Move2Player(List<AStarPoint> path)
    {
        current = new EnemyMoveCommand(this, path[1].RowIndex, path[1].ColIndex);
    }
}
