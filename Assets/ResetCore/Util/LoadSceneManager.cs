using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace ResetCore.Util
{
    public class LoadSceneManager : Singleton<LoadSceneManager>
    {
        public bool isLoading = false;
        public void LoadScene(string sceneName, System.Action<bool> loadedAct = null, System.Action<float> progressAct = null)
        {
            if (isLoading == false)
            {
                CoroutineTaskManager.Instance.AddTask(
                new CoroutineTaskManager.CoroutineTask("LoadScene" + sceneName, DoLoadScene(sceneName), loadedAct));
                CoroutineTaskManager.Instance.AddTask(
                    new CoroutineTaskManager.CoroutineTask("LoadSceneProgress" + sceneName, DoLoadSceneProgress(progressAct), loadedAct));
            }
            else
            {
                Debug.logger.LogError("加载场景错误", "正在加载其他场景无法加载新场景");
            }
        }

        private AsyncOperation operation;
        IEnumerator DoLoadScene(string sceneName)
        {
            isLoading = true;
            yield return operation = SceneManager.LoadSceneAsync(sceneName);
            isLoading = false;
        }

        IEnumerator DoLoadSceneProgress(System.Action<float> progressAct)
        {
            while (!operation.isDone)
            {
                if (progressAct != null)
                {
                    progressAct(operation.progress);
                }
                yield return null;
            }
        }  
	
	}

}

