using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XFABManager;

namespace TowerDefence
{
    public class LevelEnemyEditor
    {
        /// <summary>
        /// 将场景中的配置信息写入配置文件
        /// </summary>
        [MenuItem("Tool/LevelConfig/Write")]
        private static void LevelConfigWrite()
        {
            //添加关卡配置
            GameObject obj = Selection.activeGameObject;
            if (obj.name.StartsWith("level:") == false) return;

            int levelID = int.Parse(obj.name.Split(":")[1]);

            List<EnemyWave> enemyWave = new List<EnemyWave>();
            //添加每一关中每一波的配置
            for(int i = 0; i < obj.transform.childCount; i++)
            {
                Transform child = obj.transform.GetChild(i);
                if (child.name.StartsWith("wave") == false) continue;

                EnemyWave wave = new EnemyWave();
                string fightBtnName = child.name.Split(":")[1];
                wave.fightBtnName = fightBtnName;
                //添加每一波中每一条路线配置
                for(int j = 0; j < child.transform.childCount; j++)
                {
                    Transform childPath = child.transform.GetChild(j);
                    //添加每条路线中的每个敌人的配置
                    for(int k = 0; k < childPath.childCount; k++)
                    {
                        Transform enemy = childPath.GetChild(k);

                        EnemyData data = new EnemyData();
                        data.time = (int)Mathf.Abs(enemy.transform.localPosition.x);
                        data.enemyID = GetEnemyIDByPrefabName(enemy.name);
                        data.path = childPath.name;

                        wave.enemyWavesData.Add(data);
                    }
                }
                enemyWave.Add(wave);
            }
            LevelConfig levelConfig = AssetDatabase.LoadAssetAtPath<LevelConfig>("Assets/Games/Config/LevelConfig.asset");
            LevelData levelData = levelConfig.GetLevelData(levelID);
            levelData.enemyWaves = enemyWave;

            EditorUtility.SetDirty(levelConfig);
            Debug.Log("保存配置成功");
        }

        /// <summary>
        /// 读取配置文件中的信息,添加在场景中
        /// </summary>
        [MenuItem("Tool/LevelConfig/Read")]
        private static void LevelConfigRead()
        {
            GameObject obj = Selection.activeGameObject;
            if (obj.name.StartsWith("level:") == false) return;
            Debug.Log("读取成功");
            int levelID = int.Parse(obj.name.Split(":")[1]);
            LevelConfig levelConfig = AssetDatabase.LoadAssetAtPath<LevelConfig>("Assets/Games/Config/LevelConfig.asset");
            LevelData levelData = levelConfig.GetLevelData(levelID);

            for (int i= obj.transform.childCount - 1; i >= 0; i--)
            {
                GameObject childObj = obj.transform.GetChild(i).gameObject;
                GameObject.DestroyImmediate(childObj);
            }

            for(int i = 0; i < levelData.enemyWaves.Count; i++)
            {
                EnemyWave enemyWave = levelData.enemyWaves[i];

                GameObject waveObj = new GameObject(string.Format("wave{0}:{1}", i, enemyWave.fightBtnName));
                waveObj.transform.SetParent(obj.transform);
                //每一波最多有六个路径，记得给两波之间留足够空位
                waveObj.transform.localPosition = new Vector3(0, -i * 10, 0);

                for(int j = 0; j < enemyWave.enemyWavesData.Count; j++)
                {
                    EnemyData enemyData = enemyWave.enemyWavesData[j];

                    Transform path = waveObj.transform.Find(enemyData.path);
                    if(path == null)
                    {
                        GameObject pathObj = new GameObject(enemyData.path);
                        pathObj.transform.SetParent(waveObj.transform);
                        int pathNum = int.Parse(enemyData.path.Replace("Path",string.Empty));
                        pathObj.transform.localPosition = new Vector3(0, -1 * pathNum, 0);
                        path = pathObj.transform;
                    }
                    //实例化敌人预制体
                    EnemiesConfig enemiesConfig = AssetDatabase.LoadAssetAtPath<EnemiesConfig>("Assets/Games/Config/EnemiesConfig.asset");
                    EnemyInfo enemyInfo = enemiesConfig.GetEnemyInfo(enemyData.enemyID);

                    GameObject prefab = AssetBundleManager.LoadAsset<GameObject>("MainModule",enemyInfo.prefab_name);
                    GameObject enemyObj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                    enemyObj.name = prefab.name;
                    enemyObj.transform.SetParent(path);
                    enemyObj.transform.localPosition = new Vector3(-enemyData.time, 0, 0);
                }
            }
        }

        private static int GetEnemyIDByPrefabName(string name)
        {
            if(name.Contains(" "))
            {
                name = name.Split(" ")[0];
            }

            EnemiesConfig enemiesConfig = AssetDatabase.LoadAssetAtPath<EnemiesConfig>("Assets/Games/Config/EnemiesConfig.asset");
            foreach(EnemyInfo enemyInfo in enemiesConfig.enemies)
            {
                if (enemyInfo.name.Equals(name))
                {
                    return enemyInfo.id;
                }
            }
            return -1;
        }
    }
}

