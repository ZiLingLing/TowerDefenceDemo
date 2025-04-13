using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using XFGameFramework;

namespace TowerDefence
{
    public class Arrow : View
    {
        private Enemy enemy;
        private Vector3 startPosition;
        private Vector3 targetPosition;
        private int damage;

        private bool isFlying = false;
        private float flyTime = 0.3f;
        private float flyTimer = 0;

        private Spline arrowPath = new Spline();

        [SerializeField]
        private Animator animator;

        public void Attack(Enemy enemy, int damage, Vector3 startPosition)
        {
            this.enemy = enemy;
            this.startPosition = startPosition;
            this.damage = damage;

            isFlying = true;
            flyTimer = 0;

            string soundName = Random.Range(0, 2) == 0 ? AudioConst.SOUND_ARROW_RELEASE1 : AudioConst.SOUND_ARROW_RELEASE2;
            Module.LoadController<AudioController>().PlaySound(soundName);
        }

        //������ʧ�¼�
        public void OnDisappearFinish()
        {
            Close();
        }

        private void RecycleArrow()
        {
            animator.ResetTrigger("disappear");
            animator.SetTrigger("disappear");
        }

        private void Fly()
        {
            if (isFlying == false) return;
            //����Ŀ���λ�ã���Ϊ����һֱ�ڶ�
            if (enemy != null && enemy.transform.gameObject.activeSelf == true)
            {
                targetPosition = enemy.transform.position + Vector3.up * 0.3f;//����λ��Ӧ���ڵ��˵����ĵ�ƫ��һЩ��λ��
            }

            if (enemy != null && enemy.Hp <= 0)
            {
                enemy = null;
            }

            flyTimer += Time.deltaTime;

            //��������
            Vector3 direction = targetPosition - startPosition;
            Vector3 center = startPosition + direction * 0.2f;
            Vector3 offset = Vector3.Cross(direction, Vector3.forward).normalized * 0.3f;
            //����Ŀ���λ�øı�ƫ����
            if (direction.y >= 0)
            {
                float angle = Vector2.Angle(direction, Vector2.up);
                offset = angle / 90 * offset;
            }
            else
            {
                float angle = Vector2.Angle(direction, Vector2.down);
                offset = angle / 90 * offset;
            }
            arrowPath.Clear();
            arrowPath.Add(new BezierKnot(startPosition), TangentMode.AutoSmooth);
            if (direction.x < 0)
            {
                arrowPath.Add(new BezierKnot(center + offset), TangentMode.AutoSmooth);
            }
            else
            {
                arrowPath.Add(new BezierKnot(center - offset), TangentMode.AutoSmooth);
            }
            arrowPath.Add(new BezierKnot(targetPosition), TangentMode.AutoSmooth);
            //�����˶������и��½Ƕ�
            Vector3 currentDirection;
            if (direction.x > 0)
            {
                currentDirection = Vector3.Cross(arrowPath.EvaluateUpVector(flyTimer / flyTime), Vector3.forward);
            }
            else
            {
                currentDirection = -Vector3.Cross(arrowPath.EvaluateUpVector(flyTimer / flyTime), Vector3.forward);
            }

            this.transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, currentDirection));
            this.transform.position = arrowPath.EvaluatePosition(flyTimer / flyTime);

            if (flyTimer >= flyTime)
            {
                isFlying = false;
                RecycleArrow();
                //���������ˣ��Ե�������˺�
                if(enemy != null)
                {
                    enemy.OnHurt(damage, EDamageType.Physics);
                }

                string soundName = Random.Range(0, 2) == 0 ? AudioConst.SOUND_ARROW_HIT1 : AudioConst.SOUND_ARROW_HIT2;
                Module.LoadController<AudioController>().PlaySound(soundName);
            }
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
