using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ͳһ������Ϸ�еĹ��������ڴ����н��г�ʼ��
/// </summary>
public class GameApp : Singleton<GameApp>
{
    public static SoundManager SoundManager;
    public static ControllerManager ControllerManager;
    public static ViewManager ViewManager;
    public static ConfigManager ConfigManager;
    public static CameraManager CameraManager;
    public static MessageCenter MessageCenter;
    public static TimerManager TimerManager;
    public static UserInputManager UserInputManager;
    public static MapManager MapManager;
    public static CommandManager CommandManager;
    public static EnemyManager EnemyManager;

    public override void Init()
    {
        SoundManager = new SoundManager();
        ControllerManager = new ControllerManager();
        ViewManager = new ViewManager();
        ConfigManager = new ConfigManager();
        CameraManager = new CameraManager();
        MessageCenter = new MessageCenter();
        TimerManager = new TimerManager();
        UserInputManager = new UserInputManager();
        MapManager = new MapManager();
        CommandManager = new CommandManager();
        EnemyManager = new EnemyManager();
    }

    public override void Update(float dt)
    {
        UserInputManager.Update();
        TimerManager.OnUpdate(dt);
        CommandManager.Update(dt);
        EnemyManager.Update(dt);
    }
}
