using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    floor,
    empty
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
    }


}
