
    using UnityEngine;

    public class Level1 : MonoBehaviour
    {
        public DoorRotAnim doorRotAnim;
        private void Start()
        {
            GameMgr.Instance.OnLevelChange.AddListener(OnLevelChange);
        }

        void OnLevelChange(int curLevel)
        {
            if (curLevel == 2)
            {
                doorRotAnim.CloseDoor();
                GameMgr.Instance.OnLevelChange.RemoveListener(OnLevelChange);
            }
        }
    }
