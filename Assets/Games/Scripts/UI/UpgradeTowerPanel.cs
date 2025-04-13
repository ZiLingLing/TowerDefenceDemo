using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XFGameFramework;

namespace TowerDefence
{
    public class UpgradeTowerPanel : Panel
    {
        [SerializeField]
        private Text upgradeText;
        [SerializeField]
        private Text sellText;

        private TowerView towerPosition;

        [SerializeField]
        private Button btnUpgrade;
        [SerializeField]
        private Transform panelPosition;

        public Action onDisableAction;

        public override void OnLoaded(params object[] param)
        {
            base.OnLoaded(param);

            if(param == null || param.Length == 0)
            {
                throw new System.Exception("������崫�����Ϊ�ջ򳤶�Ϊ0");
            }

            towerPosition = (TowerView)param[0];

            if(towerPosition == null)
            {
                throw new System.Exception("��������������쳣");
            }
            panelPosition.position = Camera.main.WorldToScreenPoint(towerPosition.transform.position);
            InitUI();
        }

        private void InitUI()
        {
            TowerInfo nextLevelInfo = Module.LoadController<TowerController>().FindNextLevelTowerInfo(towerPosition.Tower.TowerID);

            btnUpgrade.gameObject.SetActive(nextLevelInfo != null);

            if(nextLevelInfo != null)
            {
                upgradeText.text = nextLevelInfo.cost.ToString();
                bool canUpgrade = Module.LoadController<TowerController>().CanCreateTower(nextLevelInfo.id);
                btnUpgrade.GetComponent<CanvasGroup>().alpha = canUpgrade == true ? 1 : 0.5f;
            }

            TowerInfo currentLevelInfo = Module.LoadController<TowerController>().GetTowerInfo(towerPosition.Tower.TowerID);
            float sellPrice = currentLevelInfo.cost * Module.LoadController<LevelController>().GetSellDiscount();
            sellText.text = ((int)sellPrice).ToString();

        }

        public void OnBtnClickUpgrade()
        {
            TowerInfo nextLevelInfo = Module.LoadController<TowerController>().FindNextLevelTowerInfo(towerPosition.Tower.TowerID);
            bool canUpgrade = Module.LoadController<TowerController>().CanCreateTower(nextLevelInfo.id);
            if(canUpgrade == false)
            {
                LogUtil.Log("��Ҳ����޷�����");
                Module.LoadController<TipController>().ShowToast("��Ҳ���");
                return;
            }
            //ɾ������
            towerPosition.Tower.Close();
            //����������
            towerPosition.Tower = Module.LoadController<TowerController>().CreateTower(nextLevelInfo.id, towerPosition.transform);

            Module.LoadController<AudioController>().PlaySound(AudioConst.SOUND_LEVEL_UP);
            Close();
        }

        public void OnBtnClickSell()
        {
            TowerInfo currentLevelInfo = Module.LoadController<TowerController>().GetTowerInfo(towerPosition.Tower.TowerID);

            int sellPrice = (int)(currentLevelInfo.cost * Module.LoadController<LevelController>().GetSellDiscount());
            Module.LoadController<FightController>().IncreaseCoin(sellPrice);
            //ɾ������
            towerPosition.Tower.Close();
            towerPosition = null;

            Module.LoadController<AudioController>().PlaySound(AudioConst.SOUND_TOWER_SELL);

            Close();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        private void Update()
        {
            if(towerPosition != null)
            {
                panelPosition.position = Camera.main.WorldToScreenPoint(towerPosition.transform.position);
            }
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            onDisableAction?.Invoke();
        }
    }
}

