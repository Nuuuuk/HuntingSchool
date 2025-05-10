
    using System;
    using UnityEngine;

    public class DelayHide : MonoBehaviour
    {
        public float durationTime = 2f;
        private float timer;

        public void Show()
        {
            timer = 0;
            gameObject.SetActive(true);
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= durationTime)
            {
                gameObject.SetActive(false);
            }
        }
    }
