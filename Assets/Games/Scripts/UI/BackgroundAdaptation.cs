using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence
{
    public class BackgroundAdaptation : MonoBehaviour
    {
        private float currentScreenWidth;
        private float currentScreenHeight;

        private float width;
        private float height;

        private Canvas canvas;
        private RectTransform canvasRectTransform;

        private void Awake()
        {
            RectTransform rect = GetComponent<RectTransform>();
            if (rect == null) return;
            width = rect.rect.width;
            height = rect.rect.height;

            canvas = GetComponentInParent<Canvas>();
            canvasRectTransform = canvas.GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            Refresh();
        }

        private void Refresh()
        {
            if (canvasRectTransform.rect.width == 0 && canvasRectTransform.rect.height == 0)
                return;

            currentScreenWidth = canvasRectTransform.rect.width;
            currentScreenHeight = canvasRectTransform.rect.height;

            float scale_x = currentScreenWidth / width;
            float scale_y = currentScreenHeight / height;

            float s = scale_x > scale_y ? scale_x : scale_y;

            if (s < 1) s = 1; // 不考虑缩小的情况
            transform.localScale = new Vector3(s, s, s);
        }
    }
}

