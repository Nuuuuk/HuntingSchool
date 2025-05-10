using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleTrigger : MonoBehaviour, ILevelReset
{
    public bool hasHandle;

    public Transform haftPos;

    public DragObj handle;

    public LidRotAnim lid;

    private void Update()
    {
        if (hasHandle) return;

        if (handle != null)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                handle.DisableDrag();
                handle.transform.position = haftPos.position;
                handle.transform.rotation = Quaternion.identity;
                hasHandle = true;
                enabled = false;
                lid.CloseLid();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasHandle && other.CompareTag("Handle"))
        {
            handle = other.GetComponent<DragObj>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!hasHandle && other.CompareTag("Handle"))
        {
            handle = null;
        }
    }

    public void LevelReset()
    {
        enabled = true;
        hasHandle = false;
        handle = null;
    }
}
