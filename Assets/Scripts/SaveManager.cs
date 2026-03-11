using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager
{
    // 判断本地是否有存档
    public bool HasSave()
    {
        return PlayerPrefs.HasKey("SavedLevel");
    }

    // 保存当前游戏进度
    public void SaveGame(string targetLevelName)
    {
        // 保存关卡名
        PlayerPrefs.SetString("SavedLevel", targetLevelName);

        // 保存玩家技能解锁状态 (PlayerPrefs 只能存 Int/Float/String，通常用 1/0 代表 True/False)
        PlayerPrefs.SetInt("HasLeg", GameApp.PlayerManager.hasLeg ? 1 : 0);
        PlayerPrefs.SetInt("HasArm", GameApp.PlayerManager.hasArm ? 1 : 0);
        PlayerPrefs.SetInt("HasHeart", GameApp.PlayerManager.hasHeart ? 1 : 0);
        PlayerPrefs.SetInt("HasBody", GameApp.PlayerManager.hasBody ? 1 : 0);
        PlayerPrefs.SetInt("HasSlash", GameApp.PlayerManager.hasSlash ? 1 : 0);

        // 保存玩家当前的血量和能量
        PlayerPrefs.SetInt("PlayerHP", GameApp.PlayerManager.PlayerHP);
        PlayerPrefs.SetInt("PlayerEnergy", GameApp.PlayerManager.PlayerEnergy);

        // 强制写入磁盘
        PlayerPrefs.Save();
        Debug.Log("游戏已保存！关卡：" + targetLevelName);
    }

    // 读取游戏进度，并返回需要加载的关卡名称
    public string LoadGame()
    {
        if (!HasSave()) return "Tutorial"; // 防错机制：没存档默认去教程关

        // 读取技能解锁状态
        GameApp.PlayerManager.hasLeg = PlayerPrefs.GetInt("HasLeg", 0) == 1;
        GameApp.PlayerManager.hasArm = PlayerPrefs.GetInt("HasArm", 0) == 1;
        GameApp.PlayerManager.hasHeart = PlayerPrefs.GetInt("HasHeart", 0) == 1;
        GameApp.PlayerManager.hasBody = PlayerPrefs.GetInt("HasBody", 1) == 1;
        GameApp.PlayerManager.hasSlash = PlayerPrefs.GetInt("HasSlash", 0) == 1;

        // 读取血量和能量
        GameApp.PlayerManager.PlayerHP = PlayerPrefs.GetInt("PlayerHP", 6);
        GameApp.PlayerManager.PlayerEnergy = PlayerPrefs.GetInt("PlayerEnergy", 5);

        // 读取关卡名
        string savedLevel = PlayerPrefs.GetString("SavedLevel", "Tutorial");
        Debug.Log("读取存档成功！前往关卡：" + savedLevel);

        return savedLevel;
    }

    // 清空存档（用于“重新开始”游戏）
    public void ClearSave()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("存档已清空！");
    }
}
