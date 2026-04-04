using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevCheatManager : MonoBehaviour
{
    [Header("开发者作弊模式开关")]
    public bool enableCheat = true;

    void Update()
    {
        // 如果开关为 false，不执行任何检测逻辑
        if (!enableCheat)
            return;

        // 检测按键组合：按住 O 键的同时，按下 P 键（使用 GetKeyDown 确保只触发一次）
        if (Input.GetKey(KeyCode.O) && Input.GetKeyDown(KeyCode.P))
        {
            UnlockAllPlayerSkills();
        }
    }

    /// <summary>
    /// 解锁所有技能的逻辑
    /// </summary>
    private void UnlockAllPlayerSkills()
    {
        // 打印绿色日志，方便在 Console 窗口中确认作弊码是否生效
        Debug.Log("<color=green>【开发者模式】激活：已解锁玩家所有技能（含隐藏技能斩击）！</color>");

       

        if (GameApp.PlayerManager != null)
        {
            
            GameApp.PlayerManager.hasLeg = true;
            GameApp.PlayerManager.hasArm = true;
            GameApp.PlayerManager.hasHeart = true;
            GameApp.PlayerManager.hasSlash = true;




            GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, "OnSlashUnlock");

            // 为了方便测试斩击，顺便把能量回满
            // GameApp.PlayerManager.AddSlashEnergy(999); 
        }
        else
        {
            Debug.LogWarning("未找到 PlayerManager，请检查 GameApp 初始化顺序！");
        }
    }
}
