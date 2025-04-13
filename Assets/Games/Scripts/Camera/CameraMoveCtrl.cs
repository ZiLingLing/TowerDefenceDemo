using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using XFGameFramework;

namespace TowerDefence
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraMoveCtrl : MonoBehaviour
    {
        private CinemachineVirtualCamera virtualCamera;

        [SerializeField]
        private float zoomMinDistance = 3.5f;
        [SerializeField]
        private float zoomMaxDistance = 6f;
        [SerializeField]
        private CinemachineConfiner cinemachineConfiner;

        private Collider2D polygonCollider2D;
        private Vector3 mousePosition;//PC
#if UNITY_ANDROID || UNITY_IPHONE
        private float touchDistance;//�ֻ���
#endif

        private void Awake()
        {
            virtualCamera = this.GetComponent<CinemachineVirtualCamera>();
        }

        private void Start()
        {
            polygonCollider2D = cinemachineConfiner.m_BoundingShape2D;
        }

        private void Update()
        {
            //��갴�¼�¼���λ��
            if (Input.GetMouseButtonDown(0))
            {
                mousePosition = Input.mousePosition;
            }
            //��갴ס�϶����
            if (Input.GetMouseButton(0))
            {
                Vector3 delta = Input.mousePosition - mousePosition;
#if UNITY_STANDALONE
                this.transform.position -= delta * Time.deltaTime;
#elif UNITY_ANDROID || UNITY_IPHONE
                this.transform.position -= delta * Time.deltaTime * 0.2f;
#endif
                
                mousePosition = Input.mousePosition;
            }
            //�������
#if UNITY_STANDALONE
            virtualCamera.m_Lens.OrthographicSize -= Input.mouseScrollDelta.y * Time.deltaTime *10;
#elif UNITY_ANDROID || UNITY_IPHONE
            if(Input.touchCount == 2)
            {
                float distance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                if(touchDistance == 0)//��¼��ʼ����ʱ��˫ָ��� 
                {
                    touchDistance = distance;
                }
                //����˫ָ���仯����
                float delta = distance - touchDistance;
                virtualCamera.m_Lens.OrthographicSize -= delta * Time.deltaTime * 0.2f;
                touchDistance = distance;
            }
#endif
            virtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(virtualCamera.m_Lens.OrthographicSize, zoomMinDistance, zoomMaxDistance);
        }

        private void LateUpdate()
        {
            //�����������λ�ã���������ʾ�߽�֮���޷����Զ���ƶ�
            Vector3 tempPos = this.transform.position;
            tempPos.x = Mathf.Clamp(tempPos.x, Camera.main.transform.position.x - 1, Camera.main.transform.position.x + 1);
            tempPos.y = Mathf.Clamp(tempPos.y, Camera.main.transform.position.y - 1, Camera.main.transform.position.y + 1);
            this.transform.position = tempPos;
        }
    }
}

