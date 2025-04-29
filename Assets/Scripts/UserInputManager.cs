using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// 用户控制管理器 键盘控制 鼠标操作等
/// </summary>
public class UserInputManager
{
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
    }
}
