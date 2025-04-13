using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TowerDefence
{
    public class ConfigMenu
    {
        [MenuItem("Assets/Create/Configs/LevelConfig")]
        private static void CreateLevelConfig()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (AssetDatabase.IsValidFolder(path) == false)
            {
                path = System.IO.Path.GetDirectoryName(path);
            }

            LevelConfig levelConfig = ScriptableObject.CreateInstance<LevelConfig>();
            AssetDatabase.CreateAsset(levelConfig, string.Format("{0}/LevelConfig.asset", path));
        }

        [MenuItem("Assets/Create/Configs/EnemiesConfig")]
        private static void CreateEnemiesConfig()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (AssetDatabase.IsValidFolder(path) == false)
            {
                path = System.IO.Path.GetDirectoryName(path);
            }

            EnemiesConfig levelConfig = ScriptableObject.CreateInstance<EnemiesConfig>();
            AssetDatabase.CreateAsset(levelConfig, string.Format("{0}/EnemiesConfig.asset", path));
        }

        [MenuItem("Assets/Create/Configs/TowersConfig")]
        private static void CreateTowersConfig()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (AssetDatabase.IsValidFolder(path) == false)
            {
                path = System.IO.Path.GetDirectoryName(path);
            }

            TowersConfig levelConfig = ScriptableObject.CreateInstance<TowersConfig>();
            AssetDatabase.CreateAsset(levelConfig, string.Format("{0}/TowersConfig.asset", path));
        }
    }
}

