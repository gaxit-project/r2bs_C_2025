using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �V�[���J�ڂ��Ǘ�����N���X
/// </summary>
public class FBSceneManager : MonoBehaviour
{
    /// <summary>
    /// �V���O���g���C���X�^���X
    /// </summary>
    public static FBSceneManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// �^�C�g���V�[���ɑJ�ڂ���
    /// </summary>
    public void LoadTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }

    /// <summary>
    /// ���U���g�V�[���ɑJ�ڂ���
    /// </summary>
    public void LoadResultScene()
    {
        SceneManager.LoadScene("ResultScene");
    }

    /// <summary>
    /// ���C���V�[���ɑJ�ڂ���
    /// </summary>
    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    /// <summary>
    /// �`�[���I���V�[���ɑJ�ڂ���
    /// </summary>
    public void LoadTeamSelectScene()
    {
        SceneManager.LoadScene("TeamSelectScene");
    }
}
