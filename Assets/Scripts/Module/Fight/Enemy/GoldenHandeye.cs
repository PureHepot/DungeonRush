using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenHandeye : Enemy
{
    private int preAtkRound;

    protected override void OnStart()
    {
        base.OnStart();

        //索敌范围
        VisionDis = 20;
    }

    public override void Init()
    {
        base.Init();
        // 设置自身及右、上、右上的 4 个格子为 enemy 阻挡
        SetBossGridOccupancy(BlockType.enemy);
    }

    public override void GenerateCommand()
    {
        base.GenerateCommand();

        switch (currentState)
        {
            case EnemyState.Idle:
                onIdleState();
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

    private void onIdleState()
    {
        current = new EnemyIdleCommand();
        PlayAni("Idle");

        
        if (GameApp.PlayerManager.GetDistance(this) <= VisionDis)
        {
            ChangeEnemyState(EnemyState.Preattack);
        }
    }

    private void onPreattackState()
    {
        ChangeType(1); // 切换到开眼/红温状态的动画
        preAtkRound++;

        // ==========================================
        // 【Boss 特化 2】：更具压迫感的攻击频率
        // 原版是 preAtkRound % 3 == 1 射击，并在第 5 回合才休息
        // 这里我们可以让它连续射击，或者保持原有的弹幕节奏
        // ==========================================
        if (preAtkRound % 3 == 1)
        {
            Debug.Log($"【Boss 黄金巨眼】正在向玩家开火！当前充能回合: {preAtkRound}");
            // 沿用原版的射击指令
            current = new HandeyeShotCommand(this);
        }
        else
        {
            // 没开火的回合，保持待机指令
            current = new EnemyIdleCommand();
        }

        // 持续压制 6 个回合后，休息一回合
        if (preAtkRound >= 6)
        {
            preAtkRound = 0;
            ChangeEnemyState(EnemyState.Idle);
            ChangeType(0);
        }
    }

    private void onAttackState()
    {
        current = new EnemyAttackCommand(this, Attack);
        ChangeEnemyState(EnemyState.Idle);
        type = 0;
    }

    private void onHitState()
    {
        current = new EnemyHitCommand(this);
        ChangeEnemyState(EnemyState.Idle);
    }

    private void onDeadState()
    {
        // 死亡时解锁 2x2 区域
        SetBossGridOccupancy(BlockType.floor);

        current = null;
        current = new EnemyDeadCommand(this);
    }

    private void SetBossGridOccupancy(BlockType type)
    {
        // 遍历 Boss 所占的 2x2 区域 (RowIndex 到 RowIndex+1, ColIndex 到 ColIndex+1)
        for (int r = RowIndex; r <= RowIndex + 1; r++)
        {
            for (int c = ColIndex; c <= ColIndex + 1; c++)
            {
                // 安全防错：防止越出地图边界
                if (r >= 0 && r < GameApp.MapManager.TotalRowCount && c >= 0 && c < GameApp.MapManager.TotalColCount)
                {
                    GameApp.MapManager.ChangeBlockType(r, c, type);
                }
            }
        }
    }

    public override bool CheckPos(int targetRow, int targetCol)
    {
        // 只要玩家攻击的坐标，在 Boss 的左下(RowIndex, ColIndex)到右上(+1, +1)之间，就算命中
        return targetRow >= this.RowIndex && targetRow <= this.RowIndex + 1 &&
               targetCol >= this.ColIndex && targetCol <= this.ColIndex + 1;
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
}
