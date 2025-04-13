using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFABManager;
using XFFSM;
using XFGameFramework;

namespace TowerDefence
{
    public class GameController : Controller
    {
        public void SetArchiveIndex(int index)
        {
            GameModel gameModel = GetGameModel();
            gameModel.archiveIndex = index;
        }

        public int GetArchiveIndex()
        {
            return GetGameModel().archiveIndex;
        }

        public GameModel GetGameModel()
        {
            GameModel gameModel = Module.GetModel<GameModel>();
            if (gameModel == null)
            {
                gameModel = new GameModel();
                Module.AddModel(gameModel);
            }
            return gameModel;
        }

        public void SetGameState(EGameState gameState)
        {
            GetGameModel().gameState = gameState;

            //更新状态进入选择关卡状态
            FSMController controller = FSMController.GetFSM("GameState");

            if(controller == null)//如果获取到是空的则重新从AB包加载
            {
                RuntimeFSMController fsm = AssetBundleManager.LoadAsset<RuntimeFSMController>(Module.ProjectName, "GameState");
                controller = FSMController.StartupFSM("GameState", fsm, Module);
            }
            controller.SetInt("State", (int)gameState);
        }

        public int GetCurrentPlayLevelID()
        {
            return GetGameModel().currentPlayLevelID;
        }

        public void SetCurrentPlayLevelID(int currentPlayLevelID)
        {
            GetGameModel().currentPlayLevelID = currentPlayLevelID;
        }

        public bool IsHaveNextLevel()
        {
            int nextLevelID = GetCurrentPlayLevelID() + 1;
            return Module.LoadController<LevelController>().GetLevelInfo(nextLevelID) != null;
        }

        public void SwitchNextLevel()
        {
            if (IsHaveNextLevel() == false) return;
            int currentLevelID = GetCurrentPlayLevelID();
            SetCurrentPlayLevelID(currentLevelID + 1);
            SetGameState(EGameState.GamingReady);
        }
    }
}

