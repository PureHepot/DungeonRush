using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//加载场景的快捷使用方式
public static class LoadSomeScene
{

    public static void LoadtheScene(BaseController controller, int viewId, string sceneName, System.Action action)
    {
        LoadingModel loadingmodel = new LoadingModel();
        loadingmodel.SceneName = sceneName;
        loadingmodel.callback = action;
        loadingmodel.viewId = viewId;
        GameApp.ViewManager.Open(ViewType.LoadingView);
        GameApp.ViewManager.GetView<LoadingView>((int)ViewType.LoadingView).Move2Center(() =>
        {
            GameApp.ViewManager.Close(viewId);
            controller.ApplyControllerFunc(ControllerType.Loading, Defines.LoadingScene, loadingmodel);
        });
        
    }
}

public class LoadingController : BaseController
{
    AsyncOperation asyncOperation;

    public LoadingController() : base()
    {
        GameApp.ViewManager.Register(ViewType.LoadingView, new ViewInfo()
        {
            PrefabName = "LoadingView",
            controller = this,
            parentTf = GameApp.ViewManager.canvasTf,
            Sorting_Order = 998      
        });

        InitModuleEvent();
    }

    public override void InitModuleEvent()
    {
        base.InitModuleEvent();
        RegisterFunc(Defines.LoadingScene, loadSceneCallback);
    }

    //加载场景回调
    private void loadSceneCallback(object[] args)
    {
        LoadingModel loadingModel = args[0] as LoadingModel;

        SetModel(loadingModel);

        asyncOperation = SceneManager.LoadSceneAsync(loadingModel.SceneName);
        asyncOperation.completed += onLoadEndCallback;
    }

    //加载后回调
    private void onLoadEndCallback(AsyncOperation operation)
    {
        asyncOperation.completed -= onLoadEndCallback;

        GameApp.ViewManager.GetView<LoadingView>((int)ViewType.LoadingView).Move2Left();

        GameApp.TimerManager.Register(1f, () =>
        {
            GetModel<LoadingModel>().callback?.Invoke();

            GameApp.ViewManager.Close(ViewType.LoadingView);
        });


    }
}
