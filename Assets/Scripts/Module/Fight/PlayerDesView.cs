using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDesView : BaseView
{
    List<Image> lifeImgs;
    List<Image> energyImgs;

    Material lit2d;
    Material blink;

    
    private GameObject swordUIParent;
    private UnityEngine.UI.Image swordFillImage;

    public override void InitData()
    {
        base.InitData();
        lifeImgs = new List<Image>();
        energyImgs = new List<Image>();

        lit2d = Resources.Load<Material>("Materials/2dlit");
        blink = Resources.Load<Material>("Materials/blink");

        Image temp;
        
        for (int i = 1; i <= 6; i++)
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

        
        Transform swordTf = transform.Find("SwordSkillUI");

        if (swordTf != null)
        {
            swordUIParent = swordTf.gameObject;

            // 寻找它内部的填充层
            Transform fillTf = swordTf.Find("Sword_Fill");
            if (fillTf != null)
            {
                swordFillImage = fillTf.GetComponent<Image>();
            }

            // 游戏刚开始时，大剑 UI 默认隐藏
            swordUIParent.SetActive(false);
        }
    }

    protected override void OnStart()
    {
        base.OnStart();
    }

    public override void Open(params object[] args)
    {
        base.Open(args);
        foreach (var item in lifeImgs)
        {
            item.material = blink;
            item.enabled = false;
        }
        foreach (var item in energyImgs)
        {
            item.material = blink;
            item.enabled = false;
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
                GameApp.SoundManager.PlayEffect("playerhit", GameApp.PlayerManager.Player.transform.position);
                GameApp.TimerManager.Register(0.1f, () =>
                {
                    lifeImgs[idx].material = lit2d;
                });
            }
        }
        else
        {
            for (int i = lastHp - 1; i < currentHp; i++)
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

    

    // 解锁大剑UI并初始化
    public void UnlockSwordUI()
    {
        if (swordUIParent != null)
        {
            swordUIParent.SetActive(true);
            // 解锁时补满能量显示
            UpdateSwordEnergy(GameApp.PlayerManager.maxSlashEnergy, GameApp.PlayerManager.maxSlashEnergy);
        }
    }

    // 刷新大剑内部的颜色比例
    public void UpdateSwordEnergy(int currentEnergy, int maxEnergy)
    {
        if (swordFillImage != null)
        {
            // 通过修改 Fill Amount 达到颜色液面下降的效果
            swordFillImage.fillAmount = (float)currentEnergy / maxEnergy;
        }
    }
}
