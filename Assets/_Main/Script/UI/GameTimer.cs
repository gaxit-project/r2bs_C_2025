using TMPro.EditorUtilities;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private bool _isActiveTime = false; //�^�C�}�[��i�߂邩�ǂ������f����
    [SerializeField] private float _startTime = 300; //��������
    [SerializeField] private float _countDownTime = 3; //�v���C�J�n���̃J�E���g�_�E���p

    private float _timer; //�v���C�x�[�g�^�C�}�[
    public float CurrentTime { get { return _timer; } } //���݂̎��Ԃ�Ԃ�(�ǂݎ���p)

    private float _mapTimer; //�J�E���g�_�E���p�̃^�C�}�[
    public float CountDownTime { get { return _countDownTime; } }

    public bool _isGameStart = false; //�J�E���g�_�E�����I�������v���C�\�ɂ���bool�ϐ� 

    private void Awake()
    {
        _mapTimer = _startTime; //�}�b�v�^�C�}�[�̏����l����
    }
    private void Update()
    {
        _countDownTime -= Time.deltaTime;

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

    /// <summary>
    /// �}�b�v�^�C�}�[�̌����Ă�䗦��Ԃ�
    /// </summary>
    public float MeltTimer()
    {
        if (_isActiveTime) { 
            _mapTimer -= Time.deltaTime;
        }

        return _mapTimer / _startTime;
    }

    /// <summary>
    /// �}�b�v�^�C�}�[�̏�Ԃ��擾
    /// </summary>
    /// <returns></returns>
    public bool IsMapTimer0()
    {
        if(_mapTimer <= 0)
        {
            _mapTimer = 0;
            return true;
        }

        return false;
    }

    /// <summary>
    /// ����\���ǂ���
    /// </summary>
    /// <returns></returns>
    public bool IsGameStart()
    {
        return _isGameStart;
    }

    /// <summary>
    /// �J�E���g�_�E�����I�������true��Ԃ��ăv���C�\�ɂ���
    /// </summary>
    public void MainGameStart()
    {
        _isGameStart = true;
    }
}