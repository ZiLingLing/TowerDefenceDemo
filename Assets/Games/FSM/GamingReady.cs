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
            Debug.LogFormat("GamingReady OnEnter:�ؿ�{0}",module.LoadController<GameController>().GetCurrentPlayLevelID());
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
                throw new System.Exception(string.Format("�ؿ�����Ϊ��,�ؿ�{0}", currentPlayLevelData));
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

            //��������ͬ��
            while(isFinish == false || (sceneLoadingPanel!=null && sceneLoadingPanel.IsProgressDone()==false)) yield return null;
            //�������UI
            if(fightPanel != null)
            {
                fightPanel.Close();
                fightPanel = null;
            }
            fightPanel = module.LoadPanel<FightPanel>();
            //��ʼ��ս�������п�ʼս���İ�ť
            module.LoadController<FightController>().InitScene(SceneManager.GetSceneByName(currentPlayLevelData.sceneName));
            //�ҵ����򿪵�һ�����˵İ�ť
            EnemyWave enemyFirstWave = currentPlayLevelData.enemyWaves[0];
            FightButton firstWaveFightBtn = module.LoadController<FightController>().GetFightButton(enemyFirstWave.fightBtnName);
            firstWaveFightBtn.Show();
            firstWaveFightBtn.UpdateFightProgress(1);
            firstWaveFightBtn.AddClick(() =>
            {
                LogUtil.Log("����˿�ʼս����ť");
                module.LoadController<GameController>().SetGameState(EGameState.GamingFight);
                firstWaveFightBtn.Hide();
            });
            //�رճ����л��Ĺ��ɽ���
            if (sceneLoadingPanel != null)
            {
                sceneLoadingPanel.Close();
                sceneLoadingPanel = null;
            }

            module.LoadController<AudioController>().PlayBGM(currentPlayLevelData.bgmName);
        }
    }
}

