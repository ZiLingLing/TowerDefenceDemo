using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFGameFramework;

namespace TowerDefence
{
    public class GameModel : Model
    {
        /// <summary>
        /// 当前选择存档下标
        /// </summary>
        public int archiveIndex;

        /// <summary>
        /// 当前游戏状态
        /// </summary>
        public EGameState gameState = EGameState.Start;

        /// <summary>
        /// 正在游玩的关卡
        /// </summary>
        public int currentPlayLevelID;
    }

    public enum EGameState
    {
        Start,
        SelectLevel,
        GamingReady,
        GamingFight,
        GameEnd,
    }
}


