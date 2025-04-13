using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XFGameFramework;

namespace TowerDefence
{
    public class SceneLoadingPanel : Panel
    {
        [SerializeField]
        private Image progressBar;
        private float targetProgress;

        public override void OnLoaded(params object[] param)
        {
            base.OnLoaded(param);
            UpdateProgressBar(0);
            progressBar.fillAmount = 0;
        }

        public void UpdateProgressBar(float progress)
        {
            targetProgress = progress;
        }

        public bool IsProgressDone()
        {
            return progressBar.fillAmount >= 1;
        }

        private void Update()
        {
            if(progressBar.fillAmount <= targetProgress)
            {
                progressBar.fillAmount += Time.deltaTime;
            }
        }
    }
}
