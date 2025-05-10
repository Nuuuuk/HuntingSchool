using System;
using System.Collections;
using UnityEngine;

public class DoorRotAnim : MonoBehaviour, ILevelReset
{
    public float animTime;
    public float targetAngleY = 90;

    [Header("Audio")]
    public AudioSource doorAudio;
    public AudioClip openSound;
    public AudioClip closeSound; 

    public void OpenDoor()
    {
        StopAllCoroutines();
        StartCoroutine(Rotate(targetAngleY));

        // opendoor audio
        if (doorAudio != null && openSound != null)
        {
            doorAudio.PlayOneShot(openSound);
        }

    }

    public void CloseDoor()
    {
        StopAllCoroutines();
        StartCoroutine(Rotate(0));

        if (doorAudio != null && closeSound != null)
        {
            doorAudio.PlayOneShot(closeSound);
        }

    }

    public void ResetDoor()
    {
        StopAllCoroutines();
        Vector3 curRot = transform.localEulerAngles;
        curRot.y = 0;
        transform.localEulerAngles = curRot;
    }

    private IEnumerator Rotate(float angleY)
    {
        Vector3 curRot = transform.localEulerAngles;
        float y = curRot.y;
        if (y > 180) y -= 360;;
        float times = animTime / Time.fixedDeltaTime;
        float step = (angleY - y) / times;
        WaitForFixedUpdate wait = new WaitForFixedUpdate();
        for (int i = 0; i < times; i++)
        {
            curRot.y += step;
            transform.localEulerAngles = curRot;
            yield return wait;
        }

        curRot.y = angleY;
        transform.localEulerAngles = curRot;
    }

    public void LevelReset()
    {
        ResetDoor();
    }
}