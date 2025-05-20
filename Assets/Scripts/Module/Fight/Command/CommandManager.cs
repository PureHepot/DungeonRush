using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���������
/// </summary>
public class CommandManager
{
    public bool isStop;

    private Queue<BaseCommand> willDoCommand;//��Ҫִ�е��������
    private Stack<BaseCommand> unDoStack;// �������� ջ
    public BaseCommand current;//��ǰ

    private float roundTime1 = 0.4f;
    private float roundTime2 = 0.7f;
    private float counter;

    public CommandManager()
    {
        willDoCommand = new Queue<BaseCommand>();
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
        if(willDoCommand.Count <1)
            willDoCommand.Enqueue(command);
    }

    public void FallingCommand()
    {
        willDoCommand.Enqueue(new FallingCommand(GameApp.PlayerManager.Player));
    }

    public void Update(float dt)
    {
        counter += dt;
        if (current == null)
        {
            if (counter >= roundTime1 && !isStop)
            {
                if (willDoCommand.Count>0)
                {
                    current = willDoCommand.Dequeue();
                    current.Do();//ִ��

                    GameApp.EnemyManager.GenerateEnemyCommand();
                    GameApp.MapManager.SpecialBlockEvent();
                    counter = 0;
                }
                else if(counter>= roundTime2)
                {
                    if(GameApp.PlayerManager.GameStart)
                    {
                        current = new IdleCommand(GameApp.PlayerManager.Player);
                        current.Do();

                        GameApp.EnemyManager.GenerateEnemyCommand();
                        GameApp.MapManager.SpecialBlockEvent();
                    }
                    counter = 0;
                }
                
            }
        }
        else
        {
            if (current.Update(dt) == true)
            {
                current = null;
                GameApp.PlayerManager.playerIdleTime = 0;
            }
        }
    }

    public void Clear()
    {
        willDoCommand = null;
        unDoStack.Clear();
        current = null;
    }

    //������һ������
    public void Undo()
    {
        if (unDoStack.Count > 0)
        {
            unDoStack.Pop().Undo();
        }
    }

}


