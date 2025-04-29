using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartView : BaseView
{
    protected override void OnStart()
    {
        base.OnStart();

        Find<Button>("StartBtn").onClick.AddListener(onStartBtn);
        Find<Button>("SettingBtn").onClick.AddListener(onSettingBtn);
        Find<Button>("ExitBtn").onClick.AddListener(onExitBtn);
    }

    private void onStartBtn()
    {

    }
    private void onSettingBtn()
    {
        GameApp.ViewManager.Close(ViewId);
        GameApp.ViewManager.Open(ViewType.SettingView);
    }
    private void onExitBtn()
    {

    }
}
