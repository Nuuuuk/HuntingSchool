using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // 需要跟随的目标
    public Vector3 offset;    // 相机与目标的偏移位置
    public float smoothSpeed = 0.125f;  // 平滑移动速度
    public float rotateY;

    private void Start()
    {
        //offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        //transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, y, transform.rotation.eulerAngles.z));
        // 计算目标位置并加上偏移
        Vector3 desiredPosition = target.position + Quaternion.Euler(0, rotateY, 0) * offset;

        // 通过插值计算平滑位置
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        // 更新相机位置
        transform.position = smoothedPosition;
        transform.LookAt(target);
    }
}