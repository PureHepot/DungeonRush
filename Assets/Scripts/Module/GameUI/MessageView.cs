using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageInfo
{
    public string txt;
    public string okBtntxt = "Yes";
    public string noBtntxt = "No";
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
        GameApp.CommandManager.isStop = true;
        info = args[0] as MessageInfo;
        Find<TextMeshProUGUI>("content/txt").text = info.txt;
        Find<Text>("okBtn/txt").text = info.okBtntxt;
        Find<Text>("noBtn/txt").text = info.noBtntxt;
    }
    public override void Close(params object[] args)
    {
        base.Close(args);
        GameApp.CommandManager.isStop = false;
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
