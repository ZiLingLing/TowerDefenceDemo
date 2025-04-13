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
    /// 关卡进度信息
    /// </summary>
    public class LevelInfo
    {
        /// <summary>
        /// 已解锁的关卡ID
        /// </summary>
        public int levelId;

        /// <summary>
        /// 关卡星星收集数量
        /// </summary>
        public int starNum;
    }
}

