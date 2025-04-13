using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XFABManager;
using XFGameFramework;

namespace TowerDefence
{
    public class SceneController : Controller
    {
        public void LoadScene(string sceneName, Action callback, Action<float> progress = null)
        {
            if (SceneManager.GetSceneByName(sceneName).IsValid())
            {
                //progress?.Invoke(1);
                callback?.Invoke();
                return;
            }
            CoroutineStarter.Start(LoadSceneExecute(sceneName, callback, progress));
        }

        private IEnumerator LoadSceneExecute(string sceneName, Action callback, Action<float> progress = null)
        {
            AsyncOperation asyncOperation = AssetBundleManager.LoadSceneAsync(Module.ProjectName, sceneName, LoadSceneMode.Single);
            while (asyncOperation.isDone == false)
            {
                yield return null;
                progress?.Invoke(asyncOperation.progress);
            }
            callback?.Invoke();
        }
    }
}

