using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDesView : BaseView
{
    List<Image> lifeImgs; 
    List<Image> energyImgs;

    Material lit2d;
    Material blink;
    public override void InitData()
    {
        base.InitData();

    }

    protected override void OnStart()
    {
        base.OnStart();
        lifeImgs = new List<Image>();
        energyImgs = new List<Image>();

        lit2d = Resources.Load<Material>("Materials/2dlit");
        blink = Resources.Load<Material>("Materials/blink");

        Image temp;
        for(int i = 1;i<=6;i++)
        {
            temp = Find<Image>($"heartsBG/life/imge{i}");
            temp.enabled = false;
            lifeImgs.Add(temp);
        }
        for (int i = 1; i <= 5; i++)
        {
            temp = Find<Image>($"heartsBG/energy/imge{i}");
            temp.enabled = false;
            energyImgs.Add(temp);
        }
    }

    public void ChangeHealth(int count)
    {
        int lastHp = GameApp.PlayerManager.PlayerHP;
        if (lastHp <= 0) return;
        GameApp.PlayerManager.PlayerHP += count;
        int currentHp = GameApp.PlayerManager.PlayerHP;
        if (lastHp > currentHp) 
        {
            for (int i = currentHp; i < lastHp; i++)
            {
                int idx = i;
                lifeImgs[idx].enabled = true;
                GameApp.CameraManager.CameraShake();
                GameApp.PlayerManager.Player.PlayAni("Hit");
                GameApp.TimerManager.Register(0.1f, () =>
                {
                    lifeImgs[idx].material = lit2d;
                });
            }
        }
        else
        {
            for (int i = lastHp-1; i < currentHp; i++)
            {
                lifeImgs[i].enabled = false;
                lifeImgs[i].material = blink;
            }
        }
    }

    public void ChangeEnergy(int count)
    {
        int lastEn = GameApp.PlayerManager.PlayerEnergy;
        GameApp.PlayerManager.PlayerEnergy += count;
        int currentEn = GameApp.PlayerManager.PlayerEnergy;
        if (lastEn > currentEn)
        {
            for (int i = currentEn; i < lastEn; i++)
            {
                int idx = i;
                energyImgs[idx].enabled = true;
                
                GameApp.TimerManager.Register(0.1f, () =>
                {
                    energyImgs[idx].material = lit2d;
                });
            }
        }
        else
        {
            for (int i = lastEn; i < currentEn; i++)
            {
                energyImgs[i].enabled = false;
                energyImgs[i].material = blink;
            }
        }
    }
}
