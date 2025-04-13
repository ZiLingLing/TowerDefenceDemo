using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFGameFramework;

namespace TowerDefence
{
    public class Archer : View
    {
        private Enemy target;
        private int damage;
        private Arrow arrow = null;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private Transform arrowParent;

        public void SetTarget(Enemy target)
        {
            this.target = target;
        }

        private bool IsTargetAbove()
        {
            if (target == null) return false;
            //���������Ŀ�������
            Vector2 direction = target.transform.position - this.transform.position;

            if (Vector2.Angle(direction, Vector2.up) < 45) return true;

            return false;
        }

        private bool IsCanAttack()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Shot"))
            {
                //������ڲ�������Ķ��������ܹ���
                return false;
            }
            return true;
        }

        //�����������˺�ȡ��������
        public void Attack(int damage)
        {
            if (IsCanAttack() == false) return;
            this.damage = damage;
            animator.ResetTrigger("isShot");
            animator.SetTrigger("isShot");
        }

        public void OnAnimationAttack(string name)
        {
            switch (name)
            {
                case "OnPreAttack":
                    //��������
                    OnPreAttack();
                    break;
                case "OnAttack":
                    //���乭��
                    OnAttack();
                    break;
            }
        }

        private void UpdateDirection()
        {
            if (target == null) return;
            Vector2 direction = target.transform.position - this.transform.position;
            //����xΪ�������ұߣ�Ϊ���������
            this.transform.localScale = direction.x > 0 ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
        }

        public void OnPreAttack()
        {
            arrow = Module.LoadView<Arrow>(arrowParent);
        }

        public void OnAttack()
        {
            if (arrow == null) return;
            if(target != null && target.gameObject.activeSelf == true)
            {
                arrow.transform.SetParent(null);
                arrow.transform.localScale = Vector3.one;
                arrow.Attack(target, damage, arrowParent.position);
                arrow = null;
            }
            else
            {
                arrow.Close();
                arrow = null;
            }
        }

        private void Update()
        {
            animator.SetFloat("up", IsTargetAbove() == true ? 1 : 0);
            UpdateDirection();
        }
    }
}

