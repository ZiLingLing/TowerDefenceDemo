using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XFGameFramework;

namespace TowerDefence
{
    public class FightPanel : Panel
    {
        [SerializeField]
        private Text hpText;
        [SerializeField]
        private Text coinText;
        [SerializeField]
        private Text enemyWaveText;

        public override void OnLoaded(params object[] param)
        {
            base.OnLoaded(param);
            InitUI();
        }

        private void InitUI()
        {
            UpdateHp();
            UpdateCoin();
            UpdateWave();
        }

        private void UpdateHp()
        {
            hpText.text = Module.LoadController<FightController>().GetFightModel().playerHp.ToString();
        }

        private void UpdateCoin()
        {
            coinText.text = Module.LoadController<FightController>().GetFightModel().playerCoin.ToString();
        }

        private void UpdateWave()
        {
            int currentWave = Module.LoadController<FightController>().GetFightModel().currentFightWave;
            int levelID = Module.LoadController<GameController>().GetCurrentPlayLevelID();
            int totalWave = Module.LoadController<LevelController>().GetLevelInfo(levelID).enemyWaves.Count;
            enemyWaveText.text = string.Format("{0}/{1}²¨", currentWave, totalWave);
        }

        public void OnBtnPauseClick()
        {
            Module.LoadPanel<PausePanel>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            EventManager.AddEvent(EventConst.ON_PLAYER_COIN_COUNT_CHANGE, UpdateCoin);
            EventManager.AddEvent(EventConst.ON_PLAYER_HP_CHANGE, UpdateHp);
            EventManager.AddEvent(EventConst.ON_ENEMY_WAVE_CHANGE, UpdateWave);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            EventManager.RemoveEvent(EventConst.ON_PLAYER_COIN_COUNT_CHANGE, UpdateCoin);
            EventManager.RemoveEvent(EventConst.ON_PLAYER_HP_CHANGE, UpdateHp);
            EventManager.RemoveEvent(EventConst.ON_ENEMY_WAVE_CHANGE, UpdateWave);
        }
    }
}

