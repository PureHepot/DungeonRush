using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Enemy
{
    [Header("炸弹专属设置")]
    public GameObject rangeIndicator; // 拖入刚才制作的 RangeIndicator 子物体
    private int fuseTimer = 0;        // 引信计时器
    private bool hasExploded = false;
    protected override void OnStart()
    {
        base.OnStart();
        ChangeEnemyState(EnemyState.Idle);
        if (rangeIndicator != null) rangeIndicator.SetActive(false);
    }

    public override void Init()
    {
        base.Init();
        // 初始化时，向 MapManager 注册为 enemy 属性，产生物理阻挡
        GameApp.MapManager.ChangeBlockType(RowIndex, ColIndex, BlockType.enemy);
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

        // 当玩家靠近炸弹（距离小于等于 VisionDis）时触发引信
        // 建议在配置表中把炸弹的 VisionDis 设为 1 或 2
        if (GameApp.PlayerManager.GetDistance(this) <= VisionDis)
        {
            ChangeEnemyState(EnemyState.Preattack);
            PlayAni("Bomb_Preatk"); // 播放 f1 预警动画
            Debug.Log("发现玩家");
            if (rangeIndicator != null) rangeIndicator.SetActive(true); // 亮起红框！
        }
    }

    private void onPreattackState()
    {
        current = new EnemyIdleCommand();
        fuseTimer++;
        GameApp.SoundManager.PlayEffect("bomb_ignit", transform.position);
        // 预警 1 个回合后引爆（如果想给玩家更多逃跑时间，可以把 1 改成 2）
        if (fuseTimer >= 1)
        {
            ChangeEnemyState(EnemyState.Attack);
        }
    }

    private void onAttackState()
    {
        // 【终极防线】：如果已经炸过了，直接拦截，天王老子来了也不能炸第二次！
        if (hasExploded) return;
        hasExploded = true;

        PlayAni("Bomb_Attack");

        current = new BombExplodeCommand(this, Attack);

        CurHp = 0;
        ChangeEnemyState(EnemyState.Dead);
    }

    private void onDeadState()
    {
        // 炸弹消失，恢复该网格的通行权
        GameApp.MapManager.ChangeBlockType(RowIndex, ColIndex, BlockType.floor);
        if (rangeIndicator != null) rangeIndicator.SetActive(false);

        // ==========================================
        // 【核心修复】：强行清空当前指令！
        // 斩断指令残留，防止 EnemyManager 重复执行上一回合的爆炸指令
        // ==========================================
        current = null;
    }

    // 处理玩家手贱砍炸弹的逻辑
    public override void EnemyBeAttacked(int damage)
    {
        // 如果处于待机状态被砍，强制点燃引信！
        if (currentState == EnemyState.Idle)
        {
            ChangeEnemyState(EnemyState.Preattack);
            PlayAni("Bomb_Preatk");
            
            if (rangeIndicator != null) rangeIndicator.SetActive(true);
        }

        CurHp -= damage;
        GameApp.SoundManager.PlayEffect("playerhit", transform.position);

        // 如果玩家伤害够高，可以直接把炸弹劈碎而不触发爆炸
        if (CurHp <= 0)
        {
            ChangeEnemyState(EnemyState.Dead);
        }
    }
}

// ==========================================
// 炸弹专属爆炸指令（处理 3x3 范围无差别伤害）
// ==========================================
public class BombExplodeCommand : BaseCommand
{
    private Bomb bomb;
    private int damage;

    public BombExplodeCommand(Bomb bomb, int damage) : base(bomb)
    {
        this.bomb = bomb;
        this.damage = damage;
    }

    public override void Do()
    {
        base.Do();

        // 震动屏幕增加爆炸的张力
        GameApp.CameraManager.CameraShake();
        GameApp.SoundManager.PlayEffect("bomb", bomb.transform.position); // 如果你有爆炸音效可以取消注释
        //Debug.Log($"发生爆炸！炸弹名称: {bomb.gameObject.name}, 内存ID: {bomb.GetInstanceID()}");
        int centerRow = bomb.RowIndex;
        int centerCol = bomb.ColIndex;

        // 1. 炸玩家
        PlayerController player = GameApp.PlayerManager.Player;
        if (Mathf.Abs(player.RowIndex - centerRow) <= 1 && Mathf.Abs(player.ColIndex - centerCol) <= 1)
        {
            player.PlayAni("Hit");
            // 通过事件中心扣除玩家血量并刷新UI
            GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, Defines.OnPlayerHpChange, -damage);
        }

        // 2. 炸怪物 (炸弹应该六亲不认，连其他敌人一起炸！)
        List<Enemy> hitEnemies = new List<Enemy>();
        foreach (var enemy in GameApp.EnemyManager.enemies)
        {
            if (enemy != bomb && enemy.CurHp > 0)
            {
                if (Mathf.Abs(enemy.RowIndex - centerRow) <= 1 && Mathf.Abs(enemy.ColIndex - centerCol) <= 1)
                {
                    hitEnemies.Add(enemy);
                }
            }
        }

        foreach (var enemy in hitEnemies)
        {
            enemy.EnemyBeAttacked(damage);
        }
    }

    public override bool Update(float dt)
    {
        return true;
    }
}
