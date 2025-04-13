using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class StartUpPanel : MonoBehaviour
    {
        [SerializeField]
        private Slider sliderProgress;
        [SerializeField]
        private Text tipText;

        private void Start()
        {
            sliderProgress.gameObject.SetActive(false);
            tipText.text = string.Empty;
        }

        public void RefreshProgress(float currentProgress)
        {
            if (sliderProgress.gameObject.activeSelf == false)
            {
                sliderProgress.gameObject.SetActive(true);
            }
            sliderProgress.value = currentProgress;
        }

        public void ShowTip(string message)
        {
            tipText.text = message;
        }
    }
}