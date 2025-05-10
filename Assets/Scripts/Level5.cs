using UnityEngine;

public class Level5 : MonoBehaviour
{
    public Transform spawnPoint;
    public Enemy5 enemy;
    public GameObject warningTrigger;
    public ILevelReset[] levelResets;
    public GameObject wall;

    private void Start()
    {
        GameMgr.Instance.OnLevelRestart.AddListener(OnLevelRestart);
        GameMgr.Instance.OnLevelChange.AddListener(OnLevelChange);
        levelResets = GetComponentsInChildren<ILevelReset>();
        enemy = GetComponentInChildren<Enemy5>();
    }

    private void OnLevelChange(int level)
    {
        if (level == 5)
        {
            enemy.enabled = true;
            wall.SetActive(true);
        }
        else if (level == 6)
        {
            enemy.StopPursuit();
        }
    }

    private void OnLevelRestart(int level)
    {
        if (level == 5)
        {
            GameMgr.Instance.playerController.Reset(spawnPoint.position);
            foreach (var levelReset in levelResets)
            {
                levelReset.LevelReset();
            }
        }
    }
}