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
    }

    public override void Do()
    {
        base.Do();
        GameApp.MapManager.ChangeBlockType(model.RowIndex, model.ColIndex, BlockType.floor);
        
        model.RowIndex = targetRow;
        model.ColIndex = targetCol;
        Block b = GameApp.MapManager.GetBlockByPos(targetRow, targetCol);
        if (b.isdamage)
        {
            b.isshot = true;
        }
        
        GameApp.MapManager.HideStepGrid(GameApp.PlayerManager.Player, int.Parse(GameApp.PlayerManager.datas[1002]["Range"]));
        GameApp.MapManager.ChangeBlockType(targetRow, targetCol, BlockType.player);
        GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, Defines.OnPlayerEnergyChange, 1);
        model.PlayAni("Move");
    }

    public override bool Update(float dt)
    {
        if (model.Move(targetRow, targetCol, 6*dt))
        {
            model.PlayAni("Idle");
            return true;
        }
        
        
        return false;
    }
}
