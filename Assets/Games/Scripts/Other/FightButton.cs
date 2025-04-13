using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFGameFramework;

namespace TowerDefence
{
    public class FightButton : MonoBehaviour
    {
        private Module module;

        [SerializeField]
        private float defaultAngle = 0;
        private FightButtonPanel fightButtonPanel;
        private Rect screenRect;

        public void InitModule(Module module)
        {
            this.module = module;
        }

        public void Show()
        {
            this.gameObject.SetActive(true);
            if(fightButtonPanel != null)
            {
                fightButtonPanel.Close();
                fightButtonPanel = null;
            }
            fightButtonPanel = module.LoadPanel<FightButtonPanel>();
        }

        public void Hide()
        {
            if (fightButtonPanel != null)
            {
                fightButtonPanel.Close();
                fightButtonPanel = null;
            }
            this.gameObject.SetActive(false);
        }

        public void UpdateFightProgress(float progress)
        {
            fightButtonPanel.UpdateFightProgress(progress);
        }

        public void AddClick(Action action)
        {
            fightButtonPanel.onFightBtnClick += action;
        }

        private void Awake()
        {
            screenRect = new Rect(0, 0, Screen.width, Screen.height);
        }

        private void Update()
        {
            if (fightButtonPanel == null) return;
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);
            //更新箭头方向
            if (screenRect.Contains(screenPosition))
            {
                //出怪点在屏幕内箭头指向默认方向
                fightButtonPanel.UpdateDirection(defaultAngle);
            }
            else
            {
                Vector3 postPosition = new Vector3();
                postPosition.x = Mathf.Clamp(screenPosition.x, 80, Screen.width - 80);
                postPosition.y = Mathf.Clamp(screenPosition.y, 80, Screen.height - 80);

                Vector3 dir = screenPosition - postPosition;
                float angle = Vector2.SignedAngle(dir, Vector2.right);
                fightButtonPanel.UpdateDirection(-angle);
            }
            //使得按钮UI一直显示在屏幕中
            screenPosition.x = Mathf.Clamp(screenPosition.x, 100, Screen.width - 100);
            screenPosition.y = Mathf.Clamp(screenPosition.y, 100, Screen.height - 100);
            fightButtonPanel.transform.position = screenPosition;
        }
    }
}
