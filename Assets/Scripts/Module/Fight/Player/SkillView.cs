using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillView : BaseView
{
    private RectTransform[] skills;

    private Vector2[] endPos;

    private Vector2[] startPos;

    public override void InitData()
    {
        base.InitData();

        skills = new RectTransform[4];
        startPos = new Vector2[4];
        endPos = new Vector2[4];
        for (int i = 0; i < skills.Length; i++)
        {
            skills[i] = Find<RectTransform>($"skill{i + 1}");
            startPos[i] = skills[i].anchoredPosition;
            endPos[i] = skills[i].anchoredPosition*1.3333f;
        }
    }

    public override void Open(params object[] args)
    {
        base.Open(args);

        for(int i = 0; i< skills.Length;i++)
        {
            skills[i].anchoredPosition = startPos[i];
        }

        for (int i = 0; i < skills.Length; i++)
        {
            int idx = i;
            GameApp.TimerManager.Register(0.05f, () =>
            {
                skills[idx].DOAnchorPos(endPos[idx], 0.15f);
            });
        }

    }

    public override void Close(params object[] args)
    {
        for (int i = 0; i < skills.Length; i++)
        {
            int idx = i;
            GameApp.TimerManager.Register(0.05f, () =>
            {
                skills[idx].DOAnchorPos(startPos[idx], 0.1f);
            });
        }
        GameApp.TimerManager.Register(0.15f, () =>
        {
            SetVisible(false);
        });
    }

    protected override void OnStart()
    {
        base.OnStart();

        Find<Button>("skill1").onClick.AddListener(onSkill1);
        Find<Button>("skill2").onClick.AddListener(onSkill2);
        Find<Button>("skill3").onClick.AddListener(onSkill3);
    }

    private void onSkill1()
    {

    }

    private void onSkill2()
    {
        if (GameApp.PlayerManager.PlayerEnergy >= 5)
        {
            GameApp.MapManager.ShowStepGrid(GameApp.PlayerManager.Player, int.Parse(GameApp.PlayerManager.datas[1002]["Range"]));
            GameApp.ViewManager.Close(ViewId);
        }
        else
        {
            Find<Transform>("skill2").DOShakePosition(0.3f, 20f, 90);
        }
    }

    private void onSkill3()
    {
        if (GameApp.PlayerManager.PlayerEnergy >= 4)
        {
            GameApp.CommandManager.AddCommand(new HealCommand(GameApp.PlayerManager.Player));
            GameApp.ViewManager.Close(ViewId);
        }
        else
            Find<Transform>("skill3").DOShakePosition(0.3f, 20f, 90);
    }
}
