using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XFGameFramework;

namespace TowerDefence
{
    public class FightButtonPanel : Panel
    {
        [SerializeField]
        private Image progressImg;
        [SerializeField]
        private Transform dirParent;

        public Action onFightBtnClick;

        public void UpdateFightProgress(float progress)
        {
            progressImg.fillAmount = progress;
        }

        public void UpdateDirection(float angle)
        {
            dirParent.transform.localEulerAngles = new Vector3(0, 0, angle);
        }

        public void OnFightBtnClick()
        {
            onFightBtnClick?.Invoke();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            onFightBtnClick = null;
        }
    }
}

