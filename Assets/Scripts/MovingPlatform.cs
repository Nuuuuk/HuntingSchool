using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour, ILevelReset
{
    public Transform startPoint;
    public Transform endPoint;
    public float speed = 2.0f;

    private Vector3 startPos;
    private Vector3 endPos;
    public bool isStart;
    public int flag = 1;
    private Rigidbody rb;
    private Vector3 lastPosition;
    public Vector3 deltaPosition;

    void Start()
    {
        startPos = startPoint.position;
        endPos = endPoint.position;
        rb = GetComponent<Rigidbody>();
        lastPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (!isStart) return;

        Vector3 move = new Vector3(0, 0, Time.fixedDeltaTime * speed * flag);;

        rb.MovePosition(rb.position + move);

        if (rb.position.z >= endPos.z)
        {
            flag = -1;
        }
        else if (rb.position.z <= startPos.z)
        {
            flag = 1;
        }

        deltaPosition = rb.position - lastPosition;
        lastPosition = rb.position;
    }

    public void StartMoving()
    {
        isStart = true;
    }

    public void LevelReset()
    {
        isStart = false;
    }
}
