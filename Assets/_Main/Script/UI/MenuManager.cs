using UnityEngine;

/// <summary>
/// �^�C�g����ʂ�UI�p�l������у{�^���̏������Ǘ�����N���X
/// </summary>
public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _titlePanel;

    [SerializeField]
    private GameObject _optionPanel;

    private void Start()
    {
        // ������Ԃł̓^�C�g���p�l����\���A�I�v�V�����p�l�����\���ɂ���
        SetPanelVisibility(isTitleVisible: true, isOptionVisible: false);
    }

    /// <summary>
    /// �X�^�[�g�{�^���������ꂽ���ɌĂ΂��
    /// �`�[���I���V�[���֑J�ڂ���
    /// </summary>
    public void OnStart()
    {
        FBSceneManager.Instance.LoadTeamSelectScene();
    }

    /// <summary>
    /// �I�v�V�����{�^���������ꂽ���ɌĂ΂��
    /// �I�v�V�����p�l����\�����A�^�C�g���p�l�����\���ɂ���
    /// </summary>
    public void OnOption()
    {
        SetPanelVisibility(isTitleVisible: false, isOptionVisible: true);
    }

    /// <summary>
    /// �߂�{�^���������ꂽ���ɌĂ΂��
    /// �^�C�g���p�l����\�����A�I�v�V�����p�l�����\���ɂ���
    /// </summary>
    public void OnBack()
    {
        SetPanelVisibility(isTitleVisible: true, isOptionVisible: false);
    }

    /// <summary>
    /// �I���{�^���������ꂽ���ɌĂ΂��
    /// �G�f�B�^��ł͍Đ����~�A�r���h���ł̓A�v���P�[�V�������I������
    /// </summary>
    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// �p�l���̕\���E��\����؂�ւ��鋤�ʏ���
    /// </summary>
    /// <param name="isTitleVisible">�^�C�g���p�l����\�����邩�ǂ���</param>
    /// <param name="isOptionVisible">�I�v�V�����p�l����\�����邩�ǂ���</param>
    private void SetPanelVisibility(bool isTitleVisible, bool isOptionVisible)
    {
        _titlePanel.SetActive(isTitleVisible);
        _optionPanel.SetActive(isOptionVisible);
    }
}
