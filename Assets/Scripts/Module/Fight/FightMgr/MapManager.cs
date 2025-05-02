using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 管理地图网格
/// </summary>
public class MapManager
{
    private Tilemap tilemap;

    public Block[,] mapArr;

    public int TotalRowCount;
    public int TotalColCount;

    public List<Sprite> dirSpArr;

    public MapManager()
    {

    }

    public void Init()
    {
        tilemap = GameObject.Find("Grid/ground").GetComponent<Tilemap>();
        TotalRowCount = 10;
        TotalColCount = 18;

        mapArr = new Block[TotalRowCount, TotalColCount];

        List<Vector3Int> temp = new List<Vector3Int>();

        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                temp.Add(pos);
            }
        }

        Object objPrefab = Resources.Load("Model/block");
        for(int i =  0; i < temp.Count; i++)
        {
            int row = i / TotalColCount;
            int col = i % TotalColCount;

            Block o = (Object.Instantiate(objPrefab) as GameObject).AddComponent<Block>();
            o.RowIndex = row;
            o.ColIndex = col;
            o.transform.position = tilemap.CellToWorld(temp[i]) + new Vector3(0.5f, 0.5f, 0);

            mapArr[row, col] = o;
        }
    }

    public Vector3 GetBlockPos(int row, int col)
    {
        return mapArr[row, col].transform.position;
    }

    public BlockType GetBlockType(int row, int col)
    {
        return mapArr[row, col].Type;
    }

    public void ChangeBlockType(int row, int col, BlockType type)
    {
        mapArr[row, col].Type = type;
    }
}
