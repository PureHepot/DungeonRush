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
