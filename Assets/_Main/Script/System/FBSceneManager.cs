using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// シーン遷移を管理するクラス
/// </summary>
public class FBSceneManager : MonoBehaviour
{
    private bool gameset = false;
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
        SoundManager.StopBgm();
        SceneManager.LoadScene("TitleScene");
    }

    /// <summary>
    /// リザルトシーンに遷移する
    /// </summary>
    public void LoadResultScene()
    {
        Time.timeScale = 1f;
        GameTimer.instance.StopTimer();
        SoundManager.StopBgm();
        if(gameset == false)
        {
            SoundManager.PlaySE("gameset");
            StartCoroutine(ToResultScene());
        }
        MapManager.Instance.DontDest();
       
    }

    /// <summary>
    /// メインシーンに遷移する
    /// </summary>
    public void LoadMainScene()
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadSceneAsync());
        NewLog.Instance.ResetSession();
    }

    /// <summary>
    /// チーム選択シーンに遷移する
    /// </summary>
    public void LoadTeamSelectScene()
    {
        Time.timeScale = 1f;
        DataBase.Instance.SetAiMode(false);
        SoundManager.StopBgm();
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

        SoundManager.StopBgm();

        asyncLoad.allowSceneActivation = true;

        Debug.Log("シーン切り替え");
    }
    IEnumerator ToResultScene()
    {
        gameset = true;
        yield return new WaitForSeconds(2f);
        NewLog.Instance.SendLog(EventType.Result.ToString(), this.transform.position,
            "System", "System", "", "",
            0, 0, 0, GatiArea.Instance.GetCurrentAreaState());

        yield return new WaitForSeconds(2f);
        gameset = false;
        SceneManager.LoadScene("ResultScene");
    }
}
