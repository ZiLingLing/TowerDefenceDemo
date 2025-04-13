using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence
{
    [Serializable]
    public class EnemyInfo
    {
        [Tooltip("唯一标识")]
        public int id;
        [Tooltip("名称")]
        public string name;
        [Tooltip("描述")]
        public string description;
        [Tooltip("移动速度")]
        public float speed;
        [Tooltip("血量")]
        public int hp;
        [Tooltip("物理防御")]
        public int physical_defense;
        [Tooltip("魔法防御")]
        public int magical_defense;
        [Tooltip("预制体名称,用来创建敌人")]
        public string prefab_name;
        [Tooltip("死亡时掉落的金币")]
        public int drop_gold;
    }

    public class EnemiesConfig : ScriptableObject
    {
        public List<EnemyInfo> enemies;

        private Dictionary<int, EnemyInfo> enemies_dic = new Dictionary<int, EnemyInfo>();

        public EnemyInfo GetEnemyInfo(int id)
        {
            if (enemies_dic.Count == 0)
            {
                foreach (var item in enemies)
                {
                    if (!enemies_dic.ContainsKey(item.id))
                        enemies_dic.Add(item.id, item);
                    else
                        throw new Exception(string.Format("Enemies Config id 重复:{0}", item.id));
                }
            }
            if (enemies_dic.ContainsKey(id))
                return enemies_dic[id];
            return null;
        }

    }
}

