using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraManager
{
    private Transform camTf;

    private Vector3 prePos;//之前的位置
    public CameraManager()
    {
        camTf = Camera.main.transform;
        prePos = camTf.transform.position;
    }

    public void SetPos(Vector3 pos)
    {
        pos.z = camTf.position.z;
        camTf.transform.DOMove(pos,0.5f);
    }

    public void ResetPos()
    {
        camTf.transform.position = prePos;
    }

    public void CameraShake()
    {
        camTf.DOShakePosition(0.5f, 3f, 10);
    }
}
