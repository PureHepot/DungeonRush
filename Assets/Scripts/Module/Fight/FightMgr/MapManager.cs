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
                Tile tile = Resources.Load<Tile>($"TileMap/Tiles/{type}-{i}");
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
        //隐藏桥体
        HideAllBridges();
    }

    public Block BlockHandler(Block b, Tile tile)
    {
        BlockType type = BlockType.empty;

        // 如果瓦片名称包含墙体前缀
        if (tile.name.Contains("atlas_walls_low") || tile.name.Contains("atlas_walls_high"))
        {
            type = BlockType.obstacle; // 强制标记为障碍物类型
            b.originType = type;
            b.Type = type;
            b.tile = tile;
            b.Init();
        }
        // 按照枚举名解析
        else if (Enum.TryParse(tile.name.Split('-')[0], out type))
        {
            b.originType = type;
            b.Type = type;
            b.tile = tile;
            b.Init();
        }

        // 只有字典中存在的类型才加入列表
        if (typeBlocklist.ContainsKey(type))
        {
            typeBlocklist[type].Add(b);
        }

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
        ShotInvoke();

        tilemap.RefreshAllTiles();
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
                    else if (scenename == "Level 3")
                    {
                        GameApp.ViewManager.CloseAll();
                        LoadSomeScene.LoadtheScene("Level 4", () =>
                        {
                            GameApp.ViewManager.Close(ViewType.LoadingView);
                            GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, Defines.BeginFight);
                        },
                        () =>
                        {
                            GameApp.ViewManager.Open(ViewType.TipView, "END");
                            GameApp.ViewManager.Open(ViewType.PlayerDesView);
                        });
                    }
                });
            }
        }
    }

    //技能限制器
    public void LevelConstraint()
    {
        string scenename = SceneManager.GetActiveScene().name;

        if (scenename == "Level 1")
        {
            // 获取第一关 Boss 的数量
            int count = GameApp.EnemyManager.GetEnemyCount(EnemyType.GoldenLeg);
            if (count <= 0)
            {
                /* 
                foreach(var item in typeBlocklist[BlockType.constraint])
                {
                    tilemap.SetTile(item.pos, replaceTileDic[BlockType.floor][0]);
                    item.originType = BlockType.floor;
                    item.Type = BlockType.floor;
                }
                */

                
                if (GameApp.PlayerManager.hasLeg == false)
                {
                    GameApp.PlayerManager.hasLeg = true;
                    GameApp.SoundManager.PlayEffect("getSkill", Camera.main.transform.position);
                    GameApp.ControllerManager.ApplyFunc(ControllerType.GameUI, Defines.OpenMessageView, new MessageInfo()
                    {
                        txt = "击败 Boss！获得了新能力：【灵巧之腿】\n(现在你可以进行冲刺了！)",
                        okCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); },
                        noCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); }
                    });
                }
            }
        }
        else if (scenename == "Level 2")
        {
            int count = GameApp.EnemyManager.GetEnemyCount(EnemyType.Homoheart);
            if (count <= 0)
            {
                /*
                
                foreach (var item in typeBlocklist[BlockType.constraint])
                {
                    tilemap.SetTile(item.pos, replaceTileDic[BlockType.floor][0]);
                    item.originType = BlockType.floor;
                    item.Type = BlockType.floor;
                }
                */

                
                if (GameApp.PlayerManager.hasHeart == false)
                {
                    GameApp.PlayerManager.hasHeart = true;
                    GameApp.SoundManager.PlayEffect("getSkill", Camera.main.transform.position);

                    GameApp.ControllerManager.ApplyFunc(ControllerType.GameUI, Defines.OpenMessageView, new MessageInfo()
                    {
                        txt = "击败 Boss！获得了新能力：【坚韧之心】\n(现在你可以抵挡一次致命攻击！)",
                        okCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); },
                        noCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); }
                    });
                }
            }
        }
        else if (scenename == "Level 3")
        {
            int count = GameApp.EnemyManager.enemyCount;
            if (count <= 0)
            {
                /*
                
                foreach (var item in typeBlocklist[BlockType.constraint])
                {
                    tilemap.SetTile(item.pos, replaceTileDic[BlockType.floor][0]);
                    item.originType = BlockType.floor;
                    item.Type = BlockType.floor;
                }
                */

                
                if (GameApp.PlayerManager.hasArm == false)
                {
                    GameApp.PlayerManager.hasArm = true;
                    GameApp.SoundManager.PlayEffect("getSkill", Camera.main.transform.position);

                    GameApp.ControllerManager.ApplyFunc(ControllerType.GameUI, Defines.OpenMessageView, new MessageInfo()
                    {
                        txt = "击败最终 Boss！获得了新能力：【力量之腕】\n(你的攻击力获得了极大提升！)",
                        okCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); },
                        noCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); }
                    });
                }
            }

            int handeyeCount = GameApp.EnemyManager.GetEnemyCount(EnemyType.Handeye);
            if (handeyeCount <= 1)
            {
                /*
                
                foreach (var item in typeBlocklist[BlockType.constraint1])
                {
                    tilemap.SetTile(item.pos, replaceTileDic[BlockType.floor][0]);
                    item.originType = BlockType.floor;
                    item.Type = BlockType.floor;
                }
                */
            }
        }
    }


    /*public void BuildBridge()
    {
        foreach (var b in typeBlocklist[BlockType.redbutton1])
        {
            if (b.Type == BlockType.player)
            {
                if (int.Parse(tilemap.GetTile(b.pos).name.Split('-')[1]) == 1)
                {
                    tilemap.SetTile(b.pos, replaceTileDic[BlockType.redbutton1][1]);
                    GameApp.SoundManager.PlayEffect("trigger", Camera.main.transform.position);
                    foreach (var t in typeBlocklist[BlockType.bridge1])
                    {
                        tilemap.SetTile(t.pos, replaceTileDic[BlockType.bridge1][1]);
                        ChangeBlockOriginType(t.RowIndex, t.ColIndex, BlockType.floor);
                    }

                    
                    GameApp.ControllerManager.ApplyFunc(ControllerType.GameUI, Defines.OpenMessageView, new MessageInfo()
                    {
                        txt = "某处的机关启动了",
                        okCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); },
                        noCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); }
                    });
                }
            }
        }
        foreach (var b in typeBlocklist[BlockType.redbutton2])
        {
            if (b.Type == BlockType.player)
            {
                if (int.Parse(tilemap.GetTile(b.pos).name.Split('-')[1]) == 1)
                {
                    tilemap.SetTile(b.pos, replaceTileDic[BlockType.redbutton2][1]);
                    GameApp.SoundManager.PlayEffect("trigger", Camera.main.transform.position);
                    foreach (var t in typeBlocklist[BlockType.bridge2])
                    {
                        tilemap.SetTile(t.pos, replaceTileDic[BlockType.bridge2][1]);
                        ChangeBlockOriginType(t.RowIndex, t.ColIndex, BlockType.floor);
                    }

                    
                    GameApp.ControllerManager.ApplyFunc(ControllerType.GameUI, Defines.OpenMessageView, new MessageInfo()
                    {
                        txt = "某处的机关启动了",
                        okCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); },
                        noCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); }
                    });
                }
            }
        }
        foreach (var b in typeBlocklist[BlockType.redbutton3])
        {
            if (b.Type == BlockType.player)
            {
                if (int.Parse(tilemap.GetTile(b.pos).name.Split('-')[1]) == 1)
                {
                    tilemap.SetTile(b.pos, replaceTileDic[BlockType.redbutton3][1]);
                    GameApp.SoundManager.PlayEffect("trigger", Camera.main.transform.position);
                    foreach (var t in typeBlocklist[BlockType.bridge3])
                    {
                        tilemap.SetTile(t.pos, replaceTileDic[BlockType.bridge3][1]);
                        ChangeBlockOriginType(t.RowIndex, t.ColIndex, BlockType.floor);
                    }

                    
                    GameApp.ControllerManager.ApplyFunc(ControllerType.GameUI, Defines.OpenMessageView, new MessageInfo()
                    {
                        txt = "某处的机关启动了",
                        okCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); },
                        noCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); }
                    });
                }
            }
        }
    }*/
    public void BuildBridge()
    {
        // 依次检查三种红按钮和对应的桥
        CheckAndTriggerBridge(BlockType.redbutton1, BlockType.bridge1);
        CheckAndTriggerBridge(BlockType.redbutton2, BlockType.bridge2);
        CheckAndTriggerBridge(BlockType.redbutton3, BlockType.bridge3);
    }

    private void CheckAndTriggerBridge(BlockType buttonType, BlockType bridgeType)
    {
        if (!typeBlocklist.ContainsKey(buttonType)) return;

        foreach (var b in typeBlocklist[buttonType])
        {
            // 当玩家踩上按钮，且该按钮还处于“未按下”的状态（名字包含 "-1"）
            if (b.Type == BlockType.player && tilemap.GetTile(b.pos) != null && tilemap.GetTile(b.pos).name.Contains("-1"))
            {
                // 改变按钮贴图为已按下状态，并播放音效
                tilemap.SetTile(b.pos, replaceTileDic[buttonType][1]);
                GameApp.SoundManager.PlayEffect("trigger", Camera.main.transform.position);

                // 让隐藏的桥体出现
                if (typeBlocklist.ContainsKey(bridgeType))
                {
                    foreach (var t in typeBlocklist[bridgeType])
                    {
                        
                        if (t.tile != null)
                        {
                            tilemap.SetTile(t.pos, t.tile);
                        }

                        // 变为普通地板，允许通行，且不会再触发坠落
                        ChangeBlockOriginType(t.RowIndex, t.ColIndex, BlockType.floor);
                        t.Type = BlockType.floor;
                    }
                }
                GameApp.ControllerManager.ApplyFunc(ControllerType.GameUI, Defines.OpenMessageView, new MessageInfo()
                {
                    txt = "某处的机关启动了",
                    okCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); },
                    noCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); }
                });

            }
        }
    }

    
 
    public void HideAllBridges()
    {
        // 遍历所有可能的桥梁类型（如果你后续加入了 BlockType.bridge_A 这种枚举，也加进这个数组里）
        BlockType[] bridgeTypes = new BlockType[] { BlockType.bridge1, BlockType.bridge2, BlockType.bridge3, BlockType.bridge4 };

        foreach (var bType in bridgeTypes)
        {
            if (typeBlocklist.ContainsKey(bType))
            {
                foreach (var t in typeBlocklist[bType])
                {
                    // 在游戏开始时，将桥体地块在视觉上清空（设为 null）
                    // 但不用担心，原瓦片资产已经安全保存在 t.tile 中了
                    tilemap.SetTile(t.pos, null);
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

    //地刺陷阱
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
                    if (GameApp.PlayerManager.isShielded)
                    {
                        // 有护盾：抵挡伤害、碎盾、取消视觉表现
                        GameApp.PlayerManager.isShielded = false;
                        GameApp.PlayerManager.HandleShieldVisual(false);
                        GameApp.SoundManager.PlayEffect("shieldbreak", GameApp.PlayerManager.Player.transform.position);
                        Debug.Log("护盾成功抵挡了地刺陷阱的伤害！");
                    }
                    else
                    {
                        // 没护盾：正常扣血
                        GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, Defines.OnPlayerHpChange, -1);
                    }
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
                    GameApp.SoundManager.PlayEffect("buttondown", Camera.main.transform.position);
                    foreach (var t in typeBlocklist[BlockType.door1])
                    {
                        tilemap.SetTile(t.pos, replaceTileDic[BlockType.door1][1]);
                        ChangeBlockOriginType(t.RowIndex, t.ColIndex, BlockType.floor);
                        GameApp.SoundManager.PlayEffect("dooropen", Camera.main.transform.position);
                    }

                    
                    GameApp.ControllerManager.ApplyFunc(ControllerType.GameUI, Defines.OpenMessageView, new MessageInfo()
                    {
                        txt = "某处沉重的大门打开了",
                        okCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); },
                        noCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); }
                    });
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
                    GameApp.SoundManager.PlayEffect("buttondown", Camera.main.transform.position);
                    foreach (var t in typeBlocklist[BlockType.door2])
                    {
                        tilemap.SetTile(t.pos, replaceTileDic[BlockType.door2][1]);
                        GameApp.SoundManager.PlayEffect("dooropen", Camera.main.transform.position);
                        ChangeBlockOriginType(t.RowIndex, t.ColIndex, BlockType.floor);
                    }

                    
                    GameApp.ControllerManager.ApplyFunc(ControllerType.GameUI, Defines.OpenMessageView, new MessageInfo()
                    {
                        txt = "某处沉重的大门打开了",
                        okCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); },
                        noCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); }
                    });
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
                // 状态机自增，改变按钮贴图并播放音效
                tilemap.SetTile(b.pos, replaceTileDic[BlockType.blueBtn][b.state++]);
                GameApp.SoundManager.PlayEffect("buttondown", Camera.main.transform.position);
                string scenename = SceneManager.GetActiveScene().name;
                if (scenename == "Tutorial")
                {
                    string currentMessage = "";

                    // 通过 Tilemap 网格坐标 (b.pos) 区分不同的按钮
                    if (b.pos.x == -1 && b.pos.y == -1)
                    {
                        currentMessage = "按下 Tab 键使用你的技能";
                    }
                    else if (b.pos.x == 7 && b.pos.y == 0)
                    {
                        currentMessage = "前有怪物！需要碰撞！提防张嘴！";
                    }
                    else if (b.pos.x == 27 && b.pos.y == 0)
                    {
                        currentMessage = "使用拉杆打开通往下层的大门吧";
                    }
                    else
                    {
                        currentMessage = "未知的提示信息"; 
                    }

                    GameApp.ControllerManager.ApplyFunc(ControllerType.GameUI, Defines.OpenMessageView, new MessageInfo()
                    {
                        txt = currentMessage,
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
                else if (scenename == "Level 3")
                {
                    GameApp.ControllerManager.ApplyFunc(ControllerType.GameUI, Defines.OpenMessageView, new MessageInfo()
                    {
                        txt = "Kill them all",
                        okCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); },
                        noCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); }

                    });
                }else if (scenename == "Level 4")
                {
                    GameApp.ControllerManager.ApplyFunc(ControllerType.GameUI, Defines.OpenMessageView, new MessageInfo()
                    {
                        txt = "Thank for Playing\nYou finally win",
                        okCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView); 
                            GameApp.CommandManager.isStop = true;
                            LoadSomeScene.LoadtheScene("game", () => { },
                            () =>
                            {
                                GameApp.ViewManager.CloseAll();
                                GameApp.ViewManager.Open(ViewType.StartView);
                            });
                        },
                        noCallback = () => { GameApp.ViewManager.Close(ViewType.MessageView);
                            GameApp.CommandManager.isStop = true;
                            LoadSomeScene.LoadtheScene("game", () => { },
                            () =>
                            {
                                GameApp.ViewManager.CloseAll();
                                GameApp.ViewManager.Open(ViewType.StartView);
                            });
                        }

                    });
                }
            }
        }
    }
    public void ShotInvoke()
    {
        Block b = mapArr[GameApp.PlayerManager.playerRow, GameApp.PlayerManager.playerCol];
        if(b.isshot)
        {
            GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, Defines.OnPlayerHpChange, -1);
            b.isshot = false;
            b.isdamage = false;
            b.HideGrid();
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
            if (mapArr[points[i].RowIndex, points[i].ColIndex].isshot)
                continue;
            mapArr[points[i].RowIndex, points[i].ColIndex].HideGrid();
            mapArr[points[i].RowIndex, points[i].ColIndex].isdamage = false;
        }
        GameApp.CommandManager.isStop = false;
    }
}
