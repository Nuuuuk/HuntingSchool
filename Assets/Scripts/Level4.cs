using UnityEngine;

public class Level4 : MonoBehaviour
{
    public Transform spawnPoint;
    public Enemy4 enemy;
    public DoorRotAnim doorRotAnim;
    public CameraFollow cameraFollow;
    public ILevelReset[] levelResets;

    private void Start()
    {
        GameMgr.Instance.OnLevelRestart.AddListener(OnLevelRestart);
        GameMgr.Instance.OnLevelChange.AddListener(OnLevelChange);
        levelResets = GetComponentsInChildren<ILevelReset>();
        enemy = GetComponentInChildren<Enemy4>();
    }

    private void OnLevelChange(int level)
    {
        if (level == 4)
        {
            enemy.enabled = true;
            GameMgr.Instance.playerController.SetRotationY(90);
            cameraFollow.rotateY = 90f;
        }
        else if (level == 5)
        {
            doorRotAnim.CloseDoor();
        }
    }

    private void OnLevelRestart(int level)
    {
        if (level == 4)
        {
            GameMgr.Instance.playerController.Reset(spawnPoint.position);
            foreach (var levelReset in levelResets)
            {
                levelReset.LevelReset();
            }
        }
    }
}