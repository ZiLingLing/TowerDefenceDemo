using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XFGameFramework;

namespace TowerDefence
{
    public class DialogPanel : Panel
    {
        [SerializeField]
        private Text mainContent;

        private Action onConfirmClick;
        private Action onCancelClick;

        public void OnBtnConfirmClick()
        {
            onConfirmClick?.Invoke();
            Close();
        }

        public void OnBtnCanelClick()
        {
            onCancelClick?.Invoke();
            Close();
        }

        public override void OnLoaded(params object[] param)
        {
            base.OnLoaded(param);
            if (param == null || param.Length == 0) throw new Exception("≤Œ ˝“Ï≥£");

            DialogPanelParam dialogPanelParam = param[0] as DialogPanelParam;
            mainContent.text = dialogPanelParam.message;
            onConfirmClick = dialogPanelParam.onConfirmClick;
            onCancelClick = dialogPanelParam.onCancelClick;
        }
    }

    public class DialogPanelParam
    {
        public string message;
        public Action onConfirmClick;
        public Action onCancelClick;
    }
}

