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
            LogUtil.Log("开始游戏");
            Module.LoadPanel<OnFilePanel>();
        }

        public void OnBtnSettingClick()
        {
            LogUtil.Log("设置按钮");
            Module.LoadPanel<SettingPanel>();
        }
    }
}

