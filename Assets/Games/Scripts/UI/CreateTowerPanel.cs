using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XFABManager;
using XFGameFramework;

namespace TowerDefence
{
    public class CreateTowerPanel : Panel
    {
        [SerializeField]
        private Button btnArcherTower;
        [SerializeField]
        private Button btnRockTower;
        [SerializeField]
        private Button btnMagicTower;
        [SerializeField]
        private Transform panelPosition;

        private TowerView towerPosition;

        public override void OnLoaded(params object[] param)
        {
            base.OnLoaded(param);

            if(param == null || param.Length == 0)
            {
                throw new System.Exception("û�д������");
            }
            towerPosition = (TowerView)param[0];
            if(towerPosition == null)
            {
                throw new System.Exception("����Ĳ���Ϊ��");
            }
            panelPosition.position = Camera.main.WorldToScreenPoint(towerPosition.transform.position);

            InitBtn(btnArcherTower, TowerType.Archer);
            InitBtn(btnRockTower, TowerType.Rock);
            InitBtn(btnMagicTower, TowerType.Magic);
        }

        private void OnPlayerCoinCountChange()
        {
            InitBtn(btnArcherTower, TowerType.Archer);
            InitBtn(btnRockTower, TowerType.Rock);
            InitBtn(btnMagicTower, TowerType.Magic);
        }

        private void InitBtn(Button btn,TowerType towerType)
        {
            TowerInfo towerInfo = Module.LoadController<TowerController>().FindLowestLevelTowerInfo(towerType);
            ImageLoader loader = btn.transform.Find("Icon").GetComponent<ImageLoader>();
            Text textPrice = btn.transform.Find("TextPrice").GetComponent<Text>();
            CanvasGroup canvasGroup = btn.transform.GetComponent<CanvasGroup>();

            loader.AssetName = towerInfo.icon_name;
            textPrice.text = towerInfo.cost.ToString();

            bool canCreateTower = Module.LoadController<TowerController>().CanCreateTower(towerInfo.id);
            canvasGroup.alpha = canCreateTower == true ? 1 : 0.5f;

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                if(canCreateTower == false)
                {
                    LogUtil.Log("��Ҳ���");
                    Module.LoadController<TipController>().ShowToast("��Ҳ���");
                    return;
                }

                TowerBase tower = Module.LoadController<TowerController>().CreateTower(towerInfo.id, towerPosition.transform);
                if(tower != null)
                {
                    LogUtil.Log("���������ɹ�");
                    towerPosition.Tower = tower;
                    Close();
                }
                else
                {
                    LogUtil.Log("��������ʧ��");
                }
            });
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            EventManager.AddEvent(EventConst.ON_PLAYER_HP_CHANGE, OnPlayerCoinCountChange);
        }

        private void Update()
        {
            if (towerPosition != null)
            {
                panelPosition.position = Camera.main.WorldToScreenPoint(towerPosition.transform.position);
            }
        }

        protected override void OnDisable()
        {
            base.OnEnable();
            EventManager.RemoveEvent(EventConst.ON_PLAYER_HP_CHANGE, OnPlayerCoinCountChange);
        }
    }
}

