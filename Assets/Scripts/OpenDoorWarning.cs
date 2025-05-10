using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OpenDoorWarning : MonoBehaviour, ILevelReset
{
    public Transform door;
    public Enemy5 enemy;

    private void Update()
    {
        if (door.localEulerAngles.y >= 60f)
        {
            enemy.StartWarning(transform.position);
            gameObject.SetActive(false);
        }
    }

    public void LevelReset()
    {
        gameObject.SetActive(true);
    }
}
