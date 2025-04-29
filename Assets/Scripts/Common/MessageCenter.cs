using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 消息处理脚本
/// </summary>
public class MessageCenter
{
    private Dictionary<string, System.Action<object>> msgDic; //消息字典
    private Dictionary<string, System.Action<object>> tempMsgDic;//存储临时的消息字典，施行后消除
    private Dictionary<object, Dictionary<string, System.Action<object>>> objMsgDic;//存储特定对象的消息字典

    public MessageCenter()
    {
        msgDic = new Dictionary<string, System.Action<object>>();
        tempMsgDic = new Dictionary<string, System.Action<object>>();
        objMsgDic = new Dictionary<object, Dictionary<string, System.Action<object>>>();
    }

    public void AddEvent(string eventName, System.Action<object> callback)
    {
        if (msgDic.ContainsKey(eventName))
        {
            msgDic[eventName] += callback;
        }
        else
        {
            msgDic.Add(eventName, callback);
        }
    }

    //移除事件
    public void RemoveEvent(string eventName, System.Action<object> callback)
    {
        if (msgDic.ContainsKey(eventName))
        {
            msgDic[eventName] -= callback;
            if (msgDic[eventName] == null)
            {
                msgDic.Remove(eventName);
            }
        }
    }

    //执行事件
    public void PostEvent(string eventName, object arg = null)
    {
        if (msgDic.ContainsKey(eventName))
        {
            msgDic[eventName].Invoke(arg);
        }
    }

    //添加事件
    public void AddEvent(object listenerObj, string eventName, System.Action<object> callback)
    {
        if (objMsgDic.ContainsKey(listenerObj))
        {
            if (objMsgDic[listenerObj].ContainsKey(eventName))
            {
                objMsgDic[listenerObj][eventName] += callback;
            }
            else
            {
                objMsgDic[listenerObj].Add(eventName, callback);
            }
        }
        else
        {
            Dictionary<string, System.Action<object>> _tempDic = new Dictionary<string, System.Action<object>>();
            _tempDic.Add(eventName, callback);
            objMsgDic.Add(listenerObj, _tempDic);
        }
    }

    //移出对象事件
    public void RemoveEvent(object listenerObj, string eventName, System.Action<object> callback)
    {
        if (objMsgDic.ContainsKey(listenerObj))
        {
            if (objMsgDic[listenerObj].ContainsKey(eventName))
            {
                objMsgDic[listenerObj][eventName] -= callback;
                if (objMsgDic[listenerObj][eventName] == null)
                {
                    objMsgDic[listenerObj].Remove(eventName);
                    if (objMsgDic[listenerObj].Count == 0)
                    {
                        objMsgDic.Remove(listenerObj);
                    }
                }
            }

        }
    }

    //移除对象的所有事件
    public void RemoveObjAllEvent(object listenerObj)
    {
        if (objMsgDic.ContainsKey(listenerObj))
        {
            objMsgDic.Remove(listenerObj);
        }
    }

    //执行对象的监听事件
    public void PostEvent(object listenerObj, string eventName, object arg = null)
    {
        if (objMsgDic.ContainsKey(listenerObj))
        {
            if (objMsgDic[listenerObj].ContainsKey(eventName))
            {
                objMsgDic[listenerObj][eventName].Invoke(arg);
            }
        }
    }

    public void AddTempEvent(string eventName, System.Action<object> callback)
    {
        if (tempMsgDic.ContainsKey(eventName))
        {
            tempMsgDic[eventName] = callback;
        }
        else
        {
            tempMsgDic.Add(eventName, callback);
        }
    }

    public void PostTempEvent(string eventName, object arg = null)
    {
        if (tempMsgDic.ContainsKey(eventName))
        {
            tempMsgDic[eventName].Invoke(arg);
            tempMsgDic[eventName] = null;
            tempMsgDic.Remove(eventName);
        }
    }
}



