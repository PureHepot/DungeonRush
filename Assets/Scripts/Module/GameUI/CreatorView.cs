using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatorView : BaseView
{
    protected override void OnStart()
    {
        base.OnStart();

        Find<Button>("DownBtn").onClick.AddListener(onDownBtn);
    }

    private void onDownBtn()
    {
        GameApp.ViewManager.Close(ViewId);
    }


}
