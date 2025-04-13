using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFFSM;
using XFGameFramework;

namespace TowerDefence
{
    public class GameStateSelectLevel : FSMState
    {
        private SelectLevelPanel selectLevelPanel;
        private SceneLoadingPanel sceneLoadingPanel;

        public override void OnEnter()
        {
            base.OnEnter();
            Module module = userData as Module;
            if (module == null) throw new System.Exception("FSMµÄuserDataÎª¿Õ");
            selectLevelPanel = module.LoadPanel<SelectLevelPanel>();

            module.LoadController<SceneController>().LoadScene("Start", () =>
            {
                selectLevelPanel = module.LoadPanel<SelectLevelPanel>();
                if(sceneLoadingPanel != null)
                {
                    TimerManager.DelayInvoke(() =>
                    {
                        sceneLoadingPanel.Close();
                        sceneLoadingPanel = null;
                    },1f);
                }
            }, (progress) =>
            {
                if(sceneLoadingPanel == null)
                {
                    sceneLoadingPanel = module.LoadPanel<SceneLoadingPanel>(UIType.DontDestroyUI);
                }
                sceneLoadingPanel.UpdateProgressBar(progress);
            });

            module.LoadController<AudioController>().PlayBGM(AudioConst.BGM_MAIN);
        }

        public override void OnExit()
        {
            base.OnExit();
            if (selectLevelPanel == null) return;
            selectLevelPanel.Close();
            selectLevelPanel = null;
        }
    }
}

