using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseView : MonoBehaviour, IBaseView
{
    public int ViewId { get; set; }
    public BaseController Controller { get; set; }

    protected Canvas _canvas;

    protected Dictionary<string, GameObject> m_cache_gos = new Dictionary<string, GameObject>();//缓存物体的字典

    private bool _isInit = false;//是否初始化

    private void Awake()
    {
        OnAwake();
    }

    private void Start()
    {
        _canvas = GetComponent<Canvas>();

        OnStart();
    }

    private void Update()
    {
        
    }

    protected virtual void OnAwake()
    {

    }

    protected virtual void OnStart()
    {

    }

    protected virtual void OnUpdate()
    {

    }

    public void ApplyControllerFunc(int controllerKey, string eventName, params object[] args)
    {
        this.Controller.ApplyControllerFunc(controllerKey, eventName, args);
    }

    public void ApplyFunc(string eventName, params object[] args)
    {
        this.Controller.ApplyFunc(eventName, args);
    }

    public virtual void Close(params object[] args)
    {
        SetVisible(false);
    }

    public void DestroyView()
    {
        Controller = null;
        Destroy(gameObject);
    }

    public virtual void InitData()
    {
        _isInit = true;

    }

    public void InitUI()
    {
        
    }

    public bool IsInit()
    {
        return _isInit;
    }

    public bool IsShow()
    {
        return _canvas.enabled == true;
    }

    public virtual void Open(params object[] args)
    {
        
    }

    public void SetVisible(bool value)
    {
        this._canvas.enabled = value;
    }


    public GameObject Find(string res)
    {
        if (m_cache_gos.ContainsKey(res))
        {
            return m_cache_gos[res];
        }
        if (transform.Find(res).gameObject == null)
        {
            return null;
        }
        m_cache_gos.Add(res,transform.Find(res).gameObject);//只能找到第一层的物体
        return m_cache_gos[res] as GameObject;
    }

    public T Find<T>(string res) where T : Component
    {
        GameObject obj = Find(res);
        if (obj == null)
            return null;
        return obj.GetComponent<T>();
    }
}
