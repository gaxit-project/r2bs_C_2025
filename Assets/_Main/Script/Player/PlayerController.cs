using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : PlayerBase
{
    private void Awake()
    {
        // �{����Prefab�Ɛ�����I�u�W�F�N�g�̎擾
        StandardBomb = Resources.Load<GameObject>("Prefab/StandardBomb");
        GameObject bombParentObj = GameObject.Find("BombGenerate");
        BombParent = bombParentObj.transform;

        //�`�[������
        playerData = Resources.Load<PlayerTeamData>("PlayerData");
        playerIndex++;
        this.gameObject.tag = playerData.PlayerTable[playerIndex].Team;
        TeamSplit();

        InitSpecialStatus();
    }

    public void RespawnPlayer()
    {
        Respawn();
    }


    /// <summary>
    /// ���g�̃`�[������Ԃ��֐�
    /// </summary>
    public Team CurrentTeamName => TeamName;



    /// <summary>
    /// ����X�e�[�^�X���A�b�v������
    /// </summary>
    public void SpecialStatusUP()
    {
        SpecialBombCnt = 2;
        SpecialBombRange = 2;
        SpecialPlayerSpeed = 1.5f;
    }
    /// <summary>
    /// ����X�e�[�^�X�����ɖ߂�
    /// </summary>
    public void InitSpecialStatus()
    {
        SpecialBombCnt = 0;
        SpecialBombRange = 0;
        SpecialPlayerSpeed = 1f;
    }

}
