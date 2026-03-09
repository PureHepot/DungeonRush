using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCommand : BaseCommand
{
    public BodyCommand(PlayerController player) : base(player) { }

    public override void Do()
    {
        base.Do();
        // 只有能量足够且未开启护盾时生效
        if (GameApp.PlayerManager.PlayerEnergy >= 5 && !GameApp.PlayerManager.isShielded)
        {
            //GameApp.PlayerManager.PlayerEnergy -= 5;
            //事件派发
            GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, Defines.OnPlayerEnergyChange, -5);
            GameApp.PlayerManager.isShielded = true;
            GameApp.PlayerManager.HandleShieldVisual(true);
            GameApp.SoundManager.PlayEffect("shieldup", GameApp.PlayerManager.Player.transform.position);
        }
    }
    public override bool Update(float dt)
    {
        return true;
    }
}