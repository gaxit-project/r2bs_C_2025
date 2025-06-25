using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// シーン遷移を管理するクラス
/// </summary>
public class FBSceneManager : MonoBehaviour
{
    /// <summary>
    /// シングルトンインスタンス
    /// </summary>
    public static FBSceneManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// タイトルシーンに遷移する
    /// </summary>
    public void LoadTitleScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene");
    }

    /// <summary>
    /// リザルトシーンに遷移する
    /// </summary>
    public void LoadResultScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("ResultScene");
    }

    /// <summary>
    /// メインシーンに遷移する
    /// </summary>
    public void LoadMainScene()
    {
        StartCoroutine(LoadSceneAsync());
    }

    /// <summary>
    /// チーム選択シーンに遷移する
    /// </summary>
    public void LoadTeamSelectScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TeamSelectScene");
    }
    IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainScene");
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
