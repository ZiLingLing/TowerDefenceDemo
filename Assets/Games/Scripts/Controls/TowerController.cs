using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFABManager;
using XFGameFramework;

namespace TowerDefence
{
    public class TowerController : Controller
    {
        private List<TowerBase> towerList = new List<TowerBase>();

        public TowerInfo GetTowerInfo(int id)
        {
            return AssetBundleManager.LoadAsset<TowersConfig>(Module.ProjectName, "TowersConfig").GetTowerInfo(id);
        }

        public bool CanCreateTower(int id)
        {
            TowerInfo towerInfo = GetTowerInfo(id);
            int playerCoin = Module.LoadController<FightController>().GetFightModel().playerCoin;

            return towerInfo.cost <= playerCoin;
        }

        public TowerBase CreateTower(int id,Transform parent)
        {
            TowerInfo towerInfo = GetTowerInfo(id);
            if (CanCreateTower(id) == false) return null;

            TowerBase tower = Module.LoadView<TowerBase>(towerInfo.prefab_name, parent, id);
            if(towerList.Contains(tower) == false)
            {
                towerList.Add(tower);
            }

            LogUtil.Log("资源名:{0} TowerBase:{1}", towerInfo.prefab_name, tower == null);

            Module.LoadController<FightController>().DecreaseCoin(towerInfo.cost);
            //播放音效
            Module.LoadController<AudioController>().PlaySound(AudioConst.SOUND_TOWER_BUILD);

            return tower;
        }

        public List<TowerInfo> GetTowerInfoByTowerType(TowerType towerType)
        {
            return AssetBundleManager.LoadAsset<TowersConfig>(Module.ProjectName, "TowersConfig").GetTowerInfoByTowerType(towerType);
        }

        public TowerInfo FindLowestLevelTowerInfo(TowerType towerType)
        {
            List<TowerInfo> towerInfoList = GetTowerInfoByTowerType(towerType);
            TowerInfo lowestLevelInfo = null;
            foreach(TowerInfo item in towerInfoList)
            {
                if(lowestLevelInfo == null)
                {
                    lowestLevelInfo = item;
                    continue;
                }
                if (lowestLevelInfo.level > item.level)
                {
                    lowestLevelInfo = item;
                }
            }
            return lowestLevelInfo;
        }

        public TowerInfo FindHighestLevelTowerInfo(TowerType towerType)
        {
            List<TowerInfo> towerInfoList = GetTowerInfoByTowerType(towerType);
            TowerInfo highestLevelInfo = null;
            foreach (TowerInfo item in towerInfoList)
            {
                if (highestLevelInfo == null)
                {
                    highestLevelInfo = item;
                    continue;
                }
                if (highestLevelInfo.level < item.level)
                {
                    highestLevelInfo = item;
                }
            }
            return highestLevelInfo;
        }

        public TowerInfo FindNextLevelTowerInfo(int id)
        {
            TowerInfo towerInfo = GetTowerInfo(id);
            if(towerInfo == null)
            {
                throw new System.Exception(string.Format("查询炮塔信息失败:{0}", id));
            }
            List<TowerInfo> towerInfoList = GetTowerInfoByTowerType(towerInfo.type);
            foreach(TowerInfo item in towerInfoList)
            {
                //同类炮塔中，等级比当前炮塔高一级的，对应下一级炮塔
                if(item.level - towerInfo.level == 1)
                {
                    return item;
                }
            }
            return null;
        }

        public void ClearTower()
        {
            while (towerList.Count > 0)
            {
                TowerBase tower = towerList[towerList.Count - 1];
                if (tower != null)
                {
                    tower.Close();
                }
                towerList.Remove(tower);
            }
            //清空已发射的箭矢
            Module.ClearView<Arrow>();
            //清空已投掷的石头
            Module.ClearView<Stone>();
        }
    }
}

