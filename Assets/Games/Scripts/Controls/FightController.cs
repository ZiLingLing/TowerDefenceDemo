using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Splines;
using XFGameFramework;

namespace TowerDefence
{
    /// <summary>
    /// ��Ϸս���߼�Control
    /// </summary>
    public class FightController : Controller
    {
        private Dictionary<string, FightButton> fightBtnDic = new Dictionary<string, FightButton>();
        private Dictionary<string, SplineContainer> pathDic = new Dictionary<string, SplineContainer>();
        private TowerView[] towerList = null;

        private List<Enemy> enemyList = new List<Enemy>();
        private float waveTimer = 0;//ս�����μ��

        public void InitScene(Scene scene)
        {
            //��ʼ��·��
            InitPath(scene);
            //��ʼ������λ��
            InitTowerPosition(scene);
            //��ʼ����ť
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
                    throw new System.Exception(string.Format("FightButton�����ظ�:{0}", path.name));
                }
                pathDic.Add(path.name, path);
            }
        }

        private void InitTowerPosition(Scene scene)
        {
            GameObject towerObjNode = scene.FindRootGameObject("TowerPosition");
            if (towerObjNode == null) throw new System.Exception("��ǰ������δ�ҵ��½�������λ��");
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
                    throw new System.Exception(string.Format("FightButton�����ظ�:{0}", fightButton.name));
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
        /// ���ɵ���
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public IEnumerator GenerateEnemy()
        {
            int levelID = Module.LoadController<GameController>().GetCurrentPlayLevelID();
            LevelData levelData = Module.LoadController<LevelController>().GetLevelInfo(levelID);
            if(levelData == null)
            {
                throw new System.Exception(string.Format("��ѯ�ؿ���Ϣʧ��:levelID{0}", levelID));
            }
            enemyList.Clear();
            //��Ҫ���ɵĵ��˵�����
            List<EnemyData> enemyDataList = new List<EnemyData>();
            //���˲����߼�
            for(int i = 0; i < levelData.enemyWaves.Count; i++)
            {
                //����ս������
                GetFightModel().currentFightWave = i + 1;
                //���Ͳ����ı��¼�
                EventManager.TriggerEvent(EventConst.ON_ENEMY_WAVE_CHANGE);

                EnemyWave wave = levelData.enemyWaves[i];
                enemyDataList.AddRange(wave.enemyWavesData);

                if (i != 0)
                {
                    //��ʼ������һ�����˵ȴ�����ʱ�Ĳ��ŵ���Ч����ζ����һ�������������
                    Module.LoadController<AudioController>().PlaySound(AudioConst.SOUND_NEXT_WAVE_READY);

                    FightButton fightButton = GetFightButton(wave.fightBtnName);
                    if(fightButton == null)
                    {
                        throw new System.Exception(string.Format("��ѯս����ťʧ��:{0}", wave.fightBtnName));
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
                        //������һ������ʱ�䣬���°�ťUI�ϵĽ�����
                        fightButton.UpdateFightProgress(waveTimer / Module.LoadController<LevelController>().GetGenerateEnemyTimeInterval());
                    }
                    fightButton.Hide();
                }
                //ÿһ���ĵ��������߼�
                float timer = 0;//ͬһ�������ɵ��˼��
                //��һ�����˳���ʱ���ŵ���Ч
                Module.LoadController<AudioController>().PlaySound(AudioConst.SOUND_WAVE_INCOMING);
                //Ĭ�ϵ��˲㼶
                int sortingOrder = 0;
                while (enemyDataList.Count > 0)
                {
                    yield return null;
                    timer += Time.deltaTime;

                    for (int j = 0; j < enemyDataList.Count; j++)
                    {
                        if (enemyDataList[j].time > timer) continue;

                        //��������
                        SplineContainer path = GetPath(enemyDataList[j].path);
                        Enemy enemy = Module.LoadController<EnemyController>().CreateEnemy(enemyDataList[j].enemyID, path);
                        enemyDataList.RemoveAt(j);
                        j--;

                        enemyList.Add(enemy);
                        //���õ��˲㼶
                        enemy.SetEnemySortingOrder(sortingOrder);
                        sortingOrder++;
                    }
                }
                yield return new WaitForSeconds(3f);
            }
            //�������е���������ɣ��ȴ����е�������
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
            //�����е��˶�����ʱ
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
            //���½��UI
            EventManager.TriggerEvent(EventConst.ON_PLAYER_COIN_COUNT_CHANGE);
        }

        public void ClearData()
        {
            //��յ���
            ClearEnemy();
            //���ս������
            ClearFightModel();
            //�������
            Module.LoadController<TowerController>().ClearTower();
            //�����������
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
