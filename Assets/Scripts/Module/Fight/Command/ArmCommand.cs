using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmCommand : BaseCommand
{
    List<Enemy> enemys;
    float count;
    float time = 0.5f;

    public ArmCommand(ModelBase model) : base(model)
    {
        this.model = model;
    }

    public override void Do()
    {
        base.Do();
        _BFS bfs = new _BFS(GameApp.MapManager.TotalRowCount, GameApp.MapManager.TotalColCount);
        List<_BFS.Point> temp =bfs.Search(model.RowIndex, model.ColIndex, GameApp.PlayerManager.ArmSkillStep);

        GameApp.MapManager.ShowStepGrid(model, 2, new Color(255f / 255f, 0f, 0f, 0.5f));
        enemys= new List<Enemy>();
        foreach (var point in temp)
        {
            Enemy t = GameApp.EnemyManager.GetEnemybyPos(point.RowIndex, point.ColIndex);
            if (t !=null)
                enemys.Add(t);
        }
        model.PlayAni("ArmSkill");
    }

    public override bool Update(float dt)
    {
        count += dt;
        if (count > time)
        {
            GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, Defines.OnPlayerEnergyChange, -5);
            GameApp.MapManager.HideStepGrid(model, 2);

            foreach (var enemy in enemys)
            {
                enemy.EnemyBeAttacked(1);
            }
            return true;
        }
        

        return false;
    }
}
