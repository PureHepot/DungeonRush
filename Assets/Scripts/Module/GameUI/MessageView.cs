using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageInfo
{
    public string txt;
    public System.Action okCallback;
    public System.Action noCallback;
}

public class MessageView : BaseView
{
    private MessageInfo info;

    protected override void OnAwake()
    {
        base.OnAwake();

        Find<Button>("okBtn").onClick.AddListener(onOkBtn);
        Find<Button>("noBtn").onClick.AddListener(onNoBtn);
    }

    public override void Open(params object[] args)
    {
        info = args[0] as MessageInfo;
        Find<Text>("content/txt").text = info.txt;
    }

    private void onOkBtn()
    {
        info.okCallback?.Invoke();
    }

    private void onNoBtn()
    {
        info?.noCallback?.Invoke();
        GameApp.ViewManager.Close(ViewId);
    }
}
