using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFFSM;
using XFGameFramework;

namespace TowerDefence
{
    public class GameStateStart : FSMState
    {
        private StartPanel startPanel; 
        public override void OnEnter()
        {
            base.OnEnter();
            LogUtil.Log("进入Start状态");
            Module module = userData as Module;
            if (module == null) throw new System.Exception("模块为空");
            startPanel = module.LoadPanel<StartPanel>();

            module.LoadController<AudioController>().PlayBGM(AudioConst.BGM_MAIN);
        }

        public override void OnExit()
        {
            base.OnExit();
            if (startPanel != null)
            {
                startPanel.Close();
                startPanel = null;
            }
        }
    }
}

