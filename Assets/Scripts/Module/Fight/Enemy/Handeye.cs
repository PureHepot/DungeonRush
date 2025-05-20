using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handeye : Enemy
{
    public List<AStarPoint> paths;
    public int pathIndex;
    private int preAtkRound;


    public override void GenerateCommand()
    {
        base.GenerateCommand();

        switch (currentState)
        {
            case EnemyState.Idle:
                onIdleState();
                break;
            case EnemyState.Observe:
                onObserveState();
                break;
            case EnemyState.Chase:
                onChaseState();
                break;
            case EnemyState.Preattack:
                onPreattackState();
                break;
            case EnemyState.Attack:
                onAttackState();
                break;
            case EnemyState.Hit:
                onHitState();
                break;
            case EnemyState.Dead:
                onDeadState();
                break;
        }
    }


    private void onDeadState()
    {
        current = new EnemyDeadCommand(this);
    }
    private void onHitState()
    {
        current = new EnemyHitCommand(this);

        ChangeEnemyState(EnemyState.Idle);
    }

    private void onAttackState()
    {
        current = new EnemyAttackCommand(this, Attack);
        ChangeEnemyState(EnemyState.Idle);
        //ChangeType(0);
        type = 0;
    }

    private void onIdleState()
    {
        current = new EnemyIdleCommand();
        PlayAni("Idle");

        if (GameApp.PlayerManager.GetDistance(this) <= VisionDis)
        {
            ChangeEnemyState(EnemyState.Preattack);
        }
    }


    private void ChangeType(int t)
    {
        type = t;
        switch (type)
        {
            case 0:
                PlayAni("Idle");
                break;
            case 1:
                PlayAni("preAtkIdle");
                break;
        }
    }

    private void onObserveState()
    {
        //AStar astar = new AStar(GameApp.MapManager.TotalRowCount, GameApp.MapManager.TotalColCount);
        //astar.FindPath(new AStarPoint(RowIndex, ColIndex), new AStarPoint(GameApp.PlayerManager.Player.RowIndex, GameApp.PlayerManager.Player.ColIndex), GetPath);
        //pathIndex = 0;
        //current = new EnemyObserveCommand(this, GameApp.PlayerManager.Player.RowIndex, GameApp.PlayerManager.Player.ColIndex);
        //int t = Random.Range(0, 10);
        //if (t <= 3)
        //{
        //    pathIndex++;
        //    if (pathIndex < paths.Count)
        //    {
        //        EnemyMove(paths[pathIndex].RowIndex, paths[pathIndex].ColIndex);
        //    }
        //}
        //t = Random.Range(0, 10);
        //if (t < 3) ChangeType(1);
        //else ChangeType(0);

        ChangeType(1);

        if (GameApp.PlayerManager.GetDistance(this) > VisionDis)
        {
            ChangeEnemyState(EnemyState.Idle);
        }
    }

    private void onChaseState()
    {
       
    }

    private void GetPath(List<AStarPoint> path)
    {
        paths = path;
    }

    private void onPreattackState()
    {
        ChangeType(1);
        preAtkRound++;
        if (preAtkRound % 3 == 1)
        {
            Debug.Log(preAtkRound);
            current = new HandeyeShotCommand(this);
        }

        
        if (preAtkRound >= 5)
        {
            preAtkRound = 0;
            ChangeEnemyState(EnemyState.Idle);
            ChangeType(0);
        }

        if (GameApp.PlayerManager.GetDistance(this) > VisionDis)
        {
            ChangeEnemyState(EnemyState.Idle);
        }
    }
}
