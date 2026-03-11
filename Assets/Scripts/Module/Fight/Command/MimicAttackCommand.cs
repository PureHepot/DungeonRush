using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicAttackCommand : BaseCommand
{
    private ChestMimic mimicAttacker; // 保存攻击者的引用
    private int damage;

    // 构造函数：需要传入 ChestMimic 的引用
    public MimicAttackCommand(ChestMimic attacker, int dmg) : base(GameApp.PlayerManager.Player)
    {
        mimicAttacker = attacker;
        damage = dmg;
    }

    public override void Do()
    {
        base.Do();

        // 【核心机制逻辑】
        // 检查玩家身上是否有护盾激活
        if (GameApp.PlayerManager.isShielded)
        {
            // 1. 消耗玩家护盾，并隐藏视觉效果（MVC 模式）
            GameApp.PlayerManager.isShielded = false;
            GameApp.PlayerManager.HandleShieldVisual(false);

            // 2. 发送消息拦截伤害并弹出提示（可选）
            Debug.Log("Shield blocked! Mimic is stunned!");
            GameApp.SoundManager.PlayEffect("Shieldbreak", GameApp.PlayerManager.Player.transform.position);

            // 【关键修复 3】：延迟 0.3 秒触发晕厥。
            // 这样宝箱怪就会先结结实实地“咬”在盾上，然后再被震晕，视觉效果满分！
            GameApp.TimerManager.Register(0.3f, () => {
                if (mimicAttacker != null)
                {
                    mimicAttacker.EnterStunnedState();
                }
            });

            // 直接返回，不再执行下方的扣血逻辑
            return;
        }

        // --- 否则：玩家没盾，正常执行原本的扣血受击逻辑 ---
        GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, Defines.OnPlayerHpChange, -damage);

        // 播放玩家没盾受击的声音
        GameApp.SoundManager.PlayEffect("playerhit", GameApp.PlayerManager.Player.transform.position);
    }

    // 瞬间完成的指令，不需要 Update 阻塞
    public override bool Update(float dt)
    {
        return true;
    }
}
