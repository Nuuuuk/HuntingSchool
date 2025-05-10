using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLevelTrigger : MonoBehaviour
{
    public int level;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameMgr.Instance.SetLevel(level);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
