using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleCommand : BaseCommand
{
    float count;
    float time = 0.3f;
    public IdleCommand(ModelBase model) : base(model)
    {
        this.model = model;
    }

    public override void Do()
    {
        base.Do();
        Block b = GameApp.MapManager.GetBlockByPos(GameApp.PlayerManager.playerRow, GameApp.PlayerManager.playerCol);
        if (b.isdamage)
        {
            b.isshot = true;
        }
        GameApp.MapManager.HideStepGrid(GameApp.PlayerManager.Player, int.Parse(GameApp.PlayerManager.datas[1002]["Range"]));
        model.PlayAni("Idle");
    }

    public override bool Update(float dt)
    {
        count += dt;
        if(count > time)
        {
            return true;
        }

        return false;
    }
}
