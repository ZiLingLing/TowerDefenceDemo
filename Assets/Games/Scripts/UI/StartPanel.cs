using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFGameFramework;

namespace TowerDefence
{
    public class StartPanel : Panel
    {
        public void OnBtnStartClick()
        {
            LogUtil.Log("��ʼ��Ϸ");
            Module.LoadPanel<OnFilePanel>();
        }

        public void OnBtnSettingClick()
        {
            LogUtil.Log("���ð�ť");
            Module.LoadPanel<SettingPanel>();
        }
    }
}

