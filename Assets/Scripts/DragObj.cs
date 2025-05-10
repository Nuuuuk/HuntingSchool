using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DragObj : MonoBehaviour, ILevelReset
{
    public bool isColsed;
    public bool isDragging;
    private Rigidbody playerRb;
    private Rigidbody rb;

    private FixedJoint dragJoint;
    public bool lockY;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }

    private void Update()
    {
        if (!isDragging && isColsed && Input.GetKeyDown(KeyCode.G))
        {
            BeginDrag();
            return;
        }

        if (isDragging && Input.GetKeyDown(KeyCode.G))
        {
            StopDrag();
        }
    }

    private void FixedUpdate()
    {
        if (isDragging && playerRb != null)
        {
            rb.velocity = playerRb.velocity;
        }
    }

    private void BeginDrag()
    {
        isDragging = true;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        if (lockY)
        {
            rb.constraints = rb.constraints | RigidbodyConstraints.FreezePositionY;
        }
        dragJoint = gameObject.AddComponent<FixedJoint>();
        dragJoint.connectedBody = GameMgr.Instance.playerController.GetComponent<Rigidbody>();
        dragJoint.breakForce = 10000;
    }

    private void StopDrag()
    {
        isDragging = false;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        if (lockY)
        {
            rb.constraints = rb.constraints | RigidbodyConstraints.FreezePositionY;
        }
        if (dragJoint != null)
            Destroy(dragJoint);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRb = other.GetComponent<Rigidbody>();
            isColsed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isColsed = false;
        }
    }

    public void LevelReset()
    {
        isDragging = false;
        isColsed = false;
        enabled = true;
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void DisableDrag()
    {
        StopDrag();
        rb.isKinematic = true;
        enabled = false;
    }
}