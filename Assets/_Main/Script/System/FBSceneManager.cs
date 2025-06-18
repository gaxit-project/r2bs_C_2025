using UnityEngine;
using UnityEngine.SceneManagement;

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
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
    }

    /// <summary>
    /// チーム選択シーンに遷移する
    /// </summary>
    public void LoadTeamSelectScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TeamSelectScene");
    }
}
