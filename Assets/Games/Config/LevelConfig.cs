using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence
{
    /// <summary>
    /// 敌人数据
    /// </summary>
    [Serializable]
    public class EnemyData
    {
        [Tooltip("敌人id")]
        public int enemyID;

        /// <summary>
        /// 敌人出击时间
        /// </summary>
        [Tooltip("敌人出击时间")]
        public int time;

        /// <summary>
        /// 敌人进攻路径
        /// </summary>
        [Tooltip("敌人进攻路线")]
        public string path;
    }

    /// <summary>
    /// 敌人波次信息
    /// </summary>
    [Serializable]
    public class EnemyWave
    {
        /// <summary>
        /// 每一波敌人的配置列表
        /// </summary>
        public List<EnemyData> enemyWavesData = new List<EnemyData>();

        /// <summary>
        /// 敌人出击按钮的名字
        /// </summary>
        public string fightBtnName;
    }

    /// <summary>
    /// 关卡配置数据
    /// </summary>
    [Serializable]
    public class LevelData
    {
        /// <summary>
        /// 关卡ID
        /// </summary>
        public int levelID;

        /// <summary>
        /// 关卡名字
        /// </summary>
        public string name;

        /// <summary>
        /// 关卡描述
        /// </summary>
        public string description;

        /// <summary>
        /// 当前关卡使用的场景的名称
        /// </summary>
        public string sceneName;

        /// <summary>
        /// 敌人波次信息
        /// </summary>
        public List<EnemyWave> enemyWaves = new List<EnemyWave>();

        /// <summary>
        /// 当前关卡初始金币数量
        /// </summary>
        public int initCoinCount = 300;

        /// <summary>
        /// 背景音乐名称
        /// </summary>
        public string bgmName = "";
    }

    [Serializable]
    public class LevelConfig : ScriptableObject
    {
        /// <summary>
        /// 所有关卡信息集合
        /// </summary>
        public List<LevelData> levels = new List<LevelData>();

        private Dictionary<int, LevelData> levelsDic = new Dictionary<int, LevelData>();

        /// <summary>
        /// 出售炮塔的折扣
        /// </summary>
        [Tooltip("出售炮塔折扣")]
        public float sellDiscount = 0.7f;

        /// <summary>
        /// 生成敌人时间间隔
        /// </summary>
        [Tooltip("一波敌人之后，生成下一波敌人的时间间隔")]
        public float generateEnemyTimeInterval = 10f;

        /// <summary>
        /// 玩家血量
        /// </summary>
        public int playerHp = 20;
        public LevelData GetLevelData(int levelID)
        {
            if (levelsDic.Count == 0)
            {
                foreach(var level in levels)
                {
                    if (levelsDic.ContainsKey(level.levelID))
                    {
                        throw new Exception(string.Format("LevelConfig id重复:{0}", level.levelID));
                    }
                    levelsDic.Add(level.levelID, level);
                }
            }

            if (levelsDic.ContainsKey(levelID))
            {
                return levelsDic[levelID];
            }
            return null;
        }
    }

}
