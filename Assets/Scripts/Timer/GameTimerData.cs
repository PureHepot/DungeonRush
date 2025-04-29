using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimerData
{
    private float timer;
    private System.Action callback;

    public GameTimerData(float timer, System.Action callback)
    {
        this.timer = timer;
        this.callback = callback;
    }

    public bool OnUpdate(float dt)
    {
        timer-=dt;
        if (timer <= 0)
        {
            this.callback?.Invoke();
            return true;
        }
        return false;
    }
}
