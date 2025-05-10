using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    public DoorRotAnim doorRotAnim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameMgr.Instance.UseKey())
            {
                doorRotAnim.OpenDoor();
            }
        }
    }
}
