using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealCommand : BaseCommand
{
    public HealCommand(ModelBase model) : base(model)
    {
        this.model = model;
    }

    public override void Do()
    {
        base.Do();
        GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, Defines.OnPlayerEnergyChange, -5);
        GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, Defines.OnPlayerHpChange, 1);
        GameApp.SoundManager.PlayEffect("heal", Camera.main.transform.position);
    }
    public override bool Update(float dt)
    {
        //todo:Ìí¼Óeffect
        return true;
    }
}
