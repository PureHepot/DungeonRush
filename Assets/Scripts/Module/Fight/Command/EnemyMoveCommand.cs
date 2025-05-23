using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveCommand : BaseCommand
{
    private int type;
    private int targetRow;
    private int targetCol;

    string IdleAnim = "Idle";
    string MoveAnim = "Move";

    public EnemyMoveCommand(ModelBase model) : base(model)
    {

    }

    public EnemyMoveCommand(ModelBase model, int Row, int Col, int type = 0) : base(model)
    {
        this.model = model;
        targetRow = Row;
        targetCol = Col;
        this.type = type;
        model.isMoving = true;
    }

    public override void Do()
    {
        base.Do();
        GameApp.MapManager.ChangeBlockType(model.RowIndex, model.ColIndex, BlockType.floor);
        model.RowIndex = targetRow;
        model.ColIndex = targetCol;
        GameApp.MapManager.ChangeBlockType(targetRow, targetCol, BlockType.enemy);

        IdleAnim = "Idle";
        MoveAnim = "Move";

        if (type == 1)
        {
            IdleAnim = "preAtkIdle";
            MoveAnim = "preAtkMove";
        }
        else if (type == 2)
        {
            MoveAnim = "hitMove";
        }
    }

    public override bool Update(float dt)
    {
        

        if (model.Move(targetRow, targetCol, 6*dt))
        {
            model.isMoving = false;
            model.PlayAni(IdleAnim);
            
            return true;
        }
        
        model.PlayAni(MoveAnim);
        return false;
    }
}
