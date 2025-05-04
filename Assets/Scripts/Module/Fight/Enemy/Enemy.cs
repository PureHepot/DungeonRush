using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : ModelBase
{
    public BaseCommand lastCommand;
    public BaseCommand current;

    public BaseCommand GenerateCommand()
    {
        return null;
    }
}
