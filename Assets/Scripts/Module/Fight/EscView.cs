using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        GameApp.CommandManager.isStop = false;
    }

    protected override void OnStart()
    {
        base.OnStart();

        Find<Button>("continueBtn").onClick.AddListener(onContinueBtn);
        Find<Button>("homeBtn").onClick.AddListener(onHomeBtn);
    }


    private void onContinueBtn()
    {
        GameApp.ViewManager.Close(ViewId);
        UserInputManager.escIsOpen = false;
    }

    private void onHomeBtn()
    {
        LoadSomeScene.LoadtheScene(Controller,"game", () =>{ },
        () =>
        {
            GameApp.ViewManager.CloseAll();
            GameApp.ViewManager.Open(ViewType.StartView);
        });
    }


}
