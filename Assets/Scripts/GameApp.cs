using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 统一定义游戏中的管理器，在此类中进行初始化
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
