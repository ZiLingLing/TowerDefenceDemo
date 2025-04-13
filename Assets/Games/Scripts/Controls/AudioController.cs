using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFABManager;
using XFGameFramework;

namespace TowerDefence
{
    public class AudioController : Controller
    {
        public override void OnInit()
        {
            base.OnInit();

            AudioPlayer.RegisterAudioController(EAudioType.BGM.ToString(), true);
            AudioPlayer.RegisterAudioController(EAudioType.Sound.ToString(), true);

            AudioPlayer.GetAuidoController(EAudioType.BGM.ToString()).Volume = 1;
            AudioPlayer.GetAuidoController(EAudioType.Sound.ToString()).Volume = 1;
        }

        public void PlayBGM(string assetName)
        {
            AudioSource audioSource = AudioPlayer.GetAuidoController(EAudioType.BGM.ToString()).AudioSource;
            if (audioSource.clip != null && audioSource.clip.name == assetName) return;

            AudioClip audioClip = AssetBundleManager.LoadAsset<AudioClip>(Module.ProjectName, assetName);
            if (audioClip == null) return;
            AudioPlayer.Play(EAudioType.BGM.ToString(), audioClip, false);
        }

        public void PlaySound(string assetName)
        {
            AudioClip audioClip = AssetBundleManager.LoadAsset<AudioClip>(Module.ProjectName, assetName);
            AudioPlayer.Play(EAudioType.Sound.ToString(), audioClip, true);
        }

        public void SetMute(EAudioType audioType,bool isMute)
        {
            AudioPlayer.GetAuidoController(audioType.ToString()).Mute = isMute;
        }

        public bool GetMute(EAudioType audioType)
        {
            return AudioPlayer.GetAuidoController(audioType.ToString()).Mute;
        }
    }

    public enum EAudioType
    {
        BGM,
        Sound,
    }
}

