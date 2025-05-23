using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour 
{
    float dt;

    public Texture2D mouseTxt;//ͼ��ͼƬ

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

        GameApp.ConfigManager.LoadAllConfigs();

        GameApp.SoundManager.PlayBGM("music");

        RegisterModule();//ע����Ϸ�еĿ�����

        InitModule();
    }

    //ע�������
    void RegisterModule()
    {
        GameApp.ControllerManager.Register(ControllerType.Game, new GameController());
        GameApp.ControllerManager.Register(ControllerType.GameUI, new GameUIController());
        GameApp.ControllerManager.Register(ControllerType.Loading, new LoadingController());
        GameApp.ControllerManager.Register(ControllerType.Fight, new FightController());
    }

    //ִ�����п�������ʼ��
    void InitModule()
    {
        GameApp.ControllerManager.InitAllModules();
    }

    void RegisterConfigs()
    {
        GameApp.ConfigManager.Register("skill", new ConfigData("skill"));
        GameApp.ConfigManager.Register("enemy", new ConfigData("enemy"));
    }

    private void Update()
    {
        dt = Time.deltaTime;
        GameApp.Instance.Update(dt);
    }
}
