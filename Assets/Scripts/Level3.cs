using System;
using UnityEngine;

public class Level3 : MonoBehaviour
{
    public Transform spawnPoint;
    public Enemy3 enemy;
    public GameObject door;
    public GameObject rotateViewTrigger;
    public ILevelReset[] levelResets;
    public CameraFollow cameraFollow;

    private void Start()
    {
        GameMgr.Instance.OnLevelRestart.AddListener(OnLevelRestart);
        GameMgr.Instance.OnLevelChange.AddListener(OnLevelChange);
        levelResets = GetComponentsInChildren<ILevelReset>();
        enemy = GetComponentInChildren<Enemy3>();
    }

    private void OnLevelChange(int level)
    {
        if (level == 3)
        {
            enemy.enabled = true;
        }
        else if (level == 4)
        {
            door.SetActive(true);
            rotateViewTrigger.SetActive(false);
        }
    }

    private void OnLevelRestart(int level)
    {
        if (level == 3)
        {
            GameMgr.Instance.playerController.Reset(spawnPoint.position);
            cameraFollow.rotateY = 180f;
            foreach (var levelReset in levelResets)
            {
                levelReset.LevelReset();
            }
        }
    }
}