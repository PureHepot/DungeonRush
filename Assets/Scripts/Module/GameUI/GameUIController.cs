using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIController : BaseController
{
    public GameUIController() : base()
    {
        GameApp.ViewManager.Register(ViewType.StartView, new ViewInfo()
        {
            PrefabName = "StartView",
            controller = this,
            parentTf = GameApp.ViewManager.canvasTf,
            Sorting_Order = 0
        });
        GameApp.ViewManager.Register(ViewType.SettingView, new ViewInfo()
        {
            PrefabName = "SettingView",
            controller = this,
            parentTf = GameApp.ViewManager.canvasTf,
            Sorting_Order = 1
        });
        GameApp.ViewManager.Register(ViewType.MessageView, new ViewInfo()
        {
            PrefabName = "MessageView",
            controller = this,
            parentTf = GameApp.ViewManager.canvasTf,
            Sorting_Order = 999
        });
        InitGlobalEvent();
        InitModuleEvent();
    }

    public override void Init()
    {
        base.Init();
    }

    public override void InitGlobalEvent()
    {
        base.InitGlobalEvent();
    }

    public override void InitModuleEvent()
    {
        RegisterFunc(Defines.OpenStartView, openStartView);
        RegisterFunc(Defines.OpenSetView, openSettingView);
        RegisterFunc(Defines.OpenMessageView, openMessageView);
    }

    public override void RemoveGlobalEvent()
    {
        base.RemoveGlobalEvent();
    }

    public override void RemoveModuelEvent()
    {
        base.RemoveModuelEvent();
    }

    private void openStartView(object[] args)
    {
        GameApp.ViewManager.Open(ViewType.StartView, args);
    }
    private void openSettingView(object[] args)
    {
        GameApp.ViewManager.Open(ViewType.SettingView, args);
    }
    private void openMessageView(object[] args)
    {
        GameApp.ViewManager.Open(ViewType.MessageView, args);   
    }
}
