using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDesModel : BaseModel
{
    public int Hpcount;
    public int EnergyCount;

    public PlayerDesModel(int hp, int energy):base()
    {
        this.Hpcount = hp;
        this.EnergyCount = energy;
    }
}
