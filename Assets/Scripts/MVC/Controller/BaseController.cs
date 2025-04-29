using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制器基类
/// </summary>
public class BaseController
{
    private Dictionary<string, System.Action<object[]>> message;//事件字典

    protected BaseModel model;//模板数据

    public BaseController()
    {
        message = new Dictionary<string, System.Action<object[]>>();
    }
    //注册后调用的初始化函数（要所有控制器初始化后执行）
    public virtual void Init()
    {

    }

    public virtual void OnLoadView(IBaseView view) { }

    //打开视图
    public virtual void OpenView(IBaseView view)
    {

    }

    //关闭视图
    public virtual void CloseView(IBaseView view)
    {

    }

    //注册模板事件
    public void RegisterFunc(string eventName, System.Action<object[]> callback)
    {
        if (message.ContainsKey(eventName))
        {
            message[eventName] += callback;
        }
        else
        {
            message.Add(eventName, callback);
        }
    }

    //移除模板事件
    public void UnregisterFunc(string eventName)
    {
        if (message.ContainsKey(eventName))
        {
            message.Remove(eventName);
        }
    }

    //触发本模块事件
    public void ApplyFunc(string eventName, params object[] args)
    {
        if (message.ContainsKey(eventName))
        {
            message[eventName].Invoke(args);
        }
        else
        {
            Debug.Log("error:" +  eventName);
        }
    }

    //触发其他模块事件
    public void ApplyControllerFunc(int controllerKey, string eventName, params object[] args)
    {
        GameApp.ControllerManager.ApplyFunc(controllerKey, eventName, args);
    }
    public void ApplyControllerFunc(ControllerType controllerKey, string eventName, params object[] args)
    {
        ApplyControllerFunc((int)controllerKey, eventName, args);
    }

    public void SetModel(BaseModel model)//一个控制器对应一个数据模块，整个MVC流程就是初始化控制器Module，然后同时完成各种事件、数据的注册、加载
    {                                    //编写好控制器后，在各个视图中调用即可，而实现全局控制的是GameApp中的各种manager，为整个框架的控制中枢
        this.model = model;
        this.model.controller = this;
    }

    public BaseModel GetModel()
    {
        return model;
    }

    public T GetModel<T>() where T : BaseModel
    {
        return model as T;
    }

    public BaseModel GetControllerModel(int controllerKey)
    {
        return GameApp.ControllerManager.GetControllerModel(controllerKey);
    }

    //删除控制器
    public virtual void Destroy()
    {
        RemoveModuelEvent();
        RemoveGlobalEvent();
    }

    //初始化模板事件
    public virtual void InitModuleEvent()
    {

    }

    //移除模板事件
    public virtual void RemoveModuelEvent()
    {

    }

    //初始化全局模板事件
    public virtual void InitGlobalEvent()
    {

    }

    //移除全局事件
    public virtual void RemoveGlobalEvent()
    {

    }
}
