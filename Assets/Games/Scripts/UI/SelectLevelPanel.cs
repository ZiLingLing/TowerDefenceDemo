using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XFABManager;
using XFGameFramework;

namespace TowerDefence
{
    public class SelectLevelPanel : Panel
    {
        [SerializeField]
        private List<Transform> levelTransformList;

        [SerializeField]
        private Button btnLeft;
        [SerializeField]
        private Button btnRight;

        private int currentPage = 0;

        public void OnBtnLeftClick()
        {
            InitLevelPageView(currentPage - 1);
        }

        public void OnBtnRightClick()
        {
            InitLevelPageView(currentPage + 1);
        }

        public void OnBtnCloseClick()
        {
            Module.LoadController<GameController>().SetGameState(EGameState.Start);
            Close();
        }

        public override void OnLoaded(params object[] param)
        {
            base.OnLoaded(param);
            InitLevelPageView(1);
        }

        private void InitLevelPageView(int pageIndex)
        {
            if (currentPage == pageIndex) return;
            currentPage = pageIndex;

            btnLeft.interactable = pageIndex > 1;
            btnRight.interactable = !Module.LoadController<LevelController>().IsLastPage(pageIndex, levelTransformList.Count);

            List<LevelData> levelDataList = Module.LoadController<LevelController>().GetLevelsByPage(pageIndex, levelTransformList.Count);

            foreach(Transform levelTransform in levelTransformList)
            {
                levelTransform.gameObject.SetActive(false);
            }

            for(int i=0; i< levelDataList.Count; i++)
            {
                Transform levelTrans = levelTransformList[i];
                if (levelTrans == null) continue;

                levelTrans.gameObject.SetActive(true);

                Text levelNameText = levelTrans.Find("LevelName").GetComponent<Text>();
                ImageLoader imgLoaderStar = levelTrans.Find("Star").GetComponent<ImageLoader>();
                GameObject lockNode = levelTrans.Find("Lock").gameObject;
                Button selectLevelBtn = levelTrans.GetComponent<Button>();

                int levelID = levelDataList[i].levelID;//对应按钮的关卡id
                levelNameText.text = (levelID).ToString();

                int archiveIndex = Module.LoadController<GameController>().GetArchiveIndex();
                int starCount = 0;

                OnFileModel onFileModel = Module.LoadController<OnFileController>().Get(archiveIndex);
                if (onFileModel.levels.ContainsKey(levelID))
                {
                    starCount = onFileModel.levels[levelID].starNum;
                }

                imgLoaderStar.AssetName = string.Format("star_{0}", starCount + 1);

                bool isLock = Module.LoadController<OnFileController>().IsLockLevel(levelID);
                lockNode.SetActive(!isLock);

                selectLevelBtn.onClick.RemoveAllListeners();
                selectLevelBtn.onClick.AddListener(() =>
                {
                    if (isLock == false)
                    {
                        LogUtil.Log(string.Format("关卡{0}，当前关卡未解锁",levelID));
                        Module.LoadController<TipController>().ShowToast("当前关卡未解锁");
                        return;
                    }
                    //保存关卡id
                    Module.LoadController<GameController>().SetCurrentPlayLevelID(levelID);
                    //切换状态进入关卡
                    Module.LoadController<GameController>().SetGameState(EGameState.GamingReady);
                    LogUtil.Log("进入关卡:" + levelID);
                });
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            currentPage = 0;
        }
    }
}

