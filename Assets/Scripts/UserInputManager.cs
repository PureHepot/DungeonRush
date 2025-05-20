using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// 用户控制管理器 键盘控制 鼠标操作等
/// </summary>
public class UserInputManager
{
    public static bool escIsOpen;


    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {

            }
            else
            {
                Tools.ScreenPointToRay2D(Camera.main, Input.mousePosition, delegate (Collider2D col)
                {
                    if (col != null)
                    {
                        GameApp.MessageCenter.PostEvent(col.gameObject, Defines.OnSelectEvent);
                    }
                    else
                    {
                        GameApp.MessageCenter.PostEvent(Defines.OnSelectEvent);
                    }
                });
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!escIsOpen)
            {
                GameApp.ViewManager.Open(ViewType.EscView);
                escIsOpen = true;
            }
            else
            {
                GameApp.ViewManager.Close(ViewType.EscView);
                escIsOpen = false;
            }
                
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, Defines.OnPlayerHpChange, -1);
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            GameApp.PlayerManager.isSkilling = true;
            GameApp.ViewManager.Open(ViewType.SkillView);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            GameApp.PlayerManager.isSkilling = false;
            GameApp.ViewManager.Close(ViewType.SkillView, true);
        }
    }
}
