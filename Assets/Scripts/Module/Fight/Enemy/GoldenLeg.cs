using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenLeg : Enemy
{
    public List<AStarPoint> paths;
    public int pathIndex;
    private int preAtkRound;

    public int maxlegkids = 4;
    public int legkids;
    public int runRound;
    public override void GenerateCommand()
    {
        base.GenerateCommand();

        if (GameApp.PlayerManager.GetDistance(this, 2) <= AttackRange && type == 1)
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
            case EnemyState.Run:
                onRunState();
                break;
        }
    }

    private void onRunState()
    {
        int[] trow = { 0, 0, 1, -1 };
        int[] tcol = { 1, -1, 0, 0 };
        int offset_row = GameApp.PlayerManager.playerRow - RowIndex;
        int offset_col = GameApp.PlayerManager.playerCol - ColIndex;
        int idx = 0;
        for(int i = 0; i<4; i++)
        {
            if (offset_row == trow[i] && offset_col == tcol[i])
            {
                idx = i;
            }
        }

        int t = Random.Range(0, 4);
        if (t == idx)
        {
            idx = (idx + 1) % 4;
        }
        switch (idx)
        {
            case 0:
                EnemyMove(RowIndex + Step, ColIndex);
                break;
            case 1:
                EnemyMove(RowIndex - Step, ColIndex);
                break;
            case 2:
                EnemyMove(RowIndex, ColIndex + Step);
                break;
            case 3:
                EnemyMove(RowIndex, ColIndex - Step);
                break;
        }
        runRound++;
        if (runRound >= 3)
        {
            ChangeEnemyState(EnemyState.Idle);
            runRound = 0;
        }
        
    }

    private void onDeadState()
    {
        current = new EnemyDeadCommand(this);
    }
    private void onHitState()
    {
        int[] trow = { 0, 0, 1, -1 };
        int[] tcol = { 1, -1, 0, 0 };
        int offset_row = GameApp.PlayerManager.playerRow - RowIndex;
        int offset_col = GameApp.PlayerManager.playerCol - ColIndex;
        int idx = 0;
        for (int i = 0; i < 4; i++)
        {
            if (offset_row == trow[i] && offset_col == tcol[i])
            {
                idx = i;
            }
        }

        int t = Random.Range(0, 4);
        if (t == idx)
        {
            idx = (idx + 1) % 4;
        }
        ChangeType(2);
        switch (idx)
        {
            case 0:
                EnemyMove(RowIndex + Step, ColIndex);
                break;
            case 1:
                EnemyMove(RowIndex - Step, ColIndex);
                break;
            case 2:
                EnemyMove(RowIndex, ColIndex + Step);
                break;
            case 3:
                EnemyMove(RowIndex, ColIndex - Step);
                break;
        }
        ChangeType(0);
        runRound++;
        if (runRound >= 2)
        {
            ChangeEnemyState(EnemyState.Idle);
            runRound = 0;
        }
        //current = new EnemyHitCommand(this);
        
        //ChangeEnemyState(EnemyState.Run);
    }

    private void onAttackState()
    {
        current = new LegSpawnCommand(this, RowIndex, ColIndex);
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
                EnemyMove(RowIndex + Step, ColIndex);
                break;
            case 1:
                EnemyMove(RowIndex - Step, ColIndex);
                break;
            case 2:
                EnemyMove(RowIndex, ColIndex + Step);
                break;
            case 3:
                EnemyMove(RowIndex, ColIndex - Step);
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
            case 2:
                PlayAni("Idle");
                break;
        }
    }

    private void onObserveState()
    {
        AStar astar = new AStar(GameApp.MapManager.TotalRowCount, GameApp.MapManager.TotalColCount);
        astar.FindPath(new AStarPoint(RowIndex, ColIndex), new AStarPoint(GameApp.PlayerManager.Player.RowIndex, GameApp.PlayerManager.Player.ColIndex), GetPath);
        pathIndex = 0;
        current = new EnemyObserveCommand(this, GameApp.PlayerManager.Player.RowIndex, GameApp.PlayerManager.Player.ColIndex);
        int t = Random.Range(0, 10);
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
        else if (GameApp.PlayerManager.GetDistance(this) <= AttackRange + 1)
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
