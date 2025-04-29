using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : BaseController
{
    public GameController() : base()
    {
        InitModuleEvent();
        InitGlobalEvent();
    }

    public override void Init()
    {
        base.Init();
        ApplyControllerFunc(ControllerType.GameUI, Defines.OpenStartView);
    }

    public override void InitGlobalEvent()
    {
        base.InitGlobalEvent();
    }

    public override void InitModuleEvent()
    {
        
    }

    public override void RemoveGlobalEvent()
    {
        base.RemoveGlobalEvent();
    }

    public override void RemoveModuelEvent()
    {
        base.RemoveModuelEvent();
    }
}
