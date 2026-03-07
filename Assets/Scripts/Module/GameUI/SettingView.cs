using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingView : BaseView
{

    private float lastVolume = 1.0f;
    private bool flag1 = true;
    private bool flag2 = true;
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

        Find<Button>("BgmVolume/VolumeBtn").onClick.AddListener(onStop1Btn);
        Find<Button>("EffectVolume/VolumeBtn").onClick.AddListener(onStop2Btn);
        Find<Slider>("BgmVolume/VolumeSlider").onValueChanged.AddListener(onChangeBgmVolumeSlider);
        Find<Slider>("EffectVolume/VolumeSlider").onValueChanged.AddListener(onChangeEffectVolumeSlider);
        Find<Button>("CloseBtn").onClick.AddListener(onCloseBtn);
    }

    private void onStop1Btn()
    {
        if (flag1)
        {
            lastVolume = GameApp.SoundManager.BgmVolume;
            GameApp.SoundManager.BgmVolume = 0;
            Find<Image>("BgmVolume/VolumeBtn").sprite = atlasSprite;
            Find<Slider>("BgmVolume/VolumeSlider").value = 0;
            flag1 = false;
        }
        else
        {
            GameApp.SoundManager.BgmVolume = lastVolume;
            Find<Image>("BgmVolume/VolumeBtn").sprite = lastSprite;
            Find<Slider>("BgmVolume/VolumeSlider").value = lastVolume;
            flag1 = true;
        }
    }

    private void onStop2Btn()
    {
        if (flag2)
        {
            lastVolume = GameApp.SoundManager.BgmVolume;
            GameApp.SoundManager.EffectVolume = 0;
            Find<Image>("BgmVolume/VolumeBtn").sprite = atlasSprite;
            Find<Slider>("BgmVolume/VolumeSlider").value = 0;
            flag2 = false;
        }
        else
        {
            GameApp.SoundManager.EffectVolume = lastVolume;
            Find<Image>("BgmVolume/VolumeBtn").sprite = lastSprite;
            Find<Slider>("BgmVolume/VolumeSlider").value = lastVolume;
            flag2 = true;
        }
    }

    private void onChangeBgmVolumeSlider(float value)
    {

        GameApp.SoundManager.BgmVolume = value;
        if (value != 0)
        {
            if (!flag1)
                flag1 = true;
            Find<Image>("BgmVolume/VolumeBtn").sprite = lastSprite;
        }
    }

    private void onChangeEffectVolumeSlider(float value)
    {

        GameApp.SoundManager.EffectVolume = value;
        if (value != 0)
        {
            if (!flag2)
                flag2 = true;
            Find<Image>("EffectVolume/VolumeBtn").sprite = lastSprite;
        }
    }
    private void onCloseBtn()
    {
        GameApp.ViewManager.Close(ViewId);
        GameApp.SoundManager.PlayEffect("cancel", Camera.main.transform.position);
    }
}
