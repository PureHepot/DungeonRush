using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HealItem : MonoBehaviour
{
    [Header("恢复的血量")]
    public int healAmount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. 确保碰到的是带有 "Player" 标签的物体
        if (collision.CompareTag("Player"))
        {
            // 获取玩家当前血量和最大血量
            int currentHp = GameApp.PlayerManager.PlayerHP;
            int maxHp = GameApp.PlayerManager.playerMaxHP;

            // 2. 核心判断：只有当玩家不是满血时，才触发治疗
            if (currentHp < maxHp)
            {
                // 计算实际恢复量（防止加血后溢出超过上限）
                int actualHeal = Mathf.Min(healAmount, maxHp - currentHp);

                // 发送加血事件（复用你现有的 MVC 逻辑，自动更新底层数据和 UI）
                GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, Defines.OnPlayerHpChange, actualHeal);

                // 播放拾取音效（复用你之前 HealCommand 里的音效）
                GameApp.SoundManager.PlayEffect("heal", transform.position);

                // 销毁医疗包自身
                Destroy(gameObject);

                Debug.Log($"拾取医疗包，恢复了 {actualHeal} 点生命值！");
            }
            else
            {
                // 满血时不会执行销毁，玩家可以直接穿过去
                Debug.Log("玩家血量已满，无法使用医疗包！");
            }
        }
    }
}
