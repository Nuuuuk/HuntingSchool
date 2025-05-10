using UnityEngine;

public class Level7 : MonoBehaviour
{
    public Enemy7 enemy;
    public Transform spawnPoint;
    public ILevelReset[] levelResets;
    public BoxFallWarning boxFallWarning;

    private void Start()
    {
        GameMgr.Instance.OnLevelRestart.AddListener(OnLevelRestart);
        GameMgr.Instance.OnLevelChange.AddListener(OnLevelChange);
        levelResets = GetComponentsInChildren<ILevelReset>();
        enemy = GetComponentInChildren<Enemy7>();
        boxFallWarning = GetComponentInChildren<BoxFallWarning>();
        boxFallWarning.enabled = false;
    }

    private void OnLevelChange(int level)
    {
        if (level == 7)
        {
            enemy.enabled = true;
            boxFallWarning.enabled = true;
        }
    }

    private void OnLevelRestart(int level)
    {
        if (level == 7)
        {
            GameMgr.Instance.playerController.Reset(spawnPoint.position);
            foreach (var levelReset in levelResets)
            {
                levelReset.LevelReset();
            }
        }
    }
}