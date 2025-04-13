using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TowerDefence
{
    public class TipPanel : MonoBehaviour
    {
        [SerializeField]
        private Text title;
        [SerializeField]
        private Text content;
        [SerializeField]
        private Button confirmBtn;
        [SerializeField]
        private Button cancelBtn;
        [SerializeField]
        private Text confirmBtnText;
        [SerializeField]
        private Text cancelBtnText;

        private void Awake()
        {
            Hide();
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        public void ShowTip(string title, string content, UnityAction onConfirmClick, UnityAction onCancelClick = null,
            bool isShowCancelBtn = true, string btnConfirmText = "确定", string btnCancelText = "取消")
        {
            this.gameObject.SetActive(true);

            this.title.text = title;
            this.content.text = content;
            confirmBtnText.text = btnConfirmText;
            cancelBtnText.text = btnCancelText;

            confirmBtn.onClick.RemoveAllListeners();
            confirmBtn.onClick.AddListener(onConfirmClick);
            cancelBtn.onClick.RemoveAllListeners();
            cancelBtn.onClick.AddListener(onCancelClick);

            cancelBtn.gameObject.SetActive(isShowCancelBtn);
        }
    }
}

