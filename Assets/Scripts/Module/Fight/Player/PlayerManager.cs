using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理角色信息
/// </summary>
public class PlayerManager
{
    private PlayerController player;
    public PlayerController Player
    {
        get
        {
            if (player == null)
            {
                player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            }
            return player;
        }
    }


    public int playerMaxHP;
    private int playerHP;
    public int PlayerHP
    {
        get { return playerHP; }
        set
        {
            playerHP = Mathf.Clamp(value,0,playerMaxHP);
        }
    }
    public int playerMaxEnergy;
    private int playerEnergy;
    public int PlayerEnergy
    {
        get { return playerEnergy; }
        set
        {
            playerEnergy = Mathf.Clamp(value,0,playerMaxEnergy);
        }
    }
    public bool isDead;
    public bool hasLeg;
    public bool hasArm;
    public bool hasHeart;

    public Dictionary<int, Dictionary<string, string>> datas;

    public PlayerManager()
    {
        playerHP = playerMaxHP = 6;
        playerEnergy = playerMaxEnergy = 5;
        isDead = false;
        hasLeg = false;
        hasArm = false;
        hasHeart = false;
    }


}
