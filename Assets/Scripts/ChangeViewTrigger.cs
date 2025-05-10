using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeViewTrigger : MonoBehaviour
{
    private CameraFollow cameraFollow;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        cameraFollow = FindObjectOfType<CameraFollow>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraFollow.offset = offset;
        }
    }
}