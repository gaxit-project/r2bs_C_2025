using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �`�[���I���V�[���ɂ�����v���C���[�̑���Ə�ԊǗ��N���X
/// </summary>
public class TeamSelectScenePlayer : MonoBehaviour
{
    /// <summary>�v���C���[�̈ړ����x</summary>
    private float _playerSpeed = 5f;

    /// <summary>�ړ����͒l</summary>
    private Vector2 _moveInput = Vector2.zero;

    /// <summary>���݂̃`�[���i���g�p�ϐ��j</summary>
    private Team _teamName;

    /// <summary>����TeamOne�ɏ������Ă��邩</summary>
    private bool _isTeamOne = false;

    /// <summary>�`�[���ɂ����W�C���p�̌W��</summary>
    private int _teamLocal = 1;

    /// <summary>�v���C���[�̃C���f�b�N�X�i�ۑ��p�j</summary>
    private int _playerIndex;

    /// <summary>�v���C���[�f�[�^�i�[ScriptableObject</summary>
    private PlayerTeamData _playerData;

    /// <summary>
    /// �����������i�C���f�b�N�X�o�^�ƃf�[�^�̓ǂݍ��݁j
    /// </summary>
    private void Start()
    {
        _playerData = Resources.Load<PlayerTeamData>("PlayerData");
        _playerIndex = TeamSelectManager.PlayerSum;
        TeamSelectManager.PlayerSum++;
    }

    /// <summary>
    /// ���t���[���ړ����������s
    /// </summary>
    private void Update()
    {
        MovePlayer();
    }

    /// <summary>
    /// ���̓C�x���g�ňړ��������X�V
    /// </summary>
    /// <param name="context">�ړ�����</param>
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// �v���C���[���ޏo�����ۂ̏���
    /// </summary>
    public void OnLeft()
    {
        Destroy(this.gameObject);
    }

    /// <summary>
    /// �`�[���I�����̏����i�`�[���̐؂�ւ��ƐF�ύX�j
    /// </summary>
    /// <param name="context">���̓R���e�L�X�g</param>
    public void OnTeamSelect(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        if (_isTeamOne)
        {
            _playerData.PlayerTable[_playerIndex].Team = "TeamTwo";
            GetComponent<MeshRenderer>().material.color = Color.red;  // �ԂɕύX
            _isTeamOne = false;
        }
        else
        {
            _playerData.PlayerTable[_playerIndex].Team = "TeamOne";
            GetComponent<MeshRenderer>().material.color = Color.blue; // �ɕύX
            _isTeamOne = true;
        }
    }

    /// <summary>
    /// �v���C���[����͂ɉ����Ĉړ�������
    /// </summary>
    private void MovePlayer()
    {
        var rb = GetComponent<Rigidbody>();
        rb.linearVelocity = new Vector3(_moveInput.x * _playerSpeed * _teamLocal, 0f, _moveInput.y * _playerSpeed * _teamLocal);
    }
}
