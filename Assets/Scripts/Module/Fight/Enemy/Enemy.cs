using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Observe,
    Chase,
    Preattack,
    Normal,
    Attack,
    Run,
    Hit,
    Dead
}

public class StateMachine
{

}

public class Enemy : ModelBase
{
    public BaseCommand lastCommand;
    public BaseCommand current;

    //public Sprite normalSp;
    //public Sprite preatkSp;
    public int Step;
    public int AttackRange;
    public int VisionDis;
    public int ObserInterval;
    public int obserTime;
    public int type;

    protected EnemyState currentState;

    protected override void OnStart()
    {
        base.OnStart();
        data = GameApp.ConfigManager.GetConfigData("enemy").GetDataById(Id);

        Attack = int.Parse(data["Damage"]);
        AttackRange = int.Parse(data["AtkRange"]);
        CurHp = MaxHp = int.Parse(data["Hp"]);
        Step = int.Parse(data["MoveStep"]);
        VisionDis = int.Parse(data["VisionDis"]);
        ObserInterval = int.Parse(data["ObInterval"]);
        //normalSp = Resources.Load<Sprite>("Arts/enemySprite/" + data["NormalSpr"]);
        //preatkSp = Resources.Load<Sprite>("Arts/enemySprite/" + data["PreAtkSpr"]);
    }


    public virtual void GenerateCommand()
    {
        
    }

    protected virtual void EnemyMove(int targetRow, int targetCol)
    {
        if (targetRow < 0 || targetCol < 0 || targetRow >= GameApp.MapManager.TotalRowCount || targetCol >= GameApp.MapManager.TotalColCount) { return; }

        if (isMoving) return;

        if (GameApp.MapManager.GetBlockByPos(targetRow, targetCol) == null) { return; }

        if (GameApp.MapManager.GetBlockType(targetRow, targetCol) != BlockType.floor) return;

        current = new EnemyMoveCommand(this, targetRow, targetCol, type);
    }
    public virtual void EnemyBeAttacked(int damage)
    {
        CurHp-=damage;
        if (CurHp <= 0) 
            ChangeEnemyState(EnemyState.Dead);
        else
            ChangeEnemyState(EnemyState.Hit);
    }

    protected void ChangeEnemyState(EnemyState state, params object[] args)
    {
        Debug.Log($"Change to {state.ToString()}");
        currentState = state;
    }
}
