using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XFGameFramework;

namespace TowerDefence
{
    public class SettingPanel : Panel
    {
        [SerializeField]
        private Toggle togSound;
        [SerializeField]
        private Toggle togBGM;

        public override void OnLoaded(params object[] param)
        {
            base.OnLoaded(param);

            togSound.isOn = !Module.LoadController<AudioController>().GetMute(EAudioType.Sound);
            togBGM.isOn = !Module.LoadController<AudioController>().GetMute(EAudioType.BGM);
        }

        public void OnTogSoundValueChange(bool isOn)
        {
            Module.LoadController<AudioController>().SetMute(EAudioType.Sound, !isOn);
        }

        public void OnTogBGMValueChange(bool isOn)
        {
            Module.LoadController<AudioController>().SetMute(EAudioType.BGM, !isOn);
        }
    }
}

