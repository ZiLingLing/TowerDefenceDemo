using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence
{

    public class ArcherTower : TowerBase
    {
        [SerializeField]
        private List<Archer> archerList;

        protected override void OnAttack(Enemy enemy, int damage)
        {
            base.OnAttack(enemy, damage);
            foreach(var archer in archerList)
            {
                archer.SetTarget(enemy);
                archer.Attack(damage);
            }
        }

        protected override void OnAttackEnd()
        {
            base.OnAttackEnd();
            foreach (var archer in archerList)
            {
                archer.SetTarget(null);
            }
        }
    }
}

