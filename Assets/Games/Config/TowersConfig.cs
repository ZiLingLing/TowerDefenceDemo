using System;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence
{
    public enum TowerType
    {
        /// <summary>
        /// 弓箭手炮塔
        /// </summary>
        Archer,
        /// <summary>
        /// 魔法炮塔
        /// </summary>
        Magic,
        /// <summary>
        /// 巨石炮塔
        /// </summary>
        Rock
    }

    [Serializable]
    public class TowerInfo
    {
        [Tooltip("唯一标识")]
        public int id;
        [Tooltip("炮台名称")]
        public string name;
        [Tooltip("伤害的最小值")]
        public int damage_min;
        [Tooltip("伤害的最大值")]
        public int damage_max;
        [Tooltip("攻击范围")]
        public float attack_range;
        [Tooltip("冷却时间,越小攻击速度越快")]
        public float cooling_time;
        [Tooltip("建造需要消耗的金币")]
        public int cost;
        [Tooltip("炮塔等级")]
        public int level;
        [Tooltip("炮塔类型")]
        public TowerType type;
        [Tooltip("预制体名称")]
        public string prefab_name;
        [Tooltip("图标名称")]
        public string icon_name;

    }


    public class TowersConfig : ScriptableObject
    {
        public List<TowerInfo> towers = new List<TowerInfo>();

        private Dictionary<int, TowerInfo> towers_dic = new Dictionary<int, TowerInfo>();

        private Dictionary<TowerType, List<TowerInfo>> towers_info_dic = new Dictionary<TowerType, List<TowerInfo>>();

        public TowerInfo GetTowerInfo(int id)
        {
            if (towers_dic.Count == 0)
            {
                foreach (var item in towers)
                {
                    if (towers_dic.ContainsKey(item.id))
                        throw new Exception(string.Format("TowersConfig id 重复:{0}", item.id));
                    else
                        towers_dic.Add(item.id, item);
                }
            }

            if (towers_dic.ContainsKey(id))
                return towers_dic[id];

            return null;
        }

        public List<TowerInfo> GetTowerInfoByTowerType(TowerType towerType)
        {
            if(towers_info_dic.Count == 0)
            {
                foreach(var item in towers)
                {
                    if (towers_info_dic.ContainsKey(item.type))
                    {
                        towers_info_dic[item.type].Add(item);
                    }
                    else
                    {
                        towers_info_dic.Add(item.type, new List<TowerInfo>() { item });
                    }
                }
            }

            if (towers_info_dic.ContainsKey(towerType))
            {
                return towers_info_dic[towerType];
            }
            return null;
        }
    }

}

