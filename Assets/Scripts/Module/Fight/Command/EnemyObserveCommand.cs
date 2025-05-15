using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObserveCommand : BaseCommand
{
    private int row;
    private int col;
    private bool jump;
    public EnemyObserveCommand(Enemy enemy, int row, int col, bool isAlarmed = false) : base(enemy)
    {
        this.model = enemy;
        this.row = row;
        this.col = col;
        this.jump = isAlarmed;
    }

    public override void Do()
    {
        base.Do();
        if(jump || col > model.ColIndex && model.transform.localScale.x<0 || col < model.ColIndex && model.transform.localScale.x > 0)
            model.PlayAni("Observe");
        this.model.Face2Cell(this.row, this.col);
    }

    public override bool Update(float dt)
    {
        
        return true;
    }
}
