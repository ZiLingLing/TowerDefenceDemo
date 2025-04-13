using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence
{
    public class StoneTower : TowerBase
    {
        [SerializeField]
        private Animator animator;

        private Enemy enemy;
        private int damage;

        [SerializeField]
        private Transform stoneParent;

        private Stone stone;

        protected override void OnAttack(Enemy enemy, int damage)
        {
            base.OnAttack(enemy, damage);

            this.damage = damage;
            this.enemy = enemy;

            animator.ResetTrigger("attack");
            animator.SetTrigger("attack");
        }

        protected override void OnReady()
        {
            base.OnReady();
            stone = Module.LoadView<Stone>(stoneParent);
        }

        public void OnAnimationAttack()
        {
            if (stone == null) return;
            if(enemy != null)
            {
                stone.Attack(enemy, damage, stoneParent.position);
            }
            else
            {
                stone.Close();
            }
            stone = null;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if(stone != null)
            {
                stone.Close();
                stone = null;
            }
        }
    }
}

