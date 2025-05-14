using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObserveCommand : BaseCommand
{
    private int row;
    private int col;
    public EnemyObserveCommand(Enemy enemy, int row, int col) : base(enemy)
    {
        this.model = enemy;
        this.row = row;
        this.col = col;
    }

    public override void Do()
    {
        base.Do();
        this.model.Face2Cell(this.row, this.col);
    }

    public override bool Update(float dt)
    {
        model.PlayAni("Observe");
        return true;
    }
}
