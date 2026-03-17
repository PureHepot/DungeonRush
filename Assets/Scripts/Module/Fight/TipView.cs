using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipView : BaseView
{
    public override void Open(params object[] args)
    {
        base.Open(args);

        // 获取传进来的关卡名
        string levelName = args[0].ToString();

        // 设置主标题
        Text mainTxt = Find<Text>("content/txt");
        if (mainTxt != null)
        {
            mainTxt.text = levelName;
        }

        //设置副标题
        Text subTxt = Find<Text>("content/Subtxt");
        if (subTxt != null)
        {
            subTxt.text = GetSubtitleByLevelName(levelName);
        }

        
        Sequence seq = DOTween.Sequence();
        seq.Append(Find("content").transform.DOScaleY(1, 0.15f)).SetEase(Ease.OutBack);

        // 专场时长
        seq.AppendInterval(0.75f);

        seq.Append(Find("content").transform.DOScaleY(0, 0.15f)).SetEase(Ease.Linear);
        seq.AppendCallback(() =>
        {
            GameApp.ViewManager.Close(ViewId);
        });
    }

    //设置副标题
    private string GetSubtitleByLevelName(string levelName)
    {
        switch (levelName)
        {
            case "Level 1":
                return "分辨虚实";
            case "Level 2":
                return "保持移动";
            case "Level 3":
                return "相信自己";
            case "Tutorial":
                return "这是一场试炼";
            case "Level 4":
                return "是时候了";
            case "Level 5":
                return "快结束了";
            case "Level 6":
                return "终局";
            case "DIE":
                return "黑暗吞噬了你...";
            default:
                // 其他提示词
                return "";
        }
    }
}
