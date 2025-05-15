using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AttackCommand : BaseCommand
{
    private int damage;
    private Enemy enemy;
    private float attackTime;
    private float timeCount;
    public AttackCommand(ModelBase model, Enemy enemy, int damage) : base(model)
    {
        this.model = model;
        this.enemy = enemy;
        this.damage = damage;
        attackTime = 0.3f;
    }

    public override void Do()
    {
        base.Do();
        Debug.Log($"EnemyHp{enemy.CurHp}");
        model.isAttacking = true;
        enemy.EnemyBeAttacked(damage);
        model.Face2Cell(enemy.RowIndex,enemy.ColIndex);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(model.transform.DOMove(GameApp.MapManager.GetBlockPos(enemy.RowIndex, enemy.ColIndex),0.1f))
                .Append(model.transform.DOMove(GameApp.MapManager.GetBlockPos(model.RowIndex,model.ColIndex),0.2f));
    }

    public override bool Update(float dt)
    {
        timeCount += dt;
        if (timeCount > attackTime)
        {
            model.isAttacking = false;
            return true;
        }
        return false;
    }
}
