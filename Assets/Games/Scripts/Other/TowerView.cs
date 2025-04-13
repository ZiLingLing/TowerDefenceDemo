using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFGameFramework;

namespace TowerDefence
{
    public class TowerView : MonoBehaviour
    {
        private Module module;

        private UpgradeTowerPanel upgradeTowerPanel;

        public TowerBase Tower { get; set; }

        public void InitModule(Module module)
        {
            this.module = module;
        }

        public void OnClickTower()
        {
            if(Tower == null || Tower.gameObject.activeSelf == false)
            {
                module.LoadPanel<CreateTowerPanel>(UIType.UI, null, this);
            }
            else
            {
                //打开升级炮塔界面
                upgradeTowerPanel = module.LoadPanel<UpgradeTowerPanel>(UIType.UI, null, this);
                upgradeTowerPanel.onDisableAction += OnUpgradePanelDisable;

                Tower.ShowAttackRange();
            }
        }

        public void OnUpgradePanelDisable()
        {
            upgradeTowerPanel.onDisableAction -= OnUpgradePanelDisable;
            if(Tower != null)
            {
                Tower.HideAttackRange();
            }
        }
    }
}

