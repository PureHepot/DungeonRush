using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyDeadCommand : BaseCommand
{
    public EnemyDeadCommand(ModelBase model) : base(model)
    {
        this.model = model;
    }

    public override void Do()
    {
        base.Do();
        Sequence sequence = DOTween.Sequence();
        model.StopAni();
        sequence.Append(model.transform.DOShakePosition(0.2f, 0.5f, 90))
            .Append(model.transform.DOLocalRotate(new Vector3(0, 0, -90f), 0.2f))
            .Append(model.transform.DOScale(new Vector3(0.001f, 0.001f, 0.001f), 0.1f));
        GameApp.SoundManager.PlayEffect("enemydead", Camera.main.transform.position);
        //怪物死亡时，奖励玩家一定的斩击能量
        GameApp.PlayerManager.AddSlashEnergy(15);

    }
    public override bool Update(float dt)
    {
        return true;
    }
}
