using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRunCommand : BaseCommand
{
    int[] trow = { 0, 0, 1, -1 };
    int[] tcol = {1, -1, 0, 0 };
    int row;
    int col;

    public EnemyRunCommand(ModelBase model, int row, int col) : base(model)
    {
        this.model = model;
        this.row = row;
        this.col = col;
    }

    public override void Do()
    {
        base.Do();
    }

    public override bool Update(float dt)
    {
        return true;
    }

}
