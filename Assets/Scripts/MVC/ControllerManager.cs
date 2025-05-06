using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 控制器管理器
/// </summary>
public class ControllerManager
{
    private Dictionary<int, BaseController> _modules;//存储控制器的字典

    public ControllerManager()
    {
        _modules = new Dictionary<int, BaseController>();
    }

    //注册管理器
    public void Register(ControllerType controllerType, BaseController ctl)
    {
        Register((int)controllerType, ctl);
    }
    public void Register(int controllerKey, BaseController ctl)
    {
        if (_modules.ContainsKey(controllerKey) == false)
        {
            _modules.Add(controllerKey, ctl);
        }
    }

    public void InitAllModules()
    {
        foreach (var item in _modules)
        {
            item.Value.Init();
        }
    }

    public void UnRegister(int controllerKey)
    {
        if (_modules.ContainsKey(controllerKey))
        {
            _modules.Remove(controllerKey);
        }
    }

    //清除
    public void Clear()
    {
        _modules.Clear();
    }

    //清除所有控制器
    public void ClearAllModules()
    {
        List<int> keys = _modules.Keys.ToList();
        for (int i = 0; i < keys.Count; i++)
        {
            _modules[keys[i]].Destroy();
            _modules.Remove(keys[i]);
        }
    }

    //跨模板触发消息
    public void ApplyFunc(int controllerKey, string eventName, params object[] args)
    {
        if (_modules.ContainsKey(controllerKey))
        {
            _modules[controllerKey].ApplyFunc(eventName, args);
        }
    }
    public void ApplyFunc(ControllerType type, string eventName, params object[] args)
    {
        int key = (int)type;
        ApplyFunc(key, eventName, args);
    }

    //获取某个控制器的Model对象
    public BaseModel GetControllerModel(int controllerKey)
    {
        if (_modules.ContainsKey(controllerKey))
        {
            return _modules[controllerKey].GetModel();
        }
        else
        {
            return null;
        }
    }
}
