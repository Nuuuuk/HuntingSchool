using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour, ILevelReset
{
    public MovingPlatform[] movingPlatforms;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var platform in movingPlatforms)
            {
                platform.StartMoving();
            }
            gameObject.SetActive(false);
        }
    }

    public void LevelReset()
    {
        gameObject.SetActive(true);
    }
}
