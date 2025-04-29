using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour 
{
    float dt;

    public Texture2D mouseTxt;//图标图片

    private static bool isLoaded = false;

    private void Awake()
    {
        if (isLoaded)
        {
            Destroy(gameObject);
        }
        else
        {
            isLoaded = true;
            DontDestroyOnLoad(gameObject);
            GameApp.Instance.Init();
        }
    }

    private void Start()
    {
        //Cursor.SetCursor(mouseTxt, Vector2.zero, CursorMode.Auto);

        RegisterConfigs();

        //GameApp.ConfigManager.LoadAllConfigs();

        //GameApp.SoundManager.PlayBGM("login");

        RegisterModule();//注册游戏中的控制器

        InitModule();
    }

    //注册控制器
    void RegisterModule()
    {
        GameApp.ControllerManager.Register(ControllerType.Game, new GameController());
        GameApp.ControllerManager.Register(ControllerType.GameUI, new GameUIController());

    }

    //执行所有控制器初始化
    void InitModule()
    {
        GameApp.ControllerManager.InitAllModules();
    }

    void RegisterConfigs()
    {
        
    }

    private void Update()
    {
        dt = Time.deltaTime;
        GameApp.Instance.Update(dt);
    }
}
