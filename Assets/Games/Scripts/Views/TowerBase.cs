using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFGameFramework;

namespace TowerDefence
{
    public class TowerBase : View
    {
        [SerializeField]
        protected List<Enemy> enemyList;

        [SerializeField]
        protected Transform rangeParent;

        [SerializeField]
        protected SpriteRenderer spriteRenderer;

        public int TowerID { get; private set; }
        private TowerInfo TowerInfo;

        private float readyTimer = 0;
        private bool isReady = false;
        public override void OnLoaded(params object[] param)
        {
            base.OnLoaded(param);
            if(param == null || param.Length == 0)
            {
                throw new System.Exception("����IDΪ�գ��봫������ID");
            }

            TowerID = (int)param[0];
            TowerInfo = Module.LoadController<TowerController>().GetTowerInfo(TowerID);
            if(TowerInfo == null)
            {
                throw new System.Exception(string.Format("δ������л�ȡ����������Ϣ:id{0}",TowerID));
            }

            rangeParent.transform.localScale = Vector3.one * TowerInfo.attack_range;
            HideAttackRange();

            readyTimer = 0;
            isReady = false;
        }

        public void OnCircleTriggerEnter2D(Collider2D collision)
        {
            LogUtil.Log("���˽��빥����Χ");
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy == null) return;

            if(enemyList.Contains(enemy) == false)
            {
                enemyList.Add(enemy);
            }

        }

        public void OnCircleTriggerExit2D(Collider2D collision)
        {
            LogUtil.Log("�����뿪������Χ");
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy == null) return;

            if (enemyList.Contains(enemy) == true)
            {
                enemyList.Remove(enemy);
            }

            if(enemyList.Count == 0)
            {
                OnAttackEnd();
            }
        }

        public void ShowAttackRange()
        {
            spriteRenderer.enabled = true;
        }

        public void HideAttackRange()
        {
            spriteRenderer.enabled = false;
        }

        protected virtual void OnAttack(Enemy enemy, int damage)
        {

        }

        protected virtual void OnAttackEnd()
        {

        }

        protected virtual void OnReady()
        {

        }

        /// <summary>
        /// ��������ص�������0-1��ʾ����
        /// </summary>
        /// <param name="progress"></param>
        protected virtual void OnPreparing(float progress)
        {

        }

        public bool CanAttack()
        {
            if(enemyList == null || enemyList.Count == 0)
            {
                return false;
            }
            return true;
        }

        private void Update()
        {
            if(isReady == true)
            {
                //���й���
                if(CanAttack() == true)
                {
                    int damage = Random.Range(TowerInfo.damage_min, TowerInfo.damage_max + 1);
                    OnAttack(enemyList[0], damage);
                    isReady = false;
                    readyTimer = 0;
                }
            }
            else
            {
                OnPreparing(readyTimer / TowerInfo.cooling_time);
                readyTimer += Time.deltaTime;
                if(readyTimer >= TowerInfo.cooling_time)
                {
                    readyTimer = 0;
                    isReady = true;
                    OnReady();
                }
            }
        }


    }
}


