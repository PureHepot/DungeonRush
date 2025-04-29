using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseView
{
    bool IsInit();//初始化否

    bool IsShow();//是否显示

    void InitUI();//初始化面板

    void InitData();//初始化数据

    void Open(params object[] args);//打开面板

    void Close(params object[] args);//关闭面板

    void DestroyView();//删除面板

    void ApplyFunc(string eventName, params object[] args);//触发本模块事件

    void ApplyControllerFunc(int controllerKey, string eventName, params object[] args);//触发其他控制器模块事件

    void SetVisible(bool value);//设置显示隐藏

    int ViewId { get; set; }//面板id

    BaseController Controller { get; set; }//面板所属控制器



}
