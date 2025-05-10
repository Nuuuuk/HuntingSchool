using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankTrigger : MonoBehaviour, ILevelReset
{
    public bool isClose;
    public Animator plankAnim;

    private void Update()
    {
        if (isClose && Input.GetKeyDown(KeyCode.G))
        {
            plankAnim.Play("Fall");
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isClose = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isClose = false;
        }
    }

    public void LevelReset()
    {
        isClose = false;
        plankAnim.Play("Empty");
        gameObject.SetActive(true);
    }
}
