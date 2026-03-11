using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartView : BaseView
{
    protected override void OnStart()
    {
        base.OnStart();

        Find<Button>("StartBtn").onClick.AddListener(onStartBtn);
        Button continueBtn = Find<Button>("ContinueBtn");
        if (continueBtn != null)
        {
            continueBtn.onClick.AddListener(onContinueBtn);

            // 如果没有存档，将按钮置灰且不可点击
            if (!GameApp.SaveManager.HasSave())
            {
                continueBtn.interactable = false;
            }
            else
            {
                continueBtn.interactable = true;
            }
        }
        Find<Button>("SettingBtn").onClick.AddListener(onSettingBtn);
        Find<Button>("ExitBtn").onClick.AddListener(onExitBtn);
        Find<Button>("CreatorBtn").onClick.AddListener(onCreatorBtn);
    }

    private void onStartBtn()
    {
        GameApp.SoundManager.PlayEffect("gamestart", Camera.main.transform.position);
        GameApp.SoundManager.StopBGM();

        // 清空旧的存档数据
        if (GameApp.SaveManager.HasSave())
        {
            GameApp.SaveManager.ClearSave();
        }

        // 重置玩家状态
        GameApp.PlayerManager.hasSlash = false;
        GameApp.PlayerManager.hasLeg = false;
        GameApp.PlayerManager.hasArm = false;
        GameApp.PlayerManager.hasHeart = false;
        GameApp.PlayerManager.hasBody = true; // 护盾是初始技能
        GameApp.PlayerManager.PlayerHP = GameApp.PlayerManager.playerMaxHP;
        GameApp.PlayerManager.PlayerEnergy = GameApp.PlayerManager.playerMaxEnergy;

        //加载初始场景
        LoadSomeScene.LoadtheScene("Level 1", () =>
        {
            GameApp.ViewManager.Close(ViewType.LoadingView);
            Controller.ApplyControllerFunc(ControllerType.Fight, Defines.BeginFight);
        },
        () =>
        {
            GameApp.SoundManager.PlayBGM("music");
            GameApp.ViewManager.Open(ViewType.TipView, "Level 1");
            GameApp.ViewManager.Open(ViewType.PlayerDesView);
            GameApp.ViewManager.Close(ViewId);
            GameApp.PlayerManager.datas = GameApp.ConfigManager.GetConfigData("skill").GetLines();
        });
    }

    private void onContinueBtn()
    {
        // 再次确认是否有存档
        if (GameApp.SaveManager.HasSave())
        {
            GameApp.SoundManager.PlayEffect("gamestart", Camera.main.transform.position);
            GameApp.SoundManager.StopBGM();

            // 读取存档数据，并获取应该前往的关卡名称
            string levelToLoad = GameApp.SaveManager.LoadGame();

            // 加载目标关卡
            LoadSomeScene.LoadtheScene(levelToLoad, () =>
            {
                GameApp.ViewManager.Close(ViewType.LoadingView);
                Controller.ApplyControllerFunc(ControllerType.Fight, Defines.BeginFight);
            },
            () =>
            {
                GameApp.SoundManager.PlayBGM("music");
                GameApp.ViewManager.Open(ViewType.TipView, levelToLoad);
                GameApp.ViewManager.Open(ViewType.PlayerDesView);
                GameApp.ViewManager.Close(ViewId);
                GameApp.PlayerManager.datas = GameApp.ConfigManager.GetConfigData("skill").GetLines();
            });
        }
    }

    private void onSettingBtn()
    {
        GameApp.SoundManager.PlayEffect("confirm", Camera.main.transform.position);
        ApplyFunc(Defines.OpenSetView);
    }
    private void onExitBtn()
    {
        GameApp.SoundManager.PlayEffect("confirm", Camera.main.transform.position);
        Controller.ApplyFunc(Defines.OpenMessageView, new MessageInfo()
        {
            txt = "Quit Game?",
            okCallback = () =>
            {
                GameApp.SoundManager.PlayEffect("confirm", Camera.main.transform.position);
                Application.Quit();
            },
            noCallback= () => { GameApp.SoundManager.PlayEffect("cancel", Camera.main.transform.position); }
        });
    }

    private void onCreatorBtn()
    {
        GameApp.SoundManager.PlayEffect("confirm", Camera.main.transform.position);
        GameApp.ViewManager.Open(ViewType.CreatorView);
    }
}
