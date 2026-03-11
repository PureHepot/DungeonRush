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
    public bool hasBody;
    public bool isShielded;
    public bool hasSlash;
    public int slashEnergy = 100;         // 当前斩击能量
    public int maxSlashEnergy = 100;      // 最大斩击能量

    public float slashEnergyTimer = 0f;       // 能量恢复计时器
    public int slashEnergyRecoverRate = 5;    // 每秒恢复的能量值

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
        hasBody = true;
        isShielded = false;
        hasSlash = false;

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
        isShielded = false;
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
                    else if(SceneManager.GetActiveScene().name == "Level 2")
                        GameApp.PlayerManager.hasHeart = false;
                    else if (SceneManager.GetActiveScene().name == "Level 3")
                        GameApp.PlayerManager.hasArm = false;
                    GameApp.PlayerManager.datas = GameApp.ConfigManager.GetConfigData("skill").GetLines();
                });
            });
        });
    }

    public void Update(float dt)
    {
        if (!isDead && player!=null && !isSkilling)
            playerIdleTime += dt;
        
        
        // 斩击能量自动恢复逻辑
        // 只有当玩家存活、已解锁斩击技能、且能量未满时才进行恢复
        if (!isDead && hasSlash && slashEnergy < maxSlashEnergy)
        {
            slashEnergyTimer += dt; // 累加时间

            // 每过 1 秒执行一次恢复
            if (slashEnergyTimer >= 1.0f)
            {
                slashEnergyTimer -= 1.0f; // 扣除1秒，保留多余的小数时间

                slashEnergy += slashEnergyRecoverRate; // 增加能量

                // 确保能量不会超过上限
                if (slashEnergy > maxSlashEnergy)
                {
                    slashEnergy = maxSlashEnergy;
                }

                // 向控制器发送信号，UI 会自动接收并呈现出液体上涨动画
                GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, "OnSlashEnergyChange", slashEnergy);
            }
        }
    }


    // 控制 Player 预制体下 ShieldOverlay 节点的显示隐藏
    
    public void HandleShieldVisual(bool isActive)
    {
        if (Player == null) return;
        Transform shield = Player.transform.Find("ShieldOverlay");
        if (shield != null) shield.gameObject.SetActive(isActive);
    }
}
