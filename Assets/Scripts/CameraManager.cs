using System.Collections;
using System.Collections.Generic;
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
        camTf.transform.position = pos;
    }

    public void ResetPos()
    {
        camTf.transform.position = prePos;
    }
}
