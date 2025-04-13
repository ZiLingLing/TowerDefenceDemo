using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFABManager;
using XFFSM;
using XFGameFramework;

namespace TowerDefence
{
    public class GamingFight : FSMState
    {
        private Module module;

        private Coroutine genertateEnemyCoroutine;

        public override void OnEnter()
        {
            base.OnEnter();
            module = userData as Module;
            if(module == null)
            {
                throw new System.Exception("战斗状态获取当前模块为空");
            }
            genertateEnemyCoroutine = CoroutineStarter.Start(module.LoadController<FightController>().GenerateEnemy());
            Debug.Log("GamingFight OnEnter");
        }

        public override void OnExit()
        {
            base.OnExit();
            if(genertateEnemyCoroutine != null)
            {
                CoroutineStarter.Stop(genertateEnemyCoroutine);
            }
            Debug.Log("GamingFight OnExit");
        }
    }
}

