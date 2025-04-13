using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XFABManager;
using XFFSM;
using XFGameFramework;

namespace TowerDefence
{
    public class GamingReady : FSMState
    {
        Module module;
        private SceneLoadingPanel sceneLoadingPanel;
        private FightPanel fightPanel;
        public override void OnEnter()
        {
            base.OnEnter();
            module = userData as Module;
            Debug.LogFormat("GamingReady OnEnter:关卡{0}",module.LoadController<GameController>().GetCurrentPlayLevelID());
            controller.StartCoroutine(LoadingLevel());
        }

        public override void OnExit()
        {
            base.OnExit();
            Debug.Log("GamingReady OnExit");
        }

        private IEnumerator LoadingLevel()
        {
            int currentPlayLevelID = module.LoadController<GameController>().GetCurrentPlayLevelID();
            LevelData currentPlayLevelData = module.LoadController<LevelController>().GetLevelInfo(currentPlayLevelID);
            if(currentPlayLevelData == null)
            {
                throw new System.Exception(string.Format("关卡配置为空,关卡{0}", currentPlayLevelData));
            }

            bool isFinish = false;

            sceneLoadingPanel = module.LoadPanel<SceneLoadingPanel>(UIType.DontDestroyUI);

            module.LoadController<SceneController>().LoadScene(currentPlayLevelData.sceneName, () =>
            {
                isFinish = true;
                sceneLoadingPanel.UpdateProgressBar(1);
            }, (progress) =>
            {
                sceneLoadingPanel.UpdateProgressBar(progress);
            });

            //场景加载同步
            while(isFinish == false || (sceneLoadingPanel!=null && sceneLoadingPanel.IsProgressDone()==false)) yield return null;
            //加载面板UI
            if(fightPanel != null)
            {
                fightPanel.Close();
                fightPanel = null;
            }
            fightPanel = module.LoadPanel<FightPanel>();
            //初始化战斗中所有开始战斗的按钮
            module.LoadController<FightController>().InitScene(SceneManager.GetSceneByName(currentPlayLevelData.sceneName));
            //找到并打开第一波敌人的按钮
            EnemyWave enemyFirstWave = currentPlayLevelData.enemyWaves[0];
            FightButton firstWaveFightBtn = module.LoadController<FightController>().GetFightButton(enemyFirstWave.fightBtnName);
            firstWaveFightBtn.Show();
            firstWaveFightBtn.UpdateFightProgress(1);
            firstWaveFightBtn.AddClick(() =>
            {
                LogUtil.Log("点击了开始战斗按钮");
                module.LoadController<GameController>().SetGameState(EGameState.GamingFight);
                firstWaveFightBtn.Hide();
            });
            //关闭场景切换的过渡界面
            if (sceneLoadingPanel != null)
            {
                sceneLoadingPanel.Close();
                sceneLoadingPanel = null;
            }

            module.LoadController<AudioController>().PlayBGM(currentPlayLevelData.bgmName);
        }
    }
}

