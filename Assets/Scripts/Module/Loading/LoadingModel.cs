using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoadingModel : BaseModel
{
    public string SceneName;//加载的场景名称
    public int viewId;
    public System.Action callback {  get; set; }
    public LoadingModel() : base()
    {

    }
}
