using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 视图信息类
/// </summary>
public class ViewInfo
{
    public string PrefabName;//预制件名字
    public Transform parentTf;//父物体Transform组件
    public BaseController controller;//视图所属的控制器
    public int Sorting_Order;//显示层级
}


/// <summary>
/// 视图管理类
/// </summary>
public class ViewManager
{
    public Transform canvasTf;//画布组件
    public Transform worldCanvasTf;//世界画布组件
    Dictionary<int, IBaseView> _opens;//开启中的视图
    Dictionary<int, IBaseView> _viewCache;//视图缓存
    Dictionary<int, ViewInfo> _views;//注册的试图信息


    public ViewManager()
    {
        canvasTf = GameObject.Find("Canvas").transform;
        worldCanvasTf = GameObject.Find("WorldCanvas").transform;
        _opens = new Dictionary<int, IBaseView>();
        _viewCache = new Dictionary<int, IBaseView>();
        _views = new Dictionary<int, ViewInfo>();   
    }

    //注册视图信息
    public void Register(int key, ViewInfo viewInfo)
    {
        if (_views.ContainsKey(key) == false)
        {
            _views.Add(key, viewInfo);
        }
    }

    public void Register(ViewType viewType, ViewInfo viewInfo)
    {
        Register((int)viewType, viewInfo);
    }


    //移除视图信息
    public void Unregister(int key)
    {
        if (_views.ContainsKey(key))
        {
            _views.Remove(key);
        }
    }

    //移除面板
    public void RemoveView(int key)
    {
        _views.Remove(key);
        _viewCache.Remove(key);
        _opens.Remove(key);
    }

    //通过控制器来移除面板
    public void RemoveViewByController(BaseController ctl)
    {
        foreach (var item in _views)
        {
            if (item.Value.controller == ctl)
            {
                RemoveView(item.Key);
            }
        }
    }


    public bool IsOpen(int key)
    {
        return _opens.ContainsKey(key);
    }


    public IBaseView GetView(int key)
    {
        if (_opens.ContainsKey(key))
        {
            return _opens[key];
        }
        if (_viewCache.ContainsKey(key))
        {
            return _viewCache[key];
        }
        return null;
    }

    public T GetView<T>(int key) where T : class, IBaseView
    {
        IBaseView view = GetView(key);
        if (view != null)
        {
            return view as T;
        }
        return null;
    }

    public T GetView<T>(ViewType type) where T : class, IBaseView
    {
        return GetView<T>((int)type);
    }


    public void Destroy(int key)
    {
        IBaseView oldView = GetView(key);
        if (oldView != null)
        {
            Unregister(key);
            oldView.DestroyView();
            _viewCache.Remove(key);
        }
    }

    //关闭面板视图
    public void Close(ViewType viewType, params object[] args)
    {
        Close((int)viewType, args); 
    }
    public void Close(int key, params object[] args)
    {
        //如果没有打开
        if (IsOpen(key) == false)
        {
            return;
        }
        IBaseView view = GetView(key);
        if (view != null)
        {
            _opens.Remove(key);
            view.Close(args);
            _views[key].controller.CloseView(view);
        }
    }

    public void CloseAll()
    {
        List<IBaseView> list = _opens.Values.ToList();
        for (int i = list.Count - 1; i >= 0; i--)
        {
            Close(list[i].ViewId);
        }
    }

    //打开某个视图
    public void Open(ViewType viewType, params object[] args)
    {
        Open((int)viewType, args);
    }

    public void Open(int key, params object[] args)
    {
        IBaseView view = GetView(key);
        ViewInfo viewInfo = _views[key];
        if (view == null)
        {
            string type = ((ViewType)key).ToString();
            GameObject uiObj = UnityEngine.Object.Instantiate(Resources.Load($"Views/{viewInfo.PrefabName}"), viewInfo.parentTf)as GameObject;
            Canvas canvas = uiObj.GetComponent<Canvas>();
            if (canvas == null)
            {
                canvas = uiObj.AddComponent<Canvas>();
            }
            if (uiObj.GetComponent<GraphicRaycaster>() == null)
            {
                uiObj.AddComponent<GraphicRaycaster>();
            }
            canvas.overrideSorting = true; // 可以设定层级
            canvas.sortingOrder = viewInfo.Sorting_Order;//设置层级
            view = uiObj.AddComponent(Type.GetType(type)) as IBaseView;
            view.ViewId = key;
            view.Controller = viewInfo.controller;
            //添加到视图缓存中
            _viewCache.Add(key,view);
            viewInfo.controller.OnLoadView(view);
        }

        //如果已经打开了
        if (this._opens.ContainsKey(key))
        {
            return;
        }
        this._opens.Add(key, view);

        //没打开但是初始化了
        if (view.IsInit())
        {
            view.SetVisible(true);//显示
            view.Open(args);//打开
            viewInfo.controller.OpenView(view);
        }
        else//没初始化过
        {
            view.InitUI();
            view.InitData();
            view.Open(args);
            viewInfo.controller.OpenView(view);
        }
    }

    /// <summary>
    /// 显示伤害数字
    /// </summary>
    /// <param name="num"></param>
    /// <param name="color"></param>
    /// <param name="pos"></param>
    public void ShowHitNum(string num, Color color, Vector3 pos)
    {
        GameObject obj = UnityEngine.Object.Instantiate(Resources.Load("Views/HitNum"), worldCanvasTf) as GameObject;
        obj.transform.position = pos;
        obj.transform.DOMove(pos + Vector3.up * 1.75f, 0.65f).SetEase(Ease.OutBack);
        UnityEngine.Object.Destroy(obj, 0.75f);
        Text hitTxt = obj.GetComponent<Text>();
        hitTxt.text = num;
        hitTxt.color = color;
    }
}
