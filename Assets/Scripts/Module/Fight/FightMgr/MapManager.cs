using System;
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
    public List<Block> prickTraplist;

    public Dictionary<BlockType, List<Tile>> replaceTileDic;

    public MapManager()
    {
        
    }

    public void Init()
    {
        tilemap = GameObject.Find("Grid/ground").GetComponent<Tilemap>();
        prickTraplist = new List<Block>();
        replaceTileDic = new Dictionary<BlockType, List<Tile>>();

        List<Vector3Int> temp = new List<Vector3Int>();

        int min_x = 1000;
        int min_y = 1000;
        int max_x = -1000;
        int max_y = -1000;
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {

                temp.Add(pos);
                min_x = Mathf.Min(min_x, pos.x);
                min_y = Mathf.Min (min_y, pos.y);
                max_x = Mathf.Max(max_x, pos.x);
                max_y = Mathf.Max(max_y, pos.y);

        }

        TotalRowCount = max_y - min_y + 1;
        TotalColCount = max_x - min_x + 1;

        Debug.Log(TotalRowCount);
        Debug.Log(TotalColCount);

        mapArr = new Block[TotalRowCount, TotalColCount];

        UnityEngine.Object objPrefab = Resources.Load("Prefabs/Model/block");
        for(int i =  0; i < temp.Count; i++)
        {
            int row = temp[i].y - min_y;
            int col = temp[i].x - min_x;

            Block o = (UnityEngine.Object.Instantiate(objPrefab) as GameObject).AddComponent<Block>();
            o.RowIndex = row;
            o.ColIndex = col;
            o.pos = temp[i];
            o.transform.position = tilemap.CellToWorld(temp[i]) + new Vector3(0.5f, 0.5f, 0);

            Tile tile = tilemap.GetTile(temp[i]) as Tile;
            if (tile != null)
                o = BlockHandler(o, tile);
            else
                o.originType = o.Type = BlockType.empty;

            mapArr[row, col] = o;
        }
        GameApp.EnemyManager.GetSceneEnemy();
    }

    public Block BlockHandler(Block b, Tile tile)
    {
        BlockType type;
        if (Enum.TryParse(tile.name.Split('-')[0], out type))
        {
            b.originType = type;
            b.Type = type;
            b.tile = tile;
            b.Init();
        }
        if (type == BlockType.prick)
        {
            prickTraplist.Add(b);
        }
        return b;
    }

    public void PrickTrapUpdate()
    {
        if (!replaceTileDic.ContainsKey(BlockType.prick))
        {
            int i = 1;
            replaceTileDic[BlockType.prick] = new List<Tile>();
            while (true)
            {
                Tile tile = Resources.Load<Tile>($"TileMap/Tiles/prick-{i}");
                if (tile != null)
                    replaceTileDic[BlockType.prick].Add(tile);
                else
                    break;
                i++;
            }
        }
        else
        {
            foreach (var item in prickTraplist)
            {
                int count = replaceTileDic[BlockType.prick].Count;
                int idx = int.Parse(tilemap.GetTile(item.pos).name.Split('-')[1]) + 1;

                if (idx > count) idx -= count;

                tilemap.SetTile(item.pos, replaceTileDic[BlockType.prick][idx-1]);
                if (item.Type == BlockType.player && idx == 2)
                {
                    GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, Defines.OnPlayerHpChange, -1);
                }
            }
            
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
