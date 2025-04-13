using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFGameFramework;

namespace TowerDefence
{
    public class GameModel : Model
    {
        /// <summary>
        /// ��ǰѡ��浵�±�
        /// </summary>
        public int archiveIndex;

        /// <summary>
        /// ��ǰ��Ϸ״̬
        /// </summary>
        public EGameState gameState = EGameState.Start;

        /// <summary>
        /// ��������Ĺؿ�
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


