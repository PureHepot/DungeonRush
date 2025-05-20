using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashCommand : BaseCommand
{
    private Block block;
    private float flashTime = 0.15f;
    private float flashCount = 0;

    public FlashCommand(ModelBase model, Block block) : base(model)
    {
        this.model = model;
        this.block = block;
    }

    public override void Do()
    {
        base.Do();
        GameApp.MapManager.ChangeBlockType(model.RowIndex, model.RowIndex, BlockType.floor);
        this.model.RowIndex = this.block.RowIndex;
        this.model.ColIndex = this.block.ColIndex;
        GameApp.MapManager.ChangeBlockType(model.RowIndex, model.RowIndex, BlockType.player);
        GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, Defines.OnPlayerEnergyChange, -5);
    }

    public override bool Update(float dt)
    {
        flashCount += dt;
        if (flashCount >= flashTime)
        {
            model.ChangePos(block.RowIndex, block.ColIndex);
            model.PlayAni("Flashout");
            return true;
        }
        model.PlayAni("Flash");
        return false;
    }

}
