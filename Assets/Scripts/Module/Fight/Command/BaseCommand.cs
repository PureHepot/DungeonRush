using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 命令基类（可以派生 移动 使用技能等）
/// </summary>
public class BaseCommand
{
    public ModelBase model;//命令对象
    protected bool isFinish;//是否做完的标签

    public BaseCommand()
    {

    }
    public BaseCommand(ModelBase model)
    {
        this.model = model;
        isFinish = false;
    }

    public virtual bool Update(float dt)
    {
        return isFinish;
    }

    //执行
    public virtual void Do()
    {

    }

    //不执行
    public virtual void Undo()
    {

    }

}
