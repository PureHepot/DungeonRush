using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscView : BaseView
{
    public override void Open(params object[] args)
    {
        base.Open(args);
        GameApp.CommandManager.isStop = true;
    }

    public override void Close(params object[] args)
    {
        base.Close(args);
        
    }

    protected override void OnStart()
    {
        base.OnStart();

        Find<Button>("continueBtn").onClick.AddListener(onContinueBtn);
        Find<Button>("homeBtn").onClick.AddListener(onHomeBtn);
        Find<Button>("resetBtn").onClick.AddListener(onResetBtn);
    }


    private void onContinueBtn()
    {
        GameApp.CommandManager.isStop = false;
        GameApp.ViewManager.Close(ViewId);
        UserInputManager.escIsOpen = false;
    }

    public void onResetBtn()
    {
        UserInputManager.escIsOpen = false;

        GameApp.ViewManager.CloseAll();
        LoadSomeScene.LoadtheScene(SceneManager.GetActiveScene().name, () =>
        {
            GameApp.ViewManager.Close(ViewType.LoadingView);
            GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, Defines.BeginFight);
        },
        () =>
        {
            GameApp.ViewManager.Open(ViewType.TipView, SceneManager.GetActiveScene().name);
            GameApp.ViewManager.Open(ViewType.PlayerDesView);
            if (SceneManager.GetActiveScene().name == "Level 1")
                GameApp.PlayerManager.hasLeg = false;
            else if (SceneManager.GetActiveScene().name == "Level 2")
                GameApp.PlayerManager.hasHeart = false;
            else if (SceneManager.GetActiveScene().name == "Level 3")
                GameApp.PlayerManager.hasArm = false;
            GameApp.PlayerManager.datas = GameApp.ConfigManager.GetConfigData("skill").GetLines();
        });
        
    }

    private void onHomeBtn()
    {
        GameApp.CommandManager.isStop = true;
        LoadSomeScene.LoadtheScene("game", () =>{ },
        () =>
        {
            GameApp.ViewManager.CloseAll();
            GameApp.ViewManager.Open(ViewType.StartView);
        });
        UserInputManager.escIsOpen = false;
    }


}
