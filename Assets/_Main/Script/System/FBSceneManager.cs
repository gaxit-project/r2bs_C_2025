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
        SceneManager.LoadScene("TitleScene");
    }

    /// <summary>
    /// リザルトシーンに遷移する
    /// </summary>
    public void LoadResultScene()
    {
        SceneManager.LoadScene("ResultScene");
    }

    /// <summary>
    /// メインシーンに遷移する
    /// </summary>
    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    /// <summary>
    /// チーム選択シーンに遷移する
    /// </summary>
    public void LoadTeamSelectScene()
    {
        SceneManager.LoadScene("TeamSelectScene");
    }
}
