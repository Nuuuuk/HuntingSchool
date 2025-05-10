using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotateViewTrigger : MonoBehaviour
{
    public float targetAngle = 0;
    public float rotateTime = 2f;
    public CameraFollow cam;

    private void Start()
    {
        cam = FindObjectOfType<CameraFollow>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(RotateViewCor());
        }
    }

    private IEnumerator RotateViewCor()
    {
        GetComponent<Collider>().enabled = false;
        yield return null;

        float currentAngle = cam.rotateY;
        float sub = targetAngle - currentAngle;
        float times = rotateTime / Time.fixedDeltaTime;
        float step = sub / times;

        var wait = new WaitForFixedUpdate();

        for (int i = 0; i < times; i++)
        {
            cam.rotateY += step;
            yield return wait;
        }

        cam.rotateY = targetAngle;
        GameMgr.Instance.playerController.SetRotationY(0);
        gameObject.SetActive(false);
    }
}
