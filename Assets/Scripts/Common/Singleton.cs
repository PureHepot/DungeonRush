using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T>
{
    private readonly static T instance = Activator.CreateInstance<T>();

    public static T Instance
    {
        get { return instance; }
    }

    public virtual void Init()
    {

    }

    public virtual void Update(float dt)
    {

    }

    public virtual void Destroy()
    {

    }
}
