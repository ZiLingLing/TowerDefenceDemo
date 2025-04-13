using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Splines;
using XFGameFramework;

namespace TowerDefence
{
    /// <summary>
    /// 游戏战斗逻辑Control
    /// </summary>
    public class FightController : Controller
    {
        private Dictionary<string, FightButton> fightBtnDic = new Dictionary<string, FightButton>();
        private Dictionary<string, SplineContainer> pathDic = new Dictionary<string, SplineContainer>();
        private TowerView[] towerList = null;

        private List<Enemy> enemyList = new List<Enemy>();
        private float waveTimer = 0;//战斗波次间隔

        public void InitScene(Scene scene)
        {
            //初始化路径
            InitPath(scene);
            //初始化炮塔位置
            InitTowerPosition(scene);
            //初始化按钮
            InitFightBtnDic(scene);
        }
        private void InitPath(Scene scene)
        {
            pathDic.Clear();
            GameObject pathNode = scene.FindRootGameObject("Paths");
            SplineContainer[] pathList = pathNode.GetComponentsInChildren<SplineContainer>();
            foreach (SplineContainer path in pathList)
            {
                if (pathDic.ContainsKey(path.name))
                {
                    throw new System.Exception(string.Format("FightButton名字重复:{0}", path.name));
                }
                pathDic.Add(path.name, path);
            }
        }

        private void InitTowerPosition(Scene scene)
        {
            GameObject towerObjNode = scene.FindRootGameObject("TowerPosition");
            if (towerObjNode == null) throw new System.Exception("当前场景中未找到新建炮塔的位置");
            towerList = towerObjNode.GetComponentsInChildren<TowerView>();
            foreach(TowerView item in towerList)
            {
                item.InitModule(Module);
            }
        }

        private void InitFightBtnDic(Scene scene)
        {
            fightBtnDic.Clear();
            GameObject fightButtonListNode = scene.FindRootGameObject("FightButtonList");

            for(int i = 0; i < fightButtonListNode.transform.childCount; i++)
            {
                Transform child = fightButtonListNode.transform.GetChild(i);
                if (child == null) continue;
                FightButton fightButton = child.GetComponent<FightButton>();
                if (fightButton == null) continue;
                if (fightBtnDic.ContainsKey(fightButton.name))
                {
                    throw new System.Exception(string.Format("FightButton名字重复:{0}", fightButton.name));
                }
                fightButton.InitModule(Module);
                fightBtnDic.Add(fightButton.name, fightButton);
            }
        }

        public FightButton GetFightButton(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            if (fightBtnDic.ContainsKey(name))
            {
                return fightBtnDic[name];
            }
            return null;
        }

        public SplineContainer GetPath(string pathName)
        {
            if (string.IsNullOrEmpty(pathName)) return null;
            if (pathDic.ContainsKey(pathName))
            {
                return pathDic[pathName];
            }
            return null;
        }

