using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public string nextSceneName;

    public void LoadNextScene()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            Debug.Log("読み込み中... " + (asyncLoad.progress * 100) + "%");
            yield return null;
        }

        Debug.Log("読み込み完了（待機中）");

        yield return new WaitForSeconds(1f);

        asyncLoad.allowSceneActivation = true;

        Debug.Log("シーン切り替え");
    }
}
