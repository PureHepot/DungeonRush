using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer
{
    private List<GameTimerData> timers;//存储时间数据的集合

    public GameTimer()
    {
        timers = new List<GameTimerData>();
    }

    public void Register(float timer, System.Action action)
    {
        GameTimerData data = new GameTimerData(timer, action);
        timers.Add(data);
    }

    public void OnUpdate(float dt)
    {
        for (int i = timers.Count - 1; i >= 0; i--)
        {
            if (timers[i].OnUpdate(dt) == true)
            {
                timers.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// 打断计时器
    /// </summary>
    public void Break()
    {
        timers.Clear();
    }

    public int Count()
    {
        return timers.Count;
    }
}
