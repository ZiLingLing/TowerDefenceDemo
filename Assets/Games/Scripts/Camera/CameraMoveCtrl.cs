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
        private float touchDistance;//手机端
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
            //鼠标按下记录鼠标位置
            if (Input.GetMouseButtonDown(0))
            {
                mousePosition = Input.mousePosition;
            }
            //鼠标按住拖动相机
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
            //相机缩放
#if UNITY_STANDALONE
            virtualCamera.m_Lens.OrthographicSize -= Input.mouseScrollDelta.y * Time.deltaTime *10;
#elif UNITY_ANDROID || UNITY_IPHONE
            if(Input.touchCount == 2)
            {
                float distance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                if(touchDistance == 0)//记录开始缩放时的双指间距 
                {
                    touchDistance = distance;
                }
                //根据双指间距变化缩放
                float delta = distance - touchDistance;
                virtualCamera.m_Lens.OrthographicSize -= delta * Time.deltaTime * 0.2f;
                touchDistance = distance;
            }
#endif
            virtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(virtualCamera.m_Lens.OrthographicSize, zoomMinDistance, zoomMaxDistance);
        }

        private void LateUpdate()
        {
            //限制虚拟相机位置，在碰到显示边界之后无法向更远处移动
            Vector3 tempPos = this.transform.position;
            tempPos.x = Mathf.Clamp(tempPos.x, Camera.main.transform.position.x - 1, Camera.main.transform.position.x + 1);
            tempPos.y = Mathf.Clamp(tempPos.y, Camera.main.transform.position.y - 1, Camera.main.transform.position.y + 1);
            this.transform.position = tempPos;
        }
    }
}

