using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XFGameFramework;

namespace TowerDefence
{
    public class ToastPanel : Panel
    {
        [SerializeField]
        private Text msgText;

        public override void OnLoaded(params object[] param)
        {
            base.OnLoaded(param);
            if(param == null || param.Length == 0)
            {
                throw new System.Exception("提示面板传入参数为空或者传参数量为0");
            }

            msgText.text = param[0].ToString();
            float time = (float)param[1];
            Invoke("Close", time);
        }
    }
}

