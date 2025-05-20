using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Evil : Enemy
{
    public List<AStarPoint> paths;
    public int pathIndex;
    private int preAtkRound;


    public override void GenerateCommand()
    {
        base.GenerateCommand();

        if (GameApp.PlayerManager.GetDistance(this) <= AttackRange && type == 1)
        {
            ChangeEnemyState(EnemyState.Attack);
        }

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
            ChangeEnemyState(EnemyState.Observe);
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
        AStar astar = new AStar(GameApp.MapManager.TotalRowCount, GameApp.MapManager.TotalColCount);
        astar.FindPath(new AStarPoint(RowIndex, ColIndex), new AStarPoint(GameApp.PlayerManager.Player.RowIndex, GameApp.PlayerManager.Player.ColIndex), GetPath);
        pathIndex = 0;
        current = new EnemyObserveCommand(this, GameApp.PlayerManager.Player.RowIndex, GameApp.PlayerManager.Player.ColIndex);
        int t = Random.Range(0,10);
        if (t <= 3)
        {
            pathIndex++;
            if (pathIndex < paths.Count)
            {
                EnemyMove(paths[pathIndex].RowIndex, paths[pathIndex].ColIndex);
            }
        }
        t = Random.Range(0, 10);
        if (t < 3) ChangeType(1);
        else ChangeType(0);

        ChangeEnemyState(EnemyState.Chase);
    }

    private void onChaseState()
    {
        if (obserTime > ObserInterval)
        {
            obserTime = 0;
            ChangeEnemyState(EnemyState.Observe);
        }
        Debug.Log(obserTime);
        pathIndex++;
        if (pathIndex < paths.Count)
        {
            EnemyMove(paths[pathIndex].RowIndex, paths[pathIndex].ColIndex);
        }
        
        obserTime += 1;

        if (GameApp.PlayerManager.GetDistance(this) > VisionDis)
        {
            ChangeEnemyState(EnemyState.Idle);
        }
        else if (GameApp.PlayerManager.GetDistance(this) <= AttackRange+1)
        {
            ChangeEnemyState(EnemyState.Preattack);
        }
    }

    private void GetPath(List<AStarPoint> path)
    {
        paths = path;
    }

    private void onPreattackState()
    {
        ChangeType(1);

        

        if (Random.Range(0, 3) == 1)
        {
            RandomMove();
        }
        preAtkRound++;
        if (preAtkRound >= 3)
        {
            preAtkRound = 0;
            ChangeEnemyState(EnemyState.Idle);
            ChangeType(0);
        }
    }
}
