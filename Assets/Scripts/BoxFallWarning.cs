using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class BoxFallWarning : MonoBehaviour, ILevelReset
{
    public float warningHeight = 13.6f;
    public Vector3 offset;
    public UnityEvent<Vector3> OnWarning;

    private void Update()
    {
        if (transform.position.y <= warningHeight)
        {
            GetComponent<DragObj>()?.DisableDrag();
            OnWarning?.Invoke(transform.position + offset);
            enabled = false;
        }
    }

    public void LevelReset()
    {
        enabled = true;
    }
}