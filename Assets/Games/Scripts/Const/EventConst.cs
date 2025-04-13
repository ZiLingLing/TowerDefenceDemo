using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence
{
    public class EventConst
    {
        /// <summary>
        /// 当敌人波数发生变化
        /// </summary>
        public const string ON_ENEMY_WAVE_CHANGE = "ON_ENEMY_WAVE_CHANGE";

        /// <summary>
        /// 当玩家金币数量发生改变
        /// </summary>
        public const string ON_PLAYER_COIN_COUNT_CHANGE = "ON_PLAYER_COIN_COUNT_CHANGE";

        /// <summary>
        /// 当玩家血量发生改变
        /// </summary>
        public const string ON_PLAYER_HP_CHANGE = "ON_PLAYER_HP_CHANGE";
    }
}

