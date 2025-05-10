
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.SceneManagement;

    public class GameMgr : MonoBehaviour
    {
        public static GameMgr Instance;

        public DelayHide[] tipUI;
        public GameObject winUI;
        public PlayerController playerController;
        public int level;
        public UnityEvent<int> OnLevelChange;
        public UnityEvent<int> OnLevelRestart;

        public bool hasKey;

        private void Awake()
        {
            Instance = this;
        }

        public void SetLevel(int curLevel)
        {
            level = curLevel;
            OnLevelChange.Invoke(level);
        }

        public void RestartLevel()
        {
            hasKey = false;
            OnLevelRestart.Invoke(level);
        }

        public void Win()
        {
            winUI.gameObject.SetActive(true);
            playerController.enabled = false;
            StopAllCoroutines();
            StartCoroutine(DelayRestart());
        }

        private IEnumerator DelayRestart()
        {
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(3f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1;
        }

        public void ShowTip(int index)
        {
            tipUI[index].Show();
        }

        public void PickUpKey()
        {
            hasKey = true;
        }

        public bool UseKey()
        {
            bool temp = hasKey;
            hasKey = false;
            return temp;
        }
    }
