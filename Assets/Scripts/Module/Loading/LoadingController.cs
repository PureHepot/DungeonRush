using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//加载场景的快捷使用方式
public static class LoadSomeScene
{
    /// <summary>
    /// 加载某个场景
    /// </summary>
    /// <param name="controller">任意控制器引用</param>
    /// <param name="sceneName">要加载的场景名称</param>
    /// <param name="action1">加载完成后做的事</param>
    /// <param name="action2">加载中做的事</param>
    public static void LoadtheScene(string sceneName, System.Action action1, System.Action action2 = null)
    {
        BaseController controller = GameApp.ControllerManager.GetControllerbyKey(ControllerType.Game);
        LoadingModel loadingmodel = new LoadingModel();
        loadingmodel.SceneName = sceneName;
        loadingmodel.callback = action1;
        GameApp.ViewManager.Open(ViewType.LoadingView);
        GameApp.ViewManager.GetView<LoadingView>((int)ViewType.LoadingView).Move2Center(() =>
        {
            action2?.Invoke();
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

        GameApp.TimerManager.Register(0.5f, () =>
        {
            GetModel<LoadingModel>().callback?.Invoke();

            GameApp.ViewManager.Close(ViewType.LoadingView);
        });


    }
}
