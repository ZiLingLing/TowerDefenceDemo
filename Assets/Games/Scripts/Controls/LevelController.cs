using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFABManager;
using XFGameFramework;

namespace TowerDefence
{
    public class LevelController : Controller
    {
        /// <summary>
        /// ��ȡĳһ�عؿ�����
        /// </summary>
        /// <param name="levelID"></param>
        /// <returns></returns>
        public LevelData GetLevelInfo(int levelID)
        {
            return AssetBundleManager.LoadAsset<LevelConfig>(Module.ProjectName, "LevelConfig").GetLevelData(levelID);
        }

        /// <summary>
        /// ��ȡĳҳ�ؿ�����
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<LevelData> GetLevelsByPage(int pageIndex, int size)
        {
            if (pageIndex <= 0) throw new Exception("ҳ�벻��С�ڵ���0");
            if (size <= 0) throw new Exception("�ؿ�������С��1");

            List<LevelData> levels = new List<LevelData>();
            for(int i = (pageIndex-1)*size; i < pageIndex*size; i++)
            {
                if (i >= AssetBundleManager.LoadAsset<LevelConfig>(Module.ProjectName, "LevelConfig").levels.Count)
                    break;

                levels.Add(AssetBundleManager.LoadAsset<LevelConfig>(Module.ProjectName, "LevelConfig").levels[i]);
            }
            return levels;
        }

        /// <summary>
        /// �ж��Ƿ������һҳ
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public bool IsLastPage(int pageIndex, int size)
        {
            return pageIndex*size >= AssetBundleManager.LoadAsset<LevelConfig>(Module.ProjectName, "LevelConfig").levels.Count;
        }

        /// <summary>
        /// ��ȡ�ؿ�����
        /// </summary>
        /// <returns></returns>
        public int GetLevelCount()
        {
            return AssetBundleManager.LoadAsset<LevelConfig>(Module.ProjectName, "LevelConfig").levels.Count;
        }

        public float GetGenerateEnemyTimeInterval()
        {
            return AssetBundleManager.LoadAsset<LevelConfig>(Module.ProjectName, "LevelConfig").generateEnemyTimeInterval;
        }

        public float GetPlayerHp()
        {
            return AssetBundleManager.LoadAsset<LevelConfig>(Module.ProjectName, "LevelConfig").playerHp;
        }

        public float GetSellDiscount()
        {
            return AssetBundleManager.LoadAsset<LevelConfig>(Module.ProjectName, "LevelConfig").sellDiscount;
        }
    }
}

