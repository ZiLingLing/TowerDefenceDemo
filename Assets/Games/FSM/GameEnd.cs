using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFFSM;
using XFGameFramework;

namespace TowerDefence
{
    public class GameEnd : FSMState
    {
        private Module module;

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("GameEnd OnEnter");
            module = userData as Module;
            if(module == null)
            {
                throw new System.Exception("��ģ��Ϊ��");
            }

            Time.timeScale = 0;
            FightModel fightModel = module.LoadController<FightController>().GetFightModel();
            if(fightModel.playerHp > 0)
            {
                //ʤ����Ч
                module.LoadController<AudioController>().PlaySound(AudioConst.SOUND_WAVE_INCOMING);
                //��������
                module.LoadController<OnFileController>().SaveCurrentPassLevelInfo();
                module.LoadPanel<WinPanel>();
            }
            else
            {
                //ʧ��
                module.LoadPanel<FailPanel>();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            Debug.Log("GameEnd OnExit");

            Time.timeScale = 1;
            module.LoadController<FightController>().ClearData();
        }
    }
}

