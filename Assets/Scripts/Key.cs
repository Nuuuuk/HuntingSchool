using System;
using UnityEngine;

public class Key : MonoBehaviour, ILevelReset
{
    public bool isColsed;

    private void Update()
    {
        if (isColsed && Input.GetKeyDown(KeyCode.E))
        {
            GameMgr.Instance.PickUpKey();
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
        isColsed = false;
        gameObject.SetActive(true);
    }
}