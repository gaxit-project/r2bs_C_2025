using UnityEngine;
using System.Collections;

/// <summary>
/// �v���C���[�̎��S�ƃ��X�|�[�����Ǘ�����N���X
/// </summary>
public class PlayerDeath : MonoBehaviour
{
    // �v���C���[�̏�Ԃ��Ǘ����� (0: ����, 1: ���S)
    public int playerStatus = 0;

    private void Start()
    {
        // �������������K�v�Ȃ炱���ɋL�q
    }

    private void Update()
    {
        // �e�X�g�p�F�L�[�����Ń��X�|�[������������
        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }
    }

    /// <summary>
    /// �v���C���[�̃��X�|�[�����J�n����
    /// </summary>
    public void Respawn()
    {
        StartCoroutine(StartRespawnRoutine());
    }

    /// <summary>
    /// ���X�|�[���������s���R���[�`��
    /// </summary>
    private IEnumerator StartRespawnRoutine()
    {
        // �����Ȃ�����i���S�j
        playerStatus = 1;

        // �t�F�[�h�C�������i���j
        Debug.Log("Fade In Start");

        // 4�b�ԑҋ@
        yield return new WaitForSeconds(4f);

        // �t�F�[�h�A�E�g�����i���j
        Debug.Log("Fade Out Start");

        // ���X�|�[�������i���j
        transform.position =new Vector3(10.5f,0,1.5f);

        // ������悤�ɂ���i�����j
        playerStatus = 0;
    }
}
