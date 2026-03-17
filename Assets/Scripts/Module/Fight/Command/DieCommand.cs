using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DieCommand : BaseCommand
{
    float time = 0.7f;
    float count;
    public DieCommand(ModelBase model) : base(model)
    {
        this.model = model;
    }

    public override void Do()
    {
        base.Do();

        if (model == null || model.gameObject == null)
        {
            isFinish = true; // 直接标记指令完成
            return;          // 退出方法
        }

        GameApp.PlayerManager.isDead = true;
        GameApp.PlayerManager.GameStart = false;
        Debug.Log("Die");
        model.PlayAni("Die");
        
    }

    public override bool Update(float dt)
    {
        count += dt;
        if (count >= time)
        {
            GameApp.CommandManager.isStop = true;
            return true;
        }
        
        return false;
    }
}
