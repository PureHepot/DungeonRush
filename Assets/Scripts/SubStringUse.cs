using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SubStringUse : MonoBehaviour
{
    public string[] txt;
    public int idx;
    public int index;

    float count;
    public float StartTime = 1f;
    public float wordTime = 0.4f;
    bool isStart = false;
    bool stop = false;
    bool once = true;

    StringBuilder str = new StringBuilder();

    Text text;

    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stop) return;
        count += Time.deltaTime;
        if (count > StartTime)
        {
            isStart = true;
            count = 0;
        }

        if (isStart && count>wordTime)
        {
            if (idx == txt.Length && once)
            {
                isStart = false;
                once = false;
                GameApp.TimerManager.Register(0.7f, () =>
                {
                    GameApp.SoundManager.StopBGM();
                    LoadSomeScene.LoadtheScene("Tutorial", () =>
                    {
                        GameApp.ViewManager.Close(ViewType.LoadingView);
                        GameApp.ControllerManager.ApplyFunc(ControllerType.Fight, Defines.BeginFight);
                    },
                    () =>
                    {
                        GameApp.SoundManager.PlayBGM("music");
                        GameApp.ViewManager.Open(ViewType.TipView, "Tutorial");
                        GameApp.ViewManager.Open(ViewType.PlayerDesView);
                        GameApp.PlayerManager.datas = GameApp.ConfigManager.GetConfigData("skill").GetLines();
                        GameApp.ViewManager.Close(ViewType.SubString);
                    });
                });
            }
            if (idx != txt.Length)
            {
                count = 0;
                str.Append(txt[idx][index++]);
                text.text = str.ToString();
                GameApp.SoundManager.PlayEffect("TalkSingle", Camera.main.transform.position);
                if (index == txt[idx].Length)
                {
                    idx++;
                    index = 0;
                    stop = true;
                    GameApp.TimerManager.Register(1.5f, () => { stop = false; str.Append('\n'); });
                }
            }
            
        }
    }
}
