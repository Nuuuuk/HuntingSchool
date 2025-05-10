using System;
using UnityEngine;

public class TipTrigger : MonoBehaviour
{
    public int tipsIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameMgr.Instance.ShowTip(tipsIndex);
            gameObject.SetActive(false);
        }
    }
}