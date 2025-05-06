using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightController : BaseController
{
    public FightController() : base()
    {
        GameApp.ViewManager.Register(ViewType.TipView, new ViewInfo()
        {
            PrefabName = "TipView",
            controller = this,
            parentTf = GameApp.ViewManager.canvasTf,
            Sorting_Order = 10
        });
        GameApp.ViewManager.Register(ViewType.EscView, new ViewInfo()
        {
            PrefabName = "EscView",
            controller = this,
            parentTf = GameApp.ViewManager.canvasTf,
            Sorting_Order = 7
        });
        GameApp.ViewManager.Register(ViewType.PlayerDesView, new ViewInfo()
        {
            PrefabName = "PlayerDesView",
            controller = this,
            parentTf = GameApp.ViewManager.canvasTf,
            Sorting_Order = 3
        });
        GameApp.ViewManager.Register(ViewType.SkillView, new ViewInfo()
        {
            PrefabName = "SkillView",
            controller = this,
            parentTf = GameApp.ViewManager.canvasTf,
            Sorting_Order = 4
        });


        InitModuleEvent();
    }

    public override void InitModuleEvent()
    {
        RegisterFunc(Defines.BeginFight, onBeginFight);
        RegisterFunc(Defines.OnPlayerHpChange, onPlayerHpChange);
        RegisterFunc(Defines.OnPlayerEnergyChange, onPlayerEnergyChange);
    }

    public void onBeginFight(object[] args)
    {
        GameApp.MapManager.Init();
        GameApp.ViewManager.Open(ViewType.TipView, "Level 1");
    }
    public void onPlayerHpChange(object[] args)
    {
        int count = (int)args[0];
        GameApp.ViewManager.GetView<PlayerDesView>(ViewType.PlayerDesView).ChangeHealth(count);
    }
    public void onPlayerEnergyChange(object[] args)
    {
        int count = (int)args[0];
        GameApp.ViewManager.GetView<PlayerDesView>(ViewType.PlayerDesView).ChangeEnergy(count);
    }
}
