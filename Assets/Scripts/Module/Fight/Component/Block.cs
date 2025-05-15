using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    birth,
    floor,
    obstacle,
    empty,
    player,
    enemy
}

public class Block : MonoBehaviour
{
    public int RowIndex;//行下标
    public int ColIndex;//纵下标
    public BlockType Type;

    private SpriteRenderer selectSp;//选中的格子的图片
    private SpriteRenderer gridSp;//网格图片
    private SpriteRenderer dirSp;//移动方向图片

    private void Awake()
    {
        selectSp = transform.Find("select").GetComponent<SpriteRenderer>();
        gridSp = transform.Find("grid").GetComponent<SpriteRenderer>();
        dirSp = transform.Find("dir").GetComponent<SpriteRenderer>();

        GameApp.MessageCenter.AddEvent(gameObject, Defines.OnSelectEvent, OnSelectCallback);
    }
    public void ShowGrid(Color color)
    {
        gridSp.enabled = true;
        gridSp.color = color;
    }

    public void HideGrid()
    {
        gridSp.enabled = false;
    }

    private void OnMouseEnter()
    {
        if (gridSp.enabled == true)
        {
            selectSp.enabled = true;
        }
    }
    private void OnMouseExit()
    {
        selectSp.enabled = false;
    }

    private void OnSelectCallback(object arg)
    {
        if (GameApp.CommandManager.isRunningCommand == false && selectSp.enabled == true)
        {
            GameApp.MapManager.HideStepGrid(GameApp.PlayerManager.Player, int.Parse(GameApp.PlayerManager.datas[1002]["Range"]));
            GameApp.CommandManager.AddCommand(new FlashCommand(GameApp.PlayerManager.Player, this));
        }
    }
}
