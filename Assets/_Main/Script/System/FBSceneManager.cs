using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// シーン遷移を管理するクラス
/// </summary>
public class FBSceneManager : MonoBehaviour
{
    private string stageName = "stage5";
    private bool areaBool = false;
    /// <summary>
    /// シングルトンインスタンス
    /// </summary>
    public static FBSceneManager Instance;

    private void Awake()
    {
        if (Instance == null) 
        { 
            Instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
        SoundManager.StopBgm();
        SceneManager.LoadScene("ResultScene");
    }

    /// <summary>
    /// メインシーンに遷移する
    /// </summary>
    public void LoadMainScene()
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadSceneAsync());
    }

    /// <summary>
    /// チーム選択シーンに遷移する
    /// </summary>
    public void LoadTeamSelectScene()
    {
        Time.timeScale = 1f;
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

    public void OnDebugF1()
    {
        stageName = "stage1";
        Debug.Log("Stage:" + stageName);
    }
    public void OnDebugF2()
    {
        stageName = "stage2";
        Debug.Log("Stage:" + stageName);
    }
    public void OnDebugF3()
    {
        stageName = "stage3";
        Debug.Log("Stage:" + stageName);
    }
    public void OnDebugF4()
    {
        stageName = "stage4";
        Debug.Log("Stage:" + stageName);
    }
    public void OnDebugF5()
    {
        stageName = "stage5";
        Debug.Log("Stage:" + stageName);
    }
    public void OnDebugF6()
    {
        stageName = "stage6";
        Debug.Log("Stage:" + stageName);
    }
    public void OnDebugF7()
    {
        stageName = "stage7";
        Debug.Log("Stage:" + stageName);
    }
    public void OnDebugF8()
    {
        stageName = "stage8";
        Debug.Log("Stage:" + stageName);
    }
    public void OnDebugF9()
    {
        stageName = "stage9";
        Debug.Log("Stage:"+stageName);
    }

    public void OnDebugF12()
    {
        areaBool = !areaBool;
        if (areaBool)
        {
            Debug.Log("Area:On");
        }
        else
        {
            Debug.Log("Area:Off");
        }
    }

    public string GetStageName()
    {
        return stageName;
    }

    public bool GetAreaBool()
    {
        return areaBool;
    }
}
