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
        
    }

    protected override void OnStart()
    {
        base.OnStart();

        Find<Button>("continueBtn").onClick.AddListener(onContinueBtn);
        Find<Button>("homeBtn").onClick.AddListener(onHomeBtn);
    }


    private void onContinueBtn()
    {
        GameApp.CommandManager.isStop = false;
        GameApp.ViewManager.Close(ViewId);
        UserInputManager.escIsOpen = false;
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
    }


}
