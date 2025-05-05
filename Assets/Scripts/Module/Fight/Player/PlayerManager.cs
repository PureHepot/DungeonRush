using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理角色信息
/// </summary>
public class PlayerManager
{
    public ModelBase player;

    public int playerHP;
    public int playerEnegy;
    public bool isDead;
    public bool hasLeg;
    public bool hasArm;
    public bool hasHeart;

    public PlayerManager()
    {
        playerHP = 6;
        isDead = false;
        hasLeg = false;
        hasArm = false;
        hasHeart = false;
    }


}
