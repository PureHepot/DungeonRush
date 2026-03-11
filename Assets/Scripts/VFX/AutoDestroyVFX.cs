using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyVFX : MonoBehaviour
{
    public float lifetime = 0.5f; // 特效存活时间，根据你8帧动画的实际长度调整

    void Start()
    {
        // 生成后，经过 lifetime 秒自动销毁自身
        Destroy(gameObject, lifetime);
    }
}
