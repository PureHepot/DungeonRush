using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 管理角色信息
/// </summary>
public class PlayerManager
{
    public bool GameStart;

    private PlayerController player;
    public PlayerController Player
    {
        get
        {
            if (player == null)
            {
                player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            }
            return player;
        }
    }

    public float playerIdleTime;

    public int playerRow
    {
        get
        {
            return player.RowIndex;
        }
    }

    public int playerCol
    {
        get
        {
            return player.ColIndex;
        }
    }

    public int ArmSkillStep = 2;

    public int playerMaxHP;
    private int playerHP;
    public int PlayerHP
    {
        get {
            if (playerHP <= 0)
            {
                GameApp.CommandManager.AddCommand(new DieCommand(player));
                PlayerDead();
            }

            return playerHP; 
        }
        set
        {
            playerHP = Mathf.Clamp(value,0,playerMaxHP);
        }
    }
    public int playerMaxEnergy;
    private int playerEnergy;
    public int PlayerEnergy
    {
        get { return playerEnergy; }
        set
        {
            playerEnergy = Mathf.Clamp(value,0,playerMaxEnergy);
        }
    }
    public bool isSkilling;
    public bool isDead;
    public bool hasLeg;
    public bool hasArm;
    public bool hasHeart;
    private SpriteRenderer bodySp;
    public SpriteRenderer BodySp
    {
        get
        {
            if (bodySp == null)
            {
                bodySp = GameObject.FindWithTag("Player").GetComponentInChildren<SpriteRenderer>();
            }
            return bodySp;
        }
    }

    public GameObject playerPrefab;

    public Dictionary<int, Dictionary<string, string>> datas;

    public PlayerManager()
    {
        playerHP = playerMaxHP = 6;
        playerEnergy = playerMaxEnergy = 5;
        isDead = false;
        hasLeg = false;
        hasArm = false;
        hasHeart = false;

        playerPrefab = Resources.Load<GameObject>("Prefabs/Model/Player/Player");
    }

    public float GetDistance(ModelBase model, int type = 1)
    {
        if(type == 1)
            return Mathf.Sqrt(Mathf.Pow(model.RowIndex - player.RowIndex, 2) + Mathf.Pow(model.ColIndex - player.ColIndex, 2));
        if (type == 2)
        {
            if (model.RowIndex == player.RowIndex)
                return Mathf.Abs(model.ColIndex - player.ColIndex);
            if (model.ColIndex == player.ColIndex)
                return Mathf.Abs(model.RowIndex - player.RowIndex);
            return 99;
        }
        return 99;
    }

    public void InitPlayer()
    {
        playerIdleTime = 0;
        playerHP = playerMaxHP;
        playerEnergy = playerMaxEnergy;
        isDead = false;
        bodySp = null;
        player = null;
    }

    public void CreatePlayer(int row, int col)
    {
        InitPlayer();
        GameApp.TimerManager.Register(0.5f, () =>
        {
            player = GameObject.Instantiate(playerPrefab, GameApp.MapManager.GetBlockPos(row, col), Quaternion.identity).GetComponent<PlayerController>();
            player.RowIndex = row;
            player.ColIndex = col;
            player.PlayAni("Flashout");
            GameStart = true;
            GameApp.CommandManager.isStop = false;
            Debug.Log("Player");
        });
        
    }

    public void PlayerDead()
    {
        GameApp.TimerManager.Register(0.7f, () =>
        {
            //GameApp.ControllerManager.ApplyFunc(ControllerType.GameUI, Defines.OpenMessageView, new MessageInfo()
            //{
            //    txt = "You r Dead...",
            //    okBtntxt = "Restart",
            //    noBtntxt = "Exit",
            //    okCallback = () =>
            //    {
            //        GameApp.ViewManager.CloseAll();
            //        LoadSomeScene.LoadtheScene("Text", () =>
            //        {
            //            GameApp.ViewManager.Close(ViewType.LoadingView);
            //            GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, Defines.BeginFight);
            //        },
            //        () =>
            //        {
            //            GameApp.ViewManager.Open(ViewType.TipView, "Tutorial");
            //            GameApp.ViewManager.Open(ViewType.PlayerDesView);
            //            GameApp.PlayerManager.datas = GameApp.ConfigManager.GetConfigData("skill").GetLines();
            //        });
            //    },
            //    noCallback = () =>
            //    {
            //        LoadSomeScene.LoadtheScene("game", () => { },
            //        () =>
            //        {
            //            GameApp.ViewManager.CloseAll();
            //            GameApp.ViewManager.Open(ViewType.StartView);
            //        });
            //    }
            //});
            GameApp.ViewManager.Open(ViewType.TipView, "DIE");
            GameApp.TimerManager.Register(1f, () =>
            {
                GameApp.ViewManager.CloseAll();
                LoadSomeScene.LoadtheScene(SceneManager.GetActiveScene().name, () =>
                {
                    GameApp.ViewManager.Close(ViewType.LoadingView);
                    GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, Defines.BeginFight);
                },
                () =>
                {
                    GameApp.ViewManager.Open(ViewType.TipView, SceneManager.GetActiveScene().name);
                    GameApp.ViewManager.Open(ViewType.PlayerDesView);
                    if(SceneManager.GetActiveScene().name == "Level 1")
                        GameApp.PlayerManager.hasLeg = false;
                    GameApp.PlayerManager.datas = GameApp.ConfigManager.GetConfigData("skill").GetLines();
                });
            });
        });
    }

    public void Update(float dt)
    {
        if (!isDead && player!=null && !isSkilling)
            playerIdleTime += dt;

    }
}
