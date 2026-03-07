using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTarget : Enemy
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
                //onObserveState();
                break;
            case EnemyState.Chase:
                //onChaseState();
                break;
            case EnemyState.Preattack:
                //onPreattackState();
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

    private void GetPath(List<AStarPoint> path)
    {
        paths = path;
    }

}
