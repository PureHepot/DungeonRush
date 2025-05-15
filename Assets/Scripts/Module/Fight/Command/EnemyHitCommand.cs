using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyHitCommand : BaseCommand
{
    public EnemyHitCommand(ModelBase model) : base(model)
    {
        this.model = model;
    }

    public override void Do()
    {
        base.Do();

        int offset_row = model.RowIndex - GameApp.PlayerManager.playerRow;
        int offset_col = model.ColIndex - GameApp.PlayerManager.playerCol;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(model.transform.DOMove(GameApp.MapManager.GetBlockPos(model.RowIndex + offset_row, model.ColIndex + offset_col)-new Vector3(offset_row*0.3f, offset_col*0.3f), 0.1f))
                .Append(model.transform.DOMove(GameApp.MapManager.GetBlockPos(model.RowIndex, model.ColIndex), 0.2f));
        model.PlayAni("Hit");
    }

    public override bool Update(float dt)
    {
        return true;
    }
}
