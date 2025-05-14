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
        GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, Defines.OnPlayerHpChange, -damage);
    }

    public override bool Update(float dt)
    {
        model.PlayAni("Attack");
        return true;
    }
}
