using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence
{
    /// <summary>
    /// ��������
    /// </summary>
    [Serializable]
    public class EnemyData
    {
        [Tooltip("����id")]
        public int enemyID;

        /// <summary>
        /// ���˳���ʱ��
        /// </summary>
        [Tooltip("���˳���ʱ��")]
        public int time;

        /// <summary>
        /// ���˽���·��
        /// </summary>
        [Tooltip("���˽���·��")]
        public string path;
    }

    /// <summary>
    /// ���˲�����Ϣ
    /// </summary>
    [Serializable]
    public class EnemyWave
    {
        /// <summary>
        /// ÿһ�����˵������б�
        /// </summary>
        public List<EnemyData> enemyWavesData = new List<EnemyData>();

        /// <summary>
        /// ���˳�����ť������
        /// </summary>
        public string fightBtnName;
    }

    /// <summary>
    /// �ؿ���������
    /// </summary>
    [Serializable]
    public class LevelData
    {
        /// <summary>
        /// �ؿ�ID
        /// </summary>
        public int levelID;

        /// <summary>
        /// �ؿ�����
        /// </summary>
        public string name;

        /// <summary>
        /// �ؿ�����
        /// </summary>
        public string description;

        /// <summary>
        /// ��ǰ�ؿ�ʹ�õĳ���������
        /// </summary>
        public string sceneName;

        /// <summary>
        /// ���˲�����Ϣ
        /// </summary>
        public List<EnemyWave> enemyWaves = new List<EnemyWave>();

        /// <summary>
        /// ��ǰ�ؿ���ʼ�������
        /// </summary>
        public int initCoinCount = 300;

        /// <summary>
        /// ������������
        /// </summary>
        public string bgmName = "";
    }

    [Serializable]
    public class LevelConfig : ScriptableObject
    {
        /// <summary>
        /// ���йؿ���Ϣ����
        /// </summary>
        public List<LevelData> levels = new List<LevelData>();

        private Dictionary<int, LevelData> levelsDic = new Dictionary<int, LevelData>();

        /// <summary>
        /// �����������ۿ�
        /// </summary>
        [Tooltip("���������ۿ�")]
        public float sellDiscount = 0.7f;

        /// <summary>
        /// ���ɵ���ʱ����
        /// </summary>
        [Tooltip("һ������֮��������һ�����˵�ʱ����")]
        public float generateEnemyTimeInterval = 10f;

        /// <summary>
        /// ���Ѫ��
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
                        throw new Exception(string.Format("LevelConfig id�ظ�:{0}", level.levelID));
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
