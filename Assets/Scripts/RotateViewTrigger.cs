using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateViewTrigger : MonoBehaviour, ILevelReset
{
    public Transform start, end;
    public float totalDis;
    public bool playerInRange;
    public CameraFollow cam;
    public Transform player;
    private float lastDis;
    public float totalChangeAngle = 90f;
    public float startAngle;

    private void Start()
    {
        totalDis = end.position.x - start.position.x;
    }

    private void Update()
    {
        if (!playerInRange) return;

        float currentDis = player.position.x - start.position.x;
        currentDis = Mathf.Max(currentDis, 0);
        currentDis = Mathf.Min(currentDis, totalDis);
        cam.rotateY = startAngle + totalChangeAngle * currentDis / totalDis;
        lastDis = currentDis;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            startAngle = cam.rotateY;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void LevelReset()
    {
        playerInRange = false;
    }
}
