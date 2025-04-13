using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using XFABManager;
using XFGameFramework;

namespace TowerDefence
{
    public class Stone : View
    {
        private bool isFlying = false;
        private float flyTimer = 0;

        [SerializeField]
        private float flyTime = 0.5f;

        private Enemy enemy;
        private int damage;

        private Vector3 startPosition;
        private Vector3 targetPosition;
        private Spline stonePath = new Spline();

        private Collider2D[] enemies = new Collider2D[20];

        public void Attack(Enemy enemy, int damage, Vector3 startPosition)
        {
            this.transform.SetParent(null);

            this.damage = damage;
            this.enemy = enemy;
            this.startPosition = startPosition;

            isFlying = true;
            flyTimer = 0;

            Module.LoadController<AudioController>().PlaySound(AudioConst.SOUND_STONE_RELEASE);
        }

        private void Fly()
        {
            if (isFlying == false) return;

            if (enemy != null && enemy.transform.gameObject.activeSelf == true)
            {
                targetPosition = enemy.transform.position;
            }

            if (enemy != null && enemy.Hp <= 0)
            {
                enemy = null;
            }

            flyTimer += Time.deltaTime;

            Vector3 direction = targetPosition - startPosition;
            Vector3 center = startPosition + direction * 0.5f;
            //踩坑：必须像下面这样忽略y轴，因为如果center.y为负数，targetPosition和startPosition的高度都小于-1.此时负数加负数会变得更小
            center.y = 0;
            //中间点的y必须比起始点和目标点中y大的那个更大
            Vector3 offset;
            if (direction.y >= 0)
            {
                offset = center + new Vector3(0, targetPosition.y + 1, 0);
            }
            else
            {
                offset = center + new Vector3(0, startPosition.y + 1, 0);
            }
            stonePath.Clear();
            stonePath.Add(new BezierKnot(startPosition), TangentMode.AutoSmooth);
            stonePath.Add(new BezierKnot(offset), TangentMode.AutoSmooth);
            stonePath.Add(new BezierKnot(targetPosition), TangentMode.AutoSmooth);

            this.transform.position = stonePath.EvaluatePosition(flyTimer / flyTime);

            if (flyTimer >= flyTime)
            {
                isFlying = false;
                //触碰到敌人，对敌人造成范围伤害
                OnExplode();
                if (enemy != null)
                {
                    enemy.OnHurt(damage, EDamageType.Physics);
                }
            }
        }

        private void OnExplode()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i] = null;
            }

            Physics2D.OverlapCircleNonAlloc(this.transform.position, 0.6f, enemies);
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] == null) continue;
                Enemy enemy = enemies[i].GetComponent<Enemy>();
                if (enemy == null) continue;
                enemy.OnHurt(damage, EDamageType.Physics);
            }

            GameObject explodeAnimObj = GameObjectLoader.Load(Module.ProjectName, "StoneExplode");
            explodeAnimObj.transform.position = this.transform.position;

            TimerManager.DelayInvoke(()=>{
                GameObjectLoader.UnLoad(explodeAnimObj);
            },0.3f);

            Close();
        }

        private void Update()
        {
            Fly();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            isFlying = false;
            enemy = null;
            flyTimer = 0;
        }
    }

}
