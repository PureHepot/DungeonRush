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
        GameApp.SoundManager.PlayEffect("gamestart", Camera.main.transform.position);
        GameApp.SoundManager.StopBGM();
        LoadSomeScene.LoadtheScene("Tutorial", () =>
        {
            GameApp.ViewManager.Close(ViewType.LoadingView);
            Controller.ApplyControllerFunc(ControllerType.Fight, Defines.BeginFight);
        },
        () =>
        {
            GameApp.SoundManager.PlayBGM("music");
            GameApp.ViewManager.Open(ViewType.TipView, "Tutorial");
            GameApp.ViewManager.Open(ViewType.PlayerDesView);
            GameApp.ViewManager.Close(ViewId);
            GameApp.PlayerManager.datas = GameApp.ConfigManager.GetConfigData("skill").GetLines();
        });
    }
    private void onSettingBtn()
    {
        GameApp.SoundManager.PlayEffect("confirm", Camera.main.transform.position);
        ApplyFunc(Defines.OpenSetView);
    }
    private void onExitBtn()
    {
        GameApp.SoundManager.PlayEffect("confirm", Camera.main.transform.position);
        Controller.ApplyFunc(Defines.OpenMessageView, new MessageInfo()
        {
            txt = "Quit Game?",
            okCallback = () =>
            {
                GameApp.SoundManager.PlayEffect("confirm", Camera.main.transform.position);
                Application.Quit();
            },
            noCallback= () => { GameApp.SoundManager.PlayEffect("cancel", Camera.main.transform.position); }
        });
    }
}
