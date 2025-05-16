using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum BlockType
{
    bornland,
    floor,
    prick,
    obstacle,
    empty,
    player,
    enemy
}

public class Block : MonoBehaviour
{
    public int RowIndex;//���±�
    public int ColIndex;//���±�
    public BlockType originType;
    public BlockType Type;

    public Vector3Int pos;
    public Tile tile;
    public Sprite tileSprite;
    public Sprite replaceSprite;

    private SpriteRenderer selectSp;//ѡ�еĸ��ӵ�ͼƬ
    private SpriteRenderer gridSp;//����ͼƬ
    private SpriteRenderer dirSp;//�ƶ�����ͼƬ

    private void Awake()
    {
        selectSp = transform.Find("select").GetComponent<SpriteRenderer>();
        gridSp = transform.Find("grid").GetComponent<SpriteRenderer>();
        dirSp = transform.Find("dir").GetComponent<SpriteRenderer>();

        GameApp.MessageCenter.AddEvent(gameObject, Defines.OnSelectEvent, OnSelectCallback);
    }

    //�������Ū���࣬����ûʱ���ˣ��л���ȥŪ
    public void Init()
    {
        tileSprite = tile.sprite;
        switch (originType)
        {
            case BlockType.prick:

                break;
            case BlockType.bornland:
                GameApp.PlayerManager.CreatePlayer(RowIndex, ColIndex);
                break;
        }
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
        if (GameApp.CommandManager.isRunningCommand == false && selectSp.enabled == true && Type!=BlockType.empty)
        {
            GameApp.MapManager.HideStepGrid(GameApp.PlayerManager.Player, int.Parse(GameApp.PlayerManager.datas[1002]["Range"]));
            GameApp.CommandManager.AddCommand(new FlashCommand(GameApp.PlayerManager.Player, this));
        }
    }
}
