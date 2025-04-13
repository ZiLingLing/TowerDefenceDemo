using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XFGameFramework;

namespace TowerDefence
{
    public class PausePanel : Panel
    {
        [SerializeField]
        private Toggle togBGM;
        [SerializeField]
        private Toggle togSound;

        public override void OnLoaded(params object[] param)
        {
            base.OnLoaded(param);

            togSound.isOn = Module.LoadController<AudioController>().GetMute(EAudioType.Sound);
            togBGM.isOn = Module.LoadController<AudioController>().GetMute(EAudioType.BGM);
        }

        public void OnBtnReplayClick()
        {
            Module.LoadController<FightController>().Replay();
            Close();
        }

        public void OnBtnExitClick()
        {
            Module.LoadController<GameController>().SetGameState(EGameState.SelectLevel);
        }

        public void OnTogSoundValueChange(bool isOn)
        {
            Module.LoadController<AudioController>().SetMute(EAudioType.Sound, isOn);
        }

        public void OnTogBGMValueChange(bool isOn)
        {
            Module.LoadController<AudioController>().SetMute(EAudioType.BGM, isOn);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Time.timeScale = 0;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Time.timeScale = 1;
        }
    }
}

