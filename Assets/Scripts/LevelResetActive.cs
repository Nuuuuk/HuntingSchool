using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelResetActive : MonoBehaviour, ILevelReset
{
    public bool initActive;
    private void Start()
    {
        initActive = gameObject.activeInHierarchy;
    }

    public void LevelReset()
    {
        gameObject.SetActive(initActive);
    }
}
