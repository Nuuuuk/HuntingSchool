using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuitTrigger : MonoBehaviour, ILevelReset
{
    public bool playerInRange;
    public bool enemyInRange;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
        else if (other.CompareTag("Enemy"))
        {
            enemyInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
        else if (other.CompareTag("Enemy"))
        {
            enemyInRange = false;
        }
    }

    public void LevelReset()
    {
        playerInRange = false;
        enemyInRange = false;
    }
}
