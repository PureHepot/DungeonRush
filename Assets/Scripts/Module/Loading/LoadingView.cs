using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingView : BaseView
{
    private RectTransform circle;
    public override void Open(params object[] args)
    {
        base.Open(args);
        circle.anchoredPosition = new Vector2(2800, 0);
    }
    protected override void OnAwake()
    {
        base.OnAwake();
        circle = Find<RectTransform>("Circle");
    }
    public void Move2Center(System.Action callback)
    {
        GameApp.SoundManager.PlayEffect("slashin", Camera.main.transform.position);
        circle.DOAnchorPosX(0, 0.5f).OnComplete(() =>
        {
            callback?.Invoke();
        });
    }
    public void Move2Left(params object[] args)
    {
        circle.DOAnchorPosX(-2800, 0.5f);
        GameApp.SoundManager.PlayEffect("slashout", Camera.main.transform.position);
    }


}
