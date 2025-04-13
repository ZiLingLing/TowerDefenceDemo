using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XFABManager;
using XFGameFramework;

namespace TowerDefence
{
    public class WinPanel : Panel
    {
        [SerializeField]
        private Button btnNextLevel;
        [SerializeField]
        private ImageLoader imageLoader;

        public override void OnLoaded(params object[] param)
        {
            base.OnLoaded(param);

            int archiveIndex = Module.LoadController<GameController>().GetArchiveIndex();
            int levelID = Module.LoadController<GameController>().GetCurrentPlayLevelID();

            int starNum = Module.LoadController<OnFileController>().Get(archiveIndex).levels[levelID].starNum;

            imageLoader.AssetName = string.Format("star_{0}", starNum + 1);

            btnNextLevel.interactable = Module.LoadController<GameController>().IsHaveNextLevel();
        }

        public void onBtnMenuClick()
        {
            Module.LoadController<GameController>().SetGameState(EGameState.SelectLevel);
        }

        public void onBtnNextLevelClick()
        {
            Module.LoadController<GameController>().SwitchNextLevel();
        }
    }
}

