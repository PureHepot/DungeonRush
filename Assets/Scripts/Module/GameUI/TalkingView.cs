using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkingView : BaseView
{

    public override void Open(params object[] args)
    {
        string txt = args[0].ToString();

        Find<Text>("BG/txt").text = txt;
    }


}
