using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoadingModel : BaseModel
{
    public string SceneName;//���صĳ�������
    public int viewId;
    public System.Action callback {  get; set; }
    public LoadingModel() : base()
    {

    }
}