        /// <summary>
        /// 生成敌人
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public IEnumerator GenerateEnemy()
        {
            int levelID = Module.LoadController<GameController>().GetCurrentPlayLevelID();
            LevelData levelData = Module.LoadController<LevelController>().GetLevelInfo(levelID);
            if(levelData == null)
            {
                throw new System.Exception(string.Format("查询关卡信息失败:levelID{0}", levelID));
            }
            enemyList.Clear();
            //需要生成的敌人的数据
            List<EnemyData> enemyDataList = new List<EnemyData>();
            //敌人波次逻辑
            for(int i = 0; i < levelData.enemyWaves.Count; i++)
            {
                //保存战斗波数
                GetFightModel().currentFightWave = i + 1;
                //发送波数改变事件
                EventManager.TriggerEvent(EventConst.ON_ENEMY_WAVE_CHANGE);

                EnemyWave wave = levelData.enemyWaves[i];
                enemyDataList.AddRange(wave.enemyWavesData);

                if (i != 0)
                {
                    //开始进入下一波敌人等待环节时的播放的音效，意味着这一波敌人生成完成
                    Module.LoadController<AudioController>().PlaySound(AudioConst.SOUND_NEXT_WAVE_READY);

                    FightButton fightButton = GetFightButton(wave.fightBtnName);
                    if(fightButton == null)
                    {
                        throw new System.Exception(string.Format("查询战斗按钮失败:{0}", wave.fightBtnName));
                    }
                    fightButton.Show();
                    fightButton.AddClick(() =>
                    {
                        waveTimer = Module.LoadController<LevelController>().GetGenerateEnemyTimeInterval();
                    });
                    waveTimer = 0;
                    while(waveTimer < Module.LoadController<LevelController>().GetGenerateEnemyTimeInterval())
                    {
                        yield return null;
                        waveTimer += Time.deltaTime;
                        //根据下一波出怪时间，更新按钮UI上的进度条
                        fightButton.UpdateFightProgress(waveTimer / Module.LoadController<LevelController>().GetGenerateEnemyTimeInterval());
                    }
                    fightButton.Hide();
                }
                //每一波的敌人生成逻辑
                float timer = 0;//同一波中生成敌人间隔
                //这一波敌人出现时播放的音效
                Module.LoadController<AudioController>().PlaySound(AudioConst.SOUND_WAVE_INCOMING);
                //默认敌人层级
                int sortingOrder = 0;
                while (enemyDataList.Count > 0)
                {
                    yield return null;
                    timer += Time.deltaTime;

                    for (int j = 0; j < enemyDataList.Count; j++)
                    {
                        if (enemyDataList[j].time > timer) continue;

                        //创建敌人
                        SplineContainer path = GetPath(enemyDataList[j].path);
                        Enemy enemy = Module.LoadController<EnemyController>().CreateEnemy(enemyDataList[j].enemyID, path);
                        enemyDataList.RemoveAt(j);
                        j--;

                        enemyList.Add(enemy);
                        //设置敌人层级
                        enemy.SetEnemySortingOrder(sortingOrder);
                        sortingOrder++;
                    }
                }
                yield return new WaitForSeconds(3f);
            }
            //至此所有敌人生成完成，等待所有敌人死亡
            while (enemyList.Count > 0)
            {
                for(int j = 0; j < enemyList.Count; j++)
                {
                    Enemy enemy = enemyList[j];
                    if(enemy == null || enemy.gameObject.activeSelf == false)
                    {
                        enemyList.RemoveAt(j);
                        j--;
                    }
                }
                yield return null;
            }
            //当所有敌人都死亡时
            Module.LoadController<GameController>().SetGameState(EGameState.GameEnd);
        }

        public FightModel GetFightModel()
        {
            FightModel fightModel = Module.LoadModel<FightModel>();
            if(fightModel == null)
            {
                fightModel = new FightModel();
                Module.AddModel(fightModel);
            }

            return fightModel;
        }

        public void PlayerHurt()
        {
            FightModel fightModel = GetFightModel();
            fightModel.playerHp--;
            if(fightModel.playerHp <= 0)
            {
                Module.LoadController<GameController>().SetGameState(EGameState.GameEnd);
            }

            EventManager.TriggerEvent(EventConst.ON_PLAYER_HP_CHANGE);

            Module.LoadController<AudioController>().PlaySound(AudioConst.SOUND_LOSE_LIFE);
        }

        public void IncreaseCoin(int coin)
        {
            FightModel fightModel = GetFightModel();
            fightModel.playerCoin += coin;
            EventManager.TriggerEvent(EventConst.ON_PLAYER_COIN_COUNT_CHANGE);
        }

        public void DecreaseCoin(int coin)
        {
            FightModel fightModel = GetFightModel();
            fightModel.playerCoin -= coin;
            if (fightModel.playerCoin < 0)
            {
                fightModel.playerCoin = 0;
            }
            //更新金币UI
            EventManager.TriggerEvent(EventConst.ON_PLAYER_COIN_COUNT_CHANGE);
        }

        public void ClearData()
        {
            //清空敌人
            ClearEnemy();
            //清空战斗数据
            ClearFightModel();
            //清空炮塔
            Module.LoadController<TowerController>().ClearTower();
            //清空炮塔底座
            ClearTowerView();
        }

        public void ClearEnemy()
        {
            while (enemyList.Count > 0)
            {
                Enemy enemy = enemyList[enemyList.Count - 1];
                enemy.Close();
                enemyList.Remove(enemy);
            }
        }

        public void ClearFightModel()
        {
            FightModel fightModel = GetFightModel();
            Module.RemoveModel(fightModel);
        }

        public void ClearTowerView()
        {
            foreach(TowerView item in towerList)
            {
                item.Tower = null;
            }
        }

        public void Replay()
        {
            ClearData();
            Module.LoadController<GameController>().SetGameState(EGameState.GamingReady);
        }

        public int CalculateStarNum()
        {
            FightModel fightModel = GetFightModel();
            int starNumRes = 1;
            if (fightModel.playerHp >= 5)
            {
                starNumRes = 3;
            }else if(fightModel.playerHp >= 3)
            {
                starNumRes = 2;
            }
            return starNumRes;
        }
    }
}
