using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public int X_min;
    public int X_max;
    public int Y_min;
    public int Y_max;

    public List<Sprite> dirSpArr;
    public List<Block> prickTraplist;
    public List<Block> FallTraplist;

    public Dictionary<BlockType, List<Tile>> replaceTileDic;
    public Dictionary<BlockType, List<Block>> typeBlocklist;

    public MapManager()
    {
        
    }

    private void InitBlockList()
    {
        BlockType[] types = (BlockType[])Enum.GetValues(typeof(BlockType));

        foreach(var type in types)
        {
            typeBlocklist[type] = new List<Block>();

            int i = 1;
            replaceTileDic[type] = new List<Tile>();
            while (true)
            {
                Tile tile = Resources.Load<Tile>($"TileMap/Tiles/{type.ToString()}-{i}");
                if (tile != null)
                    replaceTileDic[type].Add(tile);
                else
                    break;
                i++;
            }

        }
    }

    public void Init()
    {
        tilemap = GameObject.Find("Grid/ground").GetComponent<Tilemap>();
        typeBlocklist = new Dictionary<BlockType, List<Block>>();
        replaceTileDic = new Dictionary<BlockType, List<Tile>>();

        InitBlockList();

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
        X_min = min_x; X_max = max_x;
        Y_min = min_y; Y_max = max_y;

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

        typeBlocklist[type].Add(b);

        return b;
    }

    public void SpecialBlockEvent()
    {
        PrickTrapUpdate();
        FallTrapInvoke();
        BuildBridge();
        TriggerInvoke();
        NextLevel();
        MessageButton();
        LevelConstraint();
    }

    public void NextLevel()
    {
        foreach (var b in typeBlocklist[BlockType.downstair])
        {
            if(b.Type == BlockType.player)
            {
                GameApp.PlayerManager.GameStart = false;
                GameApp.CommandManager.isStop = true;
                GameApp.PlayerManager.Player.PlayAni("Flash");

                string scenename = SceneManager.GetActiveScene().name;
                GameApp.TimerManager.Register(0.3f, () =>
                {
                    if (scenename == "Tutorial")
                    {
                        GameApp.ViewManager.CloseAll();
                        LoadSomeScene.LoadtheScene("Level 1", () =>
                        {
                            GameApp.ViewManager.Close(ViewType.LoadingView);
                            GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, Defines.BeginFight);
                        },
                        () =>
                        {
                            GameApp.ViewManager.Open(ViewType.TipView, "Level 1");
                            GameApp.ViewManager.Open(ViewType.PlayerDesView);
                        });
                    }
                    else if (scenename == "Level 1")
                    {
                        GameApp.ViewManager.CloseAll();
                        LoadSomeScene.LoadtheScene("Level 2", () =>
                        {
                            GameApp.ViewManager.Close(ViewType.LoadingView);
                            GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, Defines.BeginFight);
                        },
                        () =>
                        {
                            GameApp.ViewManager.Open(ViewType.TipView, "Level 2");
                            GameApp.ViewManager.Open(ViewType.PlayerDesView);
                        });
                    }
                    else if (scenename == "Level 2")
                    {
                        GameApp.ViewManager.CloseAll();
                        LoadSomeScene.LoadtheScene("Level 3", () =>
                        {
                            GameApp.ViewManager.Close(ViewType.LoadingView);
                            GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, Defines.BeginFight);
                        },
                        () =>
                        {
                            GameApp.ViewManager.Open(ViewType.TipView, "Level 3");
                            GameApp.ViewManager.Open(ViewType.PlayerDesView);
                        });
                    }
                });
            }
        }
    }

    public void LevelConstraint()
    {
        string scenename = SceneManager.GetActiveScene().name;
        if (scenename == "Level 1")
        {
            int count = GameApp.EnemyManager.GetEnemyCount(EnemyType.GoldenLeg);
            if (count <= 0)
            {
                foreach(var item in typeBlocklist[BlockType.constraint])
                {
                    tilemap.SetTile(item.pos, replaceTileDic[BlockType.floor][0]);
                    item.originType = BlockType.floor;
                    item.Type = BlockType.floor;
                }
                GameApp.PlayerManager.hasLeg = true;
            }
        }
        else if (scenename == "Level 2")
        {
            int count = GameApp.EnemyManager.GetEnemyCount(EnemyType.Homoheart);
            if (count <= 0)
            {
                foreach (var item in typeBlocklist[BlockType.constraint])
                {
                    tilemap.SetTile(item.pos, replaceTileDic[BlockType.floor][0]);
                    item.originType = BlockType.floor;
                    item.Type = BlockType.floor;
                }
                GameApp.PlayerManager.hasHeart = true;
            }
        }
    }


    public void BuildBridge()
    {
        foreach (var b in typeBlocklist[BlockType.redbutton1])
        {
            if (b.Type == BlockType.player)
            {
                if (int.Parse(tilemap.GetTile(b.pos).name.Split('-')[1]) == 1)
                {
                    tilemap.SetTile(b.pos, replaceTileDic[BlockType.redbutton1][1]);
                    foreach (var t in typeBlocklist[BlockType.bridge1])
                    {
                        tilemap.SetTile(t.pos, replaceTileDic[BlockType.bridge1][1]);
                        ChangeBlockOriginType(t.RowIndex, t.ColIndex, BlockType.floor);
                    }
                }
            }
        }
        foreach (var b in typeBlocklist[BlockType.redbutton2])
        {
            if (b.Type == BlockType.player)
            {
                if (b.Type == BlockType.player)
                {
                    if (int.Parse(tilemap.GetTile(b.pos).name.Split('-')[1]) == 1)
                    {
                        tilemap.SetTile(b.pos, replaceTileDic[BlockType.redbutton2][1]);
                        foreach (var t in typeBlocklist[BlockType.bridge2])
                        {
                            tilemap.SetTile(t.pos, replaceTileDic[BlockType.bridge2][1]);
                            ChangeBlockOriginType(t.RowIndex, t.ColIndex, BlockType.floor);
                        }
                    }
                }
            }
        }
        foreach (var b in typeBlocklist[BlockType.redbutton3])
        {
            if (b.Type == BlockType.player)
            {
                if (b.Type == BlockType.player)
                {
                    if (int.Parse(tilemap.GetTile(b.pos).name.Split('-')[1]) == 1)
                    {
                        tilemap.SetTile(b.pos, replaceTileDic[BlockType.redbutton3][1]);
                        foreach (var t in typeBlocklist[BlockType.bridge3])
                        {
                            tilemap.SetTile(t.pos, replaceTileDic[BlockType.bridge3][1]);
                            ChangeBlockOriginType(t.RowIndex, t.ColIndex, BlockType.floor);
                        }
                    }
                }
            }
        }
    }

    public void FallTrapInvoke()
    {
        foreach (var item in typeBlocklist[BlockType.fall])
        {
            if (item.Type == BlockType.player)
            {
                item.isInvoked = true;
                Debug.Log("Invoke Fall");
            }
            if (item.isInvoked)
            {
                if (tilemap.GetTile(item.pos).name.Split("-")[1] == "3")
                {
                    if (item.Type == BlockType.player && item.RowIndex == GameApp.PlayerManager.playerRow && item.ColIndex == GameApp.PlayerManager.playerCol)
                    {
                        GameApp.CommandManager.AddCommand(new FallingCommand(GameApp.PlayerManager.Player));
                    }
                    else
                    {
                        item.Type = BlockType.floor;
                    }
                    continue;
                }

                if (item.state + 1 <= replaceTileDic[BlockType.fall].Count)
                {
                    item.state += 1;
                    tilemap.SetTile(item.pos, replaceTileDic[BlockType.fall][item.state-1]);
                    if (item.state == 3 && item.Type == BlockType.player)
                    {
                        if (item.RowIndex == GameApp.PlayerManager.playerRow && item.ColIndex == GameApp.PlayerManager.playerCol)
                        {
                            GameApp.CommandManager.AddCommand(new FallingCommand(GameApp.PlayerManager.Player));
                        }
                        else
                        {
                            item.Type = BlockType.floor;
                        }
                    }
                }
            }
        }

        foreach (var item in typeBlocklist[BlockType.bridge1])
        {
            if (item.Type == BlockType.player && item.originType == BlockType.bridge1)
            {
                if (item.RowIndex == GameApp.PlayerManager.playerRow && item.ColIndex == GameApp.PlayerManager.playerCol)
                {
                    GameApp.CommandManager.FallingCommand();
                }
                else
                    item.Type = BlockType.bridge1;
            }
        }
        foreach (var item in typeBlocklist[BlockType.bridge2])
        {
            if (item.Type == BlockType.player && item.originType == BlockType.bridge2)
            {
                if (item.RowIndex == GameApp.PlayerManager.playerRow && item.ColIndex == GameApp.PlayerManager.playerCol)
                    GameApp.CommandManager.FallingCommand();
                else
                    item.Type = BlockType.bridge2;
            }
        }
        foreach (var item in typeBlocklist[BlockType.bridge3])
        {
            if (item.Type == BlockType.player && item.originType == BlockType.bridge3)
            {
                if (item.RowIndex == GameApp.PlayerManager.playerRow && item.ColIndex == GameApp.PlayerManager.playerCol)
                    GameApp.CommandManager.FallingCommand();
                else
                    item.Type = BlockType.bridge3;
            }
        }
        foreach (var item in typeBlocklist[BlockType.bridge4])
        {
            if (item.Type == BlockType.player && item.originType == BlockType.bridge4)
            {
                if (item.RowIndex == GameApp.PlayerManager.playerRow && item.ColIndex == GameApp.PlayerManager.playerCol)
                {
                    GameApp.CommandManager.FallingCommand();
                }
                else
                    item.Type = BlockType.bridge4;
            }
        }

        foreach (var item in typeBlocklist[BlockType.constraint])
        {
            if (item.Type == BlockType.player && item.originType == BlockType.constraint)
            {
                if (item.RowIndex == GameApp.PlayerManager.playerRow && item.ColIndex == GameApp.PlayerManager.playerCol)
                    GameApp.CommandManager.FallingCommand();
                else
                    item.Type = BlockType.constraint;
            }
        }
    }

    public void PrickTrapUpdate()
    {
        foreach (var item in typeBlocklist[BlockType.prick])
        {
            int count = replaceTileDic[BlockType.prick].Count;
            int idx = int.Parse(tilemap.GetTile(item.pos).name.Split('-')[1]) + 1;

            if (idx > count) idx -= count;

            tilemap.SetTile(item.pos, replaceTileDic[BlockType.prick][idx - 1]);
            if (item.Type == BlockType.player && idx == 2)
            {
                if (item.RowIndex == GameApp.PlayerManager.playerRow && item.ColIndex == GameApp.PlayerManager.playerCol)
                {
                    GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, Defines.OnPlayerHpChange, -1);
                }
                else
                    item.Type = BlockType.prick;
                
            }
        }
    }

    public void TriggerInvoke()
    {
        foreach (var b in typeBlocklist[BlockType.trigger1])
        {
            if (b.Type == BlockType.player)
            {
                if (int.Parse(tilemap.GetTile(b.pos).name.Split('-')[1]) == 1)
                {
                    tilemap.SetTile(b.pos, replaceTileDic[BlockType.trigger1][1]);
                    foreach (var t in typeBlocklist[BlockType.door1])
                    {
                        tilemap.SetTile(t.pos, replaceTileDic[BlockType.door1][1]);
                        ChangeBlockOriginType(t.RowIndex, t.ColIndex, BlockType.floor);
                    }
                }
            }
        }
        foreach (var b in typeBlocklist[BlockType.trigger2])
        {
            if (b.Type == BlockType.player)
            {
                if (int.Parse(tilemap.GetTile(b.pos).name.Split('-')[1]) == 1)
                {
                    tilemap.SetTile(b.pos, replaceTileDic[BlockType.trigger2][1]);
                    foreach (var t in typeBlocklist[BlockType.door2])
                    {
                        tilemap.SetTile(t.pos, replaceTileDic[BlockType.door2][1]);
                        ChangeBlockOriginType(t.RowIndex, t.ColIndex, BlockType.floor);
                    }
                }
            }
        }
    }

    public void MessageButton()
    {
        foreach (var b in typeBlocklist[BlockType.blueBtn])
        {
            if(b.Type == BlockType.player && b.state == 1)
            {
                tilemap.SetTile(b.pos, replaceTileDic[BlockType.blueBtn][b.state++]);
                string scenename = SceneManager.GetActiveScene().name;
                if (scenename == "Tutorial")
                {
                    GameApp.ControllerManager.ApplyFunc(ControllerType.GameUI, Defines.OpenMessageView, new MessageInfo()
                    {
                        txt = "Press Tab to Use Your Skill",
                        okCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); },
                        noCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); }

                    });
                }
                else if (scenename == "Level 1")
                {
                    GameApp.ControllerManager.ApplyFunc(ControllerType.GameUI, Defines.OpenMessageView, new MessageInfo()
                    {
                        txt = "Get your legs back",
                        okCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); },
                        noCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); }

                    });
                }
                else if (scenename == "Level 2")
                {
                    GameApp.ControllerManager.ApplyFunc(ControllerType.GameUI, Defines.OpenMessageView, new MessageInfo()
                    {
                        txt = "Find a way out among the thorns and reclaim your heart",
                        okCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); },
                        noCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); }

                    });
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
    public BlockType GetBlockOriginType(int row, int col)
    {
        return mapArr[row, col].originType;
    }

    public void ChangeBlockType(int row, int col, BlockType type)
    {
        mapArr[row, col].Type = type;
    }
    public void ChangeBlockOriginType(int row, int col, BlockType type)
    {
        mapArr[row, col].originType = type;
    }

    public void GetCellPos(ModelBase model, Vector3 pos)
    {
        Vector3Int t = tilemap.WorldToCell(pos);
        model.RowIndex = t.y - Y_min;
        model.ColIndex = t.x - X_min;
        //Debug.Log($"R{model.RowIndex},C{model.ColIndex}");
    }

    public Block GetBlockByPos(int row, int col)
    {
        return mapArr[row,col];
    }

    //显示移动区域
    public void ShowStepGrid(ModelBase model, int step, Color color)
    {
        _BFS bfs = new _BFS(TotalRowCount, TotalColCount);

        List<_BFS.Point> points = bfs.Search(model.RowIndex, model.ColIndex, step);

        for (int i = 0; i < points.Count; i++)
        {
            mapArr[points[i].RowIndex, points[i].ColIndex].ShowGrid(color);
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
        GameApp.CommandManager.isStop = false;
    }
}
