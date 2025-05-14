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
    Run
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

    protected void EnemyMove(int targetRow, int targetCol)
    {
        if (targetRow < 0 || targetCol < 0 || targetRow >= GameApp.MapManager.TotalRowCount || targetCol >= GameApp.MapManager.TotalColCount) { return; }

        if (isMoving) return;

        current = new EnemyMoveCommand(this, targetRow, targetCol, type);
    }

    protected void ChangeEnemyState(EnemyState state)
    {
        Debug.Log($"Change to {state.ToString()}");
        currentState = state;
    }
}
