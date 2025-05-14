using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCommand : BaseCommand
{
    private int damage;
    private Enemy enemy;
    public AttackCommand(ModelBase model, Enemy enemy, int damage) : base(model)
    {
        this.model = model;
        this.enemy = enemy;
        this.damage = damage;
    }

    public override void Do()
    {
        base.Do();
        Debug.Log($"EnemyHp{enemy.CurHp}");
        enemy.CurHp -= damage;
    }

    public override bool Update(float dt)
    {
        return true;
    }
}
