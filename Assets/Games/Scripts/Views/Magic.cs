using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFGameFramework;

namespace TowerDefence
{
    public class Magic : MonoBehaviour
    {
        public void OnAttack(Enemy enemy,int damage)
        {
            this.gameObject.SetActive(true);
            //敌人的中心点默认在脚下，指向身体需要增加y
            Vector3 direction = enemy.transform.position - this.transform.position + Vector3.up * 0.3f;
            this.transform.eulerAngles = new Vector3(0, 0, -Vector2.SignedAngle(direction, Vector2.up));
            this.transform.localScale = new Vector3(1, 1 * Vector3.Distance(enemy.transform.position, this.transform.position), 1);

            enemy.OnHurt(damage, EDamageType.Magic);

            TimerManager.DelayInvoke(() =>
            {
                this.gameObject.SetActive(false);
            }, 0.1f);
        }
    }
}

