using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using XFABManager;
using XFGameFramework;

namespace TowerDefence
{
    public class EnemyController : Controller
    {
        /// <summary>
        /// 获取敌人配置信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EnemyInfo GetEnemyInfo(int id)
        {
            return AssetBundleManager.LoadAsset<EnemiesConfig>(Module.ProjectName,"EnemiesConfig").GetEnemyInfo(id);
        }

        public Enemy CreateEnemy(int id, SplineContainer path)
        {
            EnemyInfo enemyInfo = GetEnemyInfo(id);
            if(enemyInfo == null)
            {
                throw new System.Exception(string.Format("未查询到敌人信息:id{0}"));
            }

            return Module.LoadView<Enemy>(enemyInfo.prefab_name, null, id, path);
        }
    }
}

