using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : BaseCommand
{
    private int targetRow;
    private int targetCol;

    public MoveCommand(ModelBase model) : base(model)
    {

    }

    public MoveCommand(ModelBase model, int Row, int Col) : base(model)
    {
        this.model = model;
        targetRow = Row;
        targetCol = Col;
        model.isMoving = true;
    }

    public override bool Update(float dt)
    {
        if (model.Move(targetRow, targetCol, 6*dt))
        {
            model.isMoving = false;
            model.PlayAni("Idle");
            GameApp.MapManager.ChangeBlockType(targetRow, targetCol, BlockType.obstacle);
            return true;
        }
        
        model.PlayAni("Move");
        return false;
    }
}
