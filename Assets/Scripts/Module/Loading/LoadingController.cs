using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//���س����Ŀ��ʹ�÷�ʽ
public static class LoadSomeScene
{
    /// <summary>
    /// ����ĳ������
    /// </summary>
    /// <param name="controller">�������������</param>
    /// <param name="sceneName">Ҫ���صĳ�������</param>
    /// <param name="action1">������ɺ�������</param>
    /// <param name="action2">������������</param>
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

    //���س����ص�
    private void loadSceneCallback(object[] args)
    {
        LoadingModel loadingModel = args[0] as LoadingModel;

        SetModel(loadingModel);

        asyncOperation = SceneManager.LoadSceneAsync(loadingModel.SceneName);
        asyncOperation.completed += onLoadEndCallback;
    }

    //���غ�ص�
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
