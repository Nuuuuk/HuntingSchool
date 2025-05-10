using System;
using System.Collections;
using UnityEngine;

public class LidRotAnim : MonoBehaviour, ILevelReset
{
    public float animTime = 1f;
    public float targetAngleX = -90;
    public Quaternion initRot;
    public DeathTrigger deathTrigger;

    private void Start()
    {
        initRot = transform.localRotation;
    }

    public void CloseLid()
    {
        StopAllCoroutines();
        StartCoroutine(Rotate(-90));
    }

    private IEnumerator Rotate(float angleX)
    {
        Vector3 curRot = transform.localEulerAngles;
        float x = curRot.x;
        if (x > 180) x -= 360;;
        float times = animTime / Time.fixedDeltaTime;
        float step = (angleX - x) / times;
        WaitForFixedUpdate wait = new WaitForFixedUpdate();
        for (int i = 0; i < times; i++)
        {
            curRot.x += step;
            transform.localEulerAngles = curRot;
            yield return wait;
        }

        curRot.x = angleX;
        transform.localEulerAngles = curRot;
        deathTrigger.gameObject.SetActive(false);
    }

    public void LevelReset()
    {
        StopAllCoroutines();
        transform.localRotation = initRot;
        deathTrigger.gameObject.SetActive(true);
    }
}