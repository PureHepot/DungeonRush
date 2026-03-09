using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackCommand : BaseCommand
{
    private int damage;

    public EnemyAttackCommand(Enemy enemy, int damage) : base(enemy)
    {
        model = enemy;
        this.damage = damage;
    }

    public override void Do()
    {
        base.Do();
        if (GameApp.PlayerManager.isShielded)
        {
            // 1. 消耗护盾并隐藏视觉效果
            GameApp.PlayerManager.isShielded = false;
            GameApp.PlayerManager.HandleShieldVisual(false);

            // 2. 播放护盾抵挡音效
            GameApp.SoundManager.PlayEffect("shieldbreak", GameApp.PlayerManager.Player.transform.position);

            // 3. 弹出文字反馈（可选）
            Debug.Log("Shield blocked the attack!");

            
            return;
        }
        GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, Defines.OnPlayerHpChange, -damage);
    }

    public override bool Update(float dt)
    {
        model.PlayAni("Attack");
        return true;
    }
}
