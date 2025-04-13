using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using XFGameFramework;

namespace TowerDefence
{
    public class Enemy : View
    {
        [SerializeField]
        private Transform renderParent;
        [SerializeField]
        private Transform hpValue;
        [SerializeField]
        private GameObject hpGameObject;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private SpriteRenderer enemyRenderer;
        [SerializeField]
        private SpriteRenderer hpRenedererBg;
        [SerializeField]
        private SpriteRenderer hpRenedererFg;

        private int hp;
        private bool isRunning = false;
        private float runToTargetTime = 0;
        private float runTimer;

        private Vector3 renderParentLocalScale;
        private Collider2D colliderCom;

        public EnemyInfo EnemyInfo { get; private set; }
        public SplineContainer Path { get; private set; }

        public int Hp
        {
            get { return hp; }
            set
            {
                hp = value;
                if (hp < 0) hp = 0;
                UpdateHp();
                if (hp <= 0) OnDie();
            }
        }

        public override void OnLoaded(params object[] param)
        {
            base.OnLoaded(param);
            if(param == null || param.Length == 0)
            {
                throw new System.Exception("����:����Ϊ��");
            }

            int enemyId = (int)param[0];
            EnemyInfo = Module.LoadController<EnemyController>().GetEnemyInfo(enemyId);
            if(EnemyInfo == null)
            {
                throw new System.Exception(string.Format("������Ϣ��ȡʧ��:id{0}",enemyId));
            }

            Path = param[1] as SplineContainer;
            if(Path == null) {
                throw new System.Exception("����·��Ϊ��");
            }

            //�ѵ��˷��õ�·�����
            float3 firstKnotPos = Path.Spline.Knots.First().Position;//��ȡ�ֲ�����
            this.transform.position = new Vector3(firstKnotPos.x, firstKnotPos.y, firstKnotPos.z) + Path.transform.position;
            //����Ѫ��
            Hp = EnemyInfo.hp;
            //����״̬
            isRunning = true;
            //����ӿ�ʼ���е����������ʱ��
            runTimer = 0;
            runToTargetTime = Path.CalculateLength() / EnemyInfo.speed;

            hpGameObject.SetActive(true);
            animator.SetBool("isDie", false);
            colliderCom.enabled = true;
        }

        //����View
        public void UpdateHp()
        {
            hpValue.transform.localScale = new Vector3((float)Hp / EnemyInfo.hp, 1, 1);
        }

        public void OnHurt(int damage, EDamageType damageType)
        {
            if (Hp <= 0) return;
            switch (damageType)
            {
                case EDamageType.Physics:
                    damage -= EnemyInfo.physical_defense;
                    break;
                case EDamageType.Magic:
                    damage -= EnemyInfo.magical_defense;
                    break;
            }
            if (damage < 0)
            {
                damage = 0;
            }
            Hp -= damage;
        }

        public void OnDie()
        {
            Module.LoadController<FightController>().IncreaseCoin(EnemyInfo.drop_gold);

            hpGameObject.SetActive(false);
            animator.SetBool("isDie", true);

            isRunning = false;
            runTimer = 0;
            colliderCom.enabled = false;

            Module.LoadController<AudioController>().PlaySound(AudioConst.SOUND_ENEMY_COMMON_DEAD);
        }

        public void Run()
        {
            if (isRunning == false) return;
            if(runTimer < runToTargetTime)
            {
                //�����ƶ���
                runTimer += Time.deltaTime;
                Vector3 targetPosition = Path.EvaluatePosition(runTimer / runToTargetTime);//�õ�Ŀ��λ��
                UpdateFaceDirection(targetPosition);
                this.transform.position = targetPosition;
            }
            else
            {
                //�Ѿ������յ�
                isRunning = false;
                runTimer = 0;
                //���������˺����ж���Ϸ�Ƿ����
                Module.LoadController<FightController>().PlayerHurt();
                //���յ���
                Close();
            }
        }

        private void UpdateFaceDirection(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - this.transform.position;
            if(direction.x > 0)
            {
                renderParent.transform.localScale = new Vector3(Mathf.Abs(renderParentLocalScale.x), renderParentLocalScale.y, renderParentLocalScale.z);
            }
            else if(direction.x < 0)
            {
                renderParent.transform.localScale = new Vector3(-Mathf.Abs(renderParentLocalScale.x), renderParentLocalScale.y, renderParentLocalScale.z);
            }
        }

        public void SetEnemySortingOrder(int order)
        {
            enemyRenderer.sortingOrder = order;
            hpRenedererBg.sortingOrder = order + 1000;
            hpRenedererFg.sortingOrder = order + 1001;
        }

        private void Awake()
        {
            hpGameObject = this.transform.Find("Hp").gameObject;
            renderParent = this.transform.Find("RenderParent").transform;
            hpValue = this.transform.Find("Hp/Value").transform;
            animator = this.transform.Find("RenderParent/Render").GetComponent<Animator>();
            enemyRenderer = this.transform.Find("RenderParent/Render").GetComponent<SpriteRenderer>();
            hpRenedererBg = this.transform.Find("Hp/HpBg").GetComponent<SpriteRenderer>();
            hpRenedererFg = this.transform.Find("Hp/Value/HpFg").GetComponent<SpriteRenderer>();

            colliderCom = this.transform.GetComponent<Collider2D>();

            renderParentLocalScale = renderParent.transform.localScale;
        }

        private void Update()
        {
            Run();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            isRunning = false;
            runTimer = 0;
        }
    }

    public enum EDamageType
    {
        Physics,
        Magic,
    }
}

