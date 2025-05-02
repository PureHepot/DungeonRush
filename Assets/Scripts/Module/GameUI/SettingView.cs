using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingView : BaseView
{

    private float lastVolume = 1.0f;
    private bool flag = true;
    private Sprite lastSprite;
    private Sprite atlasSprite;
    protected override void OnStart()
    {
        base.OnStart();

        Sprite[] slices = Resources.LoadAll<Sprite>("Arts/UI Pixels/Blue/Sprites/Sprite-0001");
        foreach (Sprite s in slices)
        {
            if (s.name == "Sprite-0001_6")
            {
                lastSprite = s;
            }
            else if (s.name == "Sprite-0001_7")
            {
                atlasSprite = s;
            }

        }

        Find<Button>("BgmVolume/VolumeBtn").onClick.AddListener(onStopBtn);
        Find<Slider>("BgmVolume/VolumeSlider").onValueChanged.AddListener(onChangeVolumeSlider);
        Find<Button>("CloseBtn").onClick.AddListener(onCloseBtn);
    }

    private void onStopBtn()
    {
        if (flag)
        {
            lastVolume = GameApp.SoundManager.BgmVolume;
            GameApp.SoundManager.BgmVolume = 0;
            GameApp.SoundManager.EffectVolume = 0;
            Find<Image>("BgmVolume/VolumeBtn").sprite = atlasSprite;
            Find<Slider>("BgmVolume/VolumeSlider").value = 0;
            flag = false;
        }
        else
        {
            GameApp.SoundManager.BgmVolume = lastVolume;
            GameApp.SoundManager.EffectVolume = lastVolume;
            Find<Image>("BgmVolume/VolumeBtn").sprite = lastSprite;
            Find<Slider>("BgmVolume/VolumeSlider").value = lastVolume;
            flag = true;
        }
    }
    private void onChangeVolumeSlider(float value)
    {
        GameApp.SoundManager.BgmVolume = value;
        GameApp.SoundManager.EffectVolume = value;
        if (value != 0)
        {
            if (!flag)
                flag = true;
            Find<Image>("BgmVolume/VolumeBtn").sprite = lastSprite;
        }
    }
    private void onCloseBtn()
    {
        GameApp.ViewManager.Close(ViewId);
    }
}
