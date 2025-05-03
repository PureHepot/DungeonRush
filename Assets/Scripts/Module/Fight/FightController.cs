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


        InitModuleEvent();
    }

    public override void InitModuleEvent()
    {
        RegisterFunc(Defines.BeginFight, onBeginFight);
    }

    public void onBeginFight(object[] args)
    {
        GameApp.MapManager.Init();
        GameApp.ViewManager.Open(ViewType.TipView, "Level 1");
    }
}
