using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 声音管理器
/// </summary>
public class SoundManager
{
    private AudioSource bgmSource;//播放bgm的音频组件

    private Dictionary<string, AudioClip> clips;//音频缓存字典

    private bool isStop;//静音

    public bool IsStop
    {
        get
        {
            return isStop;
        }
        set
        {
            isStop  = value;
            if (isStop == true)
            {
                bgmSource.Pause();
            }
            else
            {
                bgmSource.Play();
            }
        }
    }

    private float bgmVolume;//bgm音量大小

    public float BgmVolume
    {
        get
        {
            return bgmVolume;
        }
        set
        {
            bgmVolume = value;
            bgmSource.volume = bgmVolume;
        }
    }

    private float effectVolume;//音效大小

    public float EffectVolume
    {
        get
        {
            return effectVolume;
        }
        set
        {
            effectVolume = value;
        }
    }


    public SoundManager()
    {
        bgmSource = GameObject.Find("game").GetComponent<AudioSource>();
        clips = new Dictionary<string, AudioClip>();

        IsStop = false;
        bgmVolume = 1f;
        effectVolume = 0.7f;
    }

    public void PlayBGM(string res)
    {
        if (isStop)
        {
            return;
        }
        if (clips.ContainsKey(res) == false)
        {
            //加载音频
            AudioClip clip = Resources.Load<AudioClip>($"Sounds/{res}");
            clips.Add(res, clip);
        }
        bgmSource.clip = clips[res];
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource?.Stop();
    }

    public void PlayEffect(string name, Vector3 pos)
    {
        if (isStop == true)
        {
            return;
        }
        AudioClip clip = null;
        if (clips.ContainsKey(name) == false)
        {
            clip = Resources.Load<AudioClip>($"Sounds/{name}");
            clips.Add(name, clip);
        }
        AudioSource.PlayClipAtPoint(clips[name], pos);

    }
}
