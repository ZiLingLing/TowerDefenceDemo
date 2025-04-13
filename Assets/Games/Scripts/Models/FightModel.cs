using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFGameFramework;

namespace TowerDefence
{
    public class FightModel : Model
    {
        public int currentFightWave;

        public float playerHp;

        public int playerCoin;

        public override void OnInit()
        {
            base.OnInit();
            playerHp = Module.LoadController<LevelController>().GetPlayerHp();
            int levelID = Module.LoadController<GameController>().GetCurrentPlayLevelID();
            LevelData levelData = Module.LoadController<LevelController>().GetLevelInfo(levelID);
            playerCoin = levelData.initCoinCount;
        }
    }
}

