using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 命令管理器
/// </summary>
public class CommandManager
{
    private Queue<BaseCommand> willDoCommandQueue;//将要执行的命令队列
    private Stack<BaseCommand> unDoStack;// 撤销命令 栈
    public BaseCommand current;//当前

    public CommandManager()
    {
        willDoCommandQueue = new Queue<BaseCommand>();
        unDoStack = new Stack<BaseCommand>();
    }

    public bool isRunningCommand
    {
        get
        {
            return current != null;
        }
    }

    public void AddCommand(BaseCommand command)
    {
        willDoCommandQueue.Enqueue(command);
        unDoStack.Push(command);
    }

    public void Update(float dt)
    {
        if (current == null)
        {
            if (willDoCommandQueue.Count > 0)
            {
                current = willDoCommandQueue.Dequeue();
                current.Do();//执行
            }
        }
        else
        {
            if (current.Update(dt) == true)
            {
                current = null;
            }
        }
    }

    public void Clear()
    {
        willDoCommandQueue?.Clear();
        unDoStack.Clear();
        current = null;
    }

    //撤销上一个命令
    public void Undo()
    {
        if (unDoStack.Count > 0)
        {
            unDoStack.Pop().Undo();
        }
    }

}


