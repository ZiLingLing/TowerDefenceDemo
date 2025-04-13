using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFGameFramework;

namespace TowerDefence
{
    public class OnFileModel : Model
    {
        public Dictionary<int, LevelInfo> levels = new Dictionary<int, LevelInfo>();

        public int AllStarNum
        {
            get
            {
                int count = 0;
                foreach(var item in levels.Values)
                {
                    count += item.starNum;
                }
                return count;
            }
        }
    }

    /// <summary>
    /// �ؿ�������Ϣ
    /// </summary>
    public class LevelInfo
    {
        /// <summary>
        /// �ѽ����Ĺؿ�ID
        /// </summary>
        public int levelId;

        /// <summary>
        /// �ؿ������ռ�����
        /// </summary>
        public int starNum;
    }
}

