using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCommand : BaseCommand
{
    float count;
    float time = 0.5f;

    public FallingCommand(ModelBase model) : base(model)
    {
        this.model = model;
    }

    public override void Do()
    {
        base.Do();

        model.PlayAni("fall");
        GameApp.MapManager.ChangeBlockType(model.RowIndex, model.ColIndex, BlockType.fall);
    }

    public override bool Update(float dt)
    {
        count += dt;
        if (count > time)
        {
            GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, Defines.OnPlayerHpChange, -6);
            GameApp.PlayerManager.Player.GetComponentInChildren<SpriteRenderer>().enabled = false;
            return true;
        }


        return false;
    }
}
