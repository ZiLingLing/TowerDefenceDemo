using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFGameFramework;

namespace TowerDefence
{
    public class FailPanel : Panel
    {
        public void OnBtnMenuClick()
        {
            Module.LoadController<GameController>().SetGameState(EGameState.SelectLevel);
        }

        public void OnBtnReplayClick()
        {
            Module.LoadController<FightController>().Replay();
            Close();
        }
    }
}

