using UnityEngine;

public class Level6 : MonoBehaviour
{
    public Transform spawnPoint;
    public Enemy6 enemy;
    public ILevelReset[] levelResets;
    public BoxFallWarning boxFallWarning;
    public DoorRotAnim doorRotAnim;

    private void Start()
    {
        GameMgr.Instance.OnLevelRestart.AddListener(OnLevelRestart);
        GameMgr.Instance.OnLevelChange.AddListener(OnLevelChange);
        levelResets = GetComponentsInChildren<ILevelReset>();
        enemy = GetComponentInChildren<Enemy6>();
        boxFallWarning = GetComponentInChildren<BoxFallWarning>();
        boxFallWarning.enabled = false;
    }

    private void OnLevelChange(int level)
    {
        if (level == 6)
        {
            enemy.enabled = true;
            boxFallWarning.enabled = true;
        }
        else if (level == 7)
        {
            Debug.Log(1);
            doorRotAnim.CloseDoor();
        }
    }

    private void OnLevelRestart(int level)
    {
        if (level == 6)
        {
            GameMgr.Instance.playerController.Reset(spawnPoint.position);
            foreach (var levelReset in levelResets)
            {
                levelReset.LevelReset();
            }
        }
    }
}