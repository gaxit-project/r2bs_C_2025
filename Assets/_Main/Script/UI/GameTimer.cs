using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private float _myTimer; //�v���C�x�[�g�^�C�}�[
    public float CurrentTime { get { return _myTimer; } } //���݂̎��Ԃ�Ԃ�(�ǂݎ���p)

    public bool _activeTime = false; //�^�C�}�[��i�߂邩�ǂ������f����

    private void Update()
    {
        if (_activeTime)
        {
            _myTimer += Time.deltaTime;
        }
    }

    /// <summary>
    /// �^�C�}�[�𓮂���
    /// </summary>
    public void StartTimer()
    {
        _activeTime = true;
    }

    /// <summary>
    /// �^�C�}�[���~�߂�
    /// </summary>
    public void StopTimer()
    {
        _activeTime = false;
    }

    /// <summary>
    /// �^�C�}�[�����Z�b�g����
    /// </summary>
    public void ResetTimer()
    {
        _myTimer = 0f;
        StopTimer();
    }
}