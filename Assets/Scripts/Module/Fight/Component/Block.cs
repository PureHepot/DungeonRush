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
    public int RowIndex;//���±�
    public int ColIndex;//���±�
    public BlockType Type;

    private SpriteRenderer selectSp;//ѡ�еĸ��ӵ�ͼƬ
    private SpriteRenderer gridSp;//����ͼƬ
    private SpriteRenderer dirSp;//�ƶ�����ͼƬ

    private void Awake()
    {
        selectSp = transform.Find("select").GetComponent<SpriteRenderer>();
        gridSp = transform.Find("grid").GetComponent<SpriteRenderer>();
        dirSp = transform.Find("dir").GetComponent<SpriteRenderer>();
    }


}
