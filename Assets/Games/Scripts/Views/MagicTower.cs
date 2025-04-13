using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class MagicTower : TowerBase
    {
        private Enemy enemy;
        private int damage;

        [SerializeField]
        private List<Magic> magicList = new List<Magic>();
        [SerializeField]
        private List<SpriteRenderer> spriteRendererList = new List<SpriteRenderer>();

        protected override void OnAttack(Enemy enemy, int damage)
        {
            base.OnAttack(enemy, damage);

            foreach(Magic item in magicList)
            {
                this.enemy = enemy;
                this.damage = damage;

                item.OnAttack(this.enemy, this.damage);
            }

            Module.LoadController<AudioController>().PlaySound(AudioConst.SOUND_LIGHTNING);
        }

        protected override void OnPreparing(float progress)
        {
            base.OnPreparing(progress);
            foreach (SpriteRenderer item in spriteRendererList)
            {
                item.color = new Color(1, 1, 1, progress);
            }
        }

        private void Awake()
        {
            foreach (Magic item in magicList)
            {
                item.gameObject.SetActive(false);
            }
        }
    }
}

