using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFGameFramework;

namespace TowerDefence
{
    public class OnFileController : Controller
    {
        public void Create(int index)
        {
            OnFileModel onFileModel = new OnFileModel();
            onFileModel.Id = index;
            Module.AddModel(onFileModel);

            string key = GetPrefabsKey(index);
            PlayerPrefs.SetString(key, JsonConvert.SerializeObject(onFileModel));
            PlayerPrefs.Save();
        }

        public void Delete(int index)
        {
            OnFileModel onFileModel = Module.GetModel<OnFileModel>(index);
            if(onFileModel != null)
            {
                Module.RemoveModel(onFileModel);
                string key = GetPrefabsKey(index);
                PlayerPrefs.DeleteKey(key);
                PlayerPrefs.Save();
            }
        }

        public void Update(int index)
        {
            OnFileModel onFileModel = Module.GetModel<OnFileModel>(index);
            if (onFileModel == null) return;

            string key = GetPrefabsKey(index);
            PlayerPrefs.SetString(key, JsonConvert.SerializeObject(onFileModel));
            PlayerPrefs.Save();
        }

        public OnFileModel Get(int index)
        {
            OnFileModel onFileModel = Module.GetModel<OnFileModel>(index);
            if (onFileModel != null) return onFileModel;
            string key = GetPrefabsKey(index);

            try
            {
                string content = PlayerPrefs.GetString(key);
                onFileModel = JsonConvert.DeserializeObject<OnFileModel>(content);
                Module.AddModel(onFileModel);
            }
            catch (System.Exception)
            {

            }

            return onFileModel;
        }

        private string GetPrefabsKey(int index)
        {
            return string.Format("OnFileController:OnFileModel:{0}", index);
        }

        public bool IsLockLevel(int levelID)
        {
            if (levelID == 1) return true;//第一关默认解锁

            int archiveIndex = Module.LoadController<GameController>().GetArchiveIndex();
            OnFileModel onFileModel = Get(archiveIndex);
            if (onFileModel.levels.ContainsKey(levelID) || onFileModel.levels.ContainsKey(levelID-1))
            {
                return true;
            }
            return false;
        }

        public void SaveCurrentPassLevelInfo()
        {
            int levelID = Module.LoadController<GameController>().GetCurrentPlayLevelID();
            OnFileModel onFileModel = Get(Module.LoadController<GameController>().GetArchiveIndex());

            int starNum = Module.LoadController<FightController>().CalculateStarNum();
            if(onFileModel.levels.ContainsKey(levelID) == true)
            {
                //更新星星数量
                if (onFileModel.levels[levelID].starNum < starNum)
                {
                    onFileModel.levels[levelID].starNum = starNum;
                }
            }
            else
            {
                //添加过关数据
                LevelInfo passLevelInfo = new LevelInfo();
                passLevelInfo.levelId = levelID;
                passLevelInfo.starNum = starNum;

                onFileModel.levels.Add(passLevelInfo.levelId, passLevelInfo);
            }

            Update(Module.LoadController<GameController>().GetArchiveIndex());
        }
    }
}

