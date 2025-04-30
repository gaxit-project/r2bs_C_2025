using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private float _timer; //�v���C�x�[�g�^�C�}�[
    public float CurrentTime { get { return _timer; } } //���݂̎��Ԃ�Ԃ�(�ǂݎ���p)

    [SerializeField] private bool _isActiveTime = false; //�^�C�}�[��i�߂邩�ǂ������f����

    private void Update()
    {
        if (_isActiveTime)
        {
            _timer += Time.deltaTime;
        }
    }

    /// <summary>
    /// �^�C�}�[�𓮂���
    /// </summary>
    public void StartTimer()
    {
        _isActiveTime = true;
    }

    /// <summary>
    /// �^�C�}�[���~�߂�
    /// </summary>
    public void StopTimer()
    {
        _isActiveTime = false;
    }

    /// <summary>
    /// �^�C�}�[�����Z�b�g����
    /// </summary>
    public void ResetTimer()
    {
        _timer = 0f;
        StopTimer();
    }
}