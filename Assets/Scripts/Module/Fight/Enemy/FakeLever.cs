using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeLever : MonoBehaviour
{
    [Header("嘲讽音效的名称")]
    public string laughSoundName = "clown_laugh"; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 判断是否是玩家触碰到了它
        if (collision.CompareTag("Player"))
        {
            // 播放音效
            GameApp.SoundManager.PlayEffect(laughSoundName, transform.position);

            /*
            GameApp.ControllerManager.ApplyFunc(ControllerType.GameUI, Defines.OpenMessageView, new MessageInfo()
            {
                txt = "",
                okCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); },
                noCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); }
            });*/

            // 销毁假拉杆
            Destroy(gameObject);

            Debug.Log("玩家触发了假拉杆");
        }
    }
}
