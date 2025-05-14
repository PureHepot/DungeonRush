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

        Object objPrefab = Resources.Load("Prefabs/Model/block");
        for(int i =  0; i < temp.Count; i++)
        {
            int row = i / TotalColCount;
            int col = i % TotalColCount;

            Block o = (Object.Instantiate(objPrefab) as GameObject).AddComponent<Block>();
            o.RowIndex = row;
            o.ColIndex = col;
            o.transform.position = tilemap.CellToWorld(temp[i]) + new Vector3(0.5f, 0.5f, 0);
            o.Type = BlockType.floor;
            mapArr[row, col] = o;
        }
        GameApp.EnemyManager.GetSceneEnemy();
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

    public void GetCellPos(ModelBase model, Vector3 pos)
    {
        Vector3Int t = tilemap.WorldToCell(pos);
        model.RowIndex = t.x;
        model.ColIndex = t.y;
        Debug.Log($"R{model.RowIndex},C{model.ColIndex}");
    }

    public Block GetBlockByPos(int row, int col)
    {
        return mapArr[row,col];
    }

    //显示移动区域
    public void ShowStepGrid(ModelBase model, int step)
    {
        _BFS bfs = new _BFS(TotalRowCount, TotalColCount);

        List<_BFS.Point> points = bfs.Search(model.RowIndex, model.ColIndex, step);

        for (int i = 0; i < points.Count; i++)
        {
            mapArr[points[i].RowIndex, points[i].ColIndex].ShowGrid(new Color(0,234f/255f,234f/255f,0.5f));
        }
    }

    //隐藏移动的区域
    public void HideStepGrid(ModelBase model, int step)
    {
        _BFS bfs = new _BFS(TotalRowCount, TotalColCount);

        List<_BFS.Point> points = bfs.Search(model.RowIndex, model.ColIndex, step);

        for (int i = 0; i < points.Count; i++)
        {
            mapArr[points[i].RowIndex, points[i].ColIndex].HideGrid();
        }
    }
}
