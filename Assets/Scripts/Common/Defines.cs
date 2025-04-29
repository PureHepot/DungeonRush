using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 常量类
/// </summary>
public static class Defines
{
    //控制器相关的事件字符串
    public static readonly string OpenStartView = "OpenStartView"; //打开开始视图
    public static readonly string OpenSetView = "OpenSetView"; //打开开始视图
    public static readonly string OpenMessageView = "OpenMessageView";
    public static readonly string OpenSelectLevelView = "OpenSelectLevelView";
    public static readonly string LoadingScene = "LoadingScene";
    public static readonly string BeginFight = "BeginFight";//开始战斗

    //全局事件相关
    public static readonly string ShowLevelEvent = "ShowLevelEvent";
    public static readonly string HideLevelEvent = "HideLevelEvent";

    public static readonly string OnSelectEvent = "OnSelectEvent";//选中事件
    public static readonly string OnUnSelectEvent = "OnUnSelectEvent";//未选中事件

    //option
    public static readonly string OnAttackEvent = "OnAttackEvent";
    public static readonly string OnIdleEvent = "OnIdleEvent";
    public static readonly string OnCancelEvent = "OnCancelEvent";
    public static readonly string OnRemoveHeroToSceneEvent = "OnRemoveHeroToSceneEvent";
}
