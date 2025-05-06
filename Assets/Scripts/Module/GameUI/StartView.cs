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
        LoadSomeScene.LoadtheScene(Controller,"dungeon", () =>
        {
            GameApp.ViewManager.Close(ViewType.LoadingView);
            Controller.ApplyControllerFunc(ControllerType.Fight, Defines.BeginFight);
        },
        () =>
        {
            GameApp.ViewManager.Open(ViewType.PlayerDesView);
            GameApp.ViewManager.Close(ViewId);
            GameApp.PlayerManager.datas = GameApp.ConfigManager.GetConfigData("skill").GetLines();
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
                Application.Quit();
            }
        });
    }
}
