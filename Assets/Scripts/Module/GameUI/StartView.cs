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
        LoadSomeScene.LoadtheScene(Controller,this.ViewId, "dungeon", () =>
        {
            GameApp.ViewManager.GetView<LoadingView>((int)ViewType.LoadingView).Move2Left();
            GameApp.TimerManager.Register(1f, () =>
            {
                GameApp.ViewManager.Close(ViewType.LoadingView);
            });
        });
    }
    private void onSettingBtn()
    {
        ApplyFunc(Defines.OpenSetView);
    }
    private void onExitBtn()
    {
        Controller.ApplyFunc(Defines.OpenMessageView, new MessageInfo()
        {
            txt = "Quit Game?",
            okCallback = () =>
            {

            }
        });
    }
}
