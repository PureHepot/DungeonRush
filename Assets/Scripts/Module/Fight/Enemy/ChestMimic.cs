using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ChestMimic : Enemy
{
    [Header("宝箱怪属性")]
    public bool isStunned = false;
    private int stunRounds = 0;
    public int stunMaxRounds = 3; // 昏厥持续回合数
    private int preAtkRound = 0;

    protected override void OnStart()
    {
        base.OnStart();
        ChangeEnemyState(EnemyState.Idle);
        isStunned = false;
        stunRounds = 0;
        type = 0; // 0: 闭合待机, 1: 准备咬人
    }

    public override void Init()
    {
        base.Init();
        GameApp.MapManager.ChangeBlockType(RowIndex, ColIndex, BlockType.enemy);
    }

    //核心指令
    public override void GenerateCommand()
    {
        base.GenerateCommand();

        // 昏厥判定（优先级最高，强力阻断）
        if (isStunned)
        {
            stunRounds++;
            Debug.Log($"【宝箱怪】正在昏厥中... ({stunRounds}/{stunMaxRounds})");

            // 判断是否到达清醒回合
            if (stunRounds >= stunMaxRounds)
            {
                Debug.Log("【宝箱怪】昏厥时间结束，强制清醒！");

                // 强力重置所有状态，不依赖外部方法
                isStunned = false;
                stunRounds = 0;
                type = 0;

                
                if (animator != null)
                {
                    animator.SetBool("isStunned", false);
                }
                PlayAni("Mimic_Idle");

                // 醒来的这一回合，必须待机喘息，不能立刻攻击
                current = new EnemyIdleCommand();
                return;
            }
            else
            {
                // 还没到时间，继续晕厥待机
                current = new EnemyIdleCommand();
                return;
            }
        }

        // 攻击检测
        if (GameApp.PlayerManager.GetDistance(this) <= AttackRange && type == 1)
        {
            ChangeEnemyState(EnemyState.Attack);
        }

        // 状态机路由分发
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
            default:
                current = new EnemyIdleCommand();
                break;
        }
    }

    private void onIdleState()
    {
        current = new EnemyIdleCommand();
        if (GameApp.PlayerManager.GetDistance(this) <= AttackRange)
        {
            ChangeEnemyState(EnemyState.Preattack);
        }
    }

    private void onPreattackState()
    {
       
        if (GameApp.PlayerManager.GetDistance(this) > AttackRange)
        {
            preAtkRound = 0;
            type = 0;
            ChangeEnemyState(EnemyState.Idle);

            // 玩家不在攻击范围内
            PlayAni("Mimic_Idle");

            
            current = new EnemyIdleCommand();
            return;
        }

        //玩家在攻击范围内
        type = 1;
        PlayAni("preAtk"); 

        current = new EnemyIdleCommand();
        preAtkRound++;
    }

    private void onAttackState()
    {
        PlayAni("Mimic_Attack");
        current = new MimicAttackCommand(this, Attack);

        ChangeEnemyState(EnemyState.Idle);
        type = 0;
        preAtkRound = 0;

        GameApp.TimerManager.Register(0.5f, () => {
            if (!isStunned && currentState != EnemyState.Dead)
            {
                PlayAni("Mimic_Idle");
            }
        });
    }

    private void onHitState()
    {
        current = new EnemyHitCommand(this);
        ChangeEnemyState(EnemyState.Idle);
    }

    private void onDeadState()
    {
        GameApp.MapManager.ChangeBlockType(RowIndex, ColIndex, BlockType.floor);
        current = new EnemyDeadCommand(this);
    }

    
    // 无敌机制与受击
    public override void EnemyBeAttacked(int damage)
    {
        if (isStunned)
        {
            CurHp -= damage;
            GameApp.SoundManager.PlayEffect("playerhit", transform.position);

            if (CurHp <= 0)
            {
                ChangeEnemyState(EnemyState.Dead);
                GameApp.SoundManager.PlayEffect("enemydead",transform.position);

                if (SceneManager.GetActiveScene().name == "Level 2")
                {
                    // 全局解锁标志
                    GameApp.PlayerManager.hasSlash = true;
                    GameApp.PlayerManager.slashEnergy = GameApp.PlayerManager.maxSlashEnergy; // 补满能量

                    // 呼叫 UI 界面显示
                    GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, "OnSlashUnlock");

                    // 弹出文字提示
                    GameApp.ControllerManager.ApplyFunc(ControllerType.GameUI, Defines.OpenMessageView, new MessageInfo()
                    {
                        txt = "击杀传说宝箱，获得口口剑！\n(按 J 键或 鼠标左键 释放强力斩击)",
                        okCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); },
                        noCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); }
                    });
                }
                else 
                {
                    Debug.Log("普通宝箱怪被击杀！");
                }
            }
            else
            {
                ChangeEnemyState(EnemyState.Hit);
            }
        }
        else
        {
            GameApp.SoundManager.PlayEffect("block", transform.position);
            Debug.Log("【宝箱怪】处于免疫状态！");
        }
    }

    
    // 护盾抵挡后触发
    public void EnterStunnedState()
    {
        if (isStunned || currentState == EnemyState.Dead) return;

        Debug.Log("【宝箱怪】被护盾弹晕！");
        isStunned = true;
        stunRounds = 0;

        if (animator != null) animator.SetBool("isStunned", true);
        PlayAni("Mimic_Stunned");

        GameApp.SoundManager.PlayEffect("mimic_stun", transform.position);
    }
}
