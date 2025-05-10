using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelResetTrans : MonoBehaviour, ILevelReset
{
    public Vector3 initPos;
    public Quaternion initRot;

    private void Start()
    {
        initPos = transform.position;
        initRot = transform.rotation;
    }

    public void LevelReset()
    {
        transform.position = initPos;
        transform.rotation = initRot;
    }
}
