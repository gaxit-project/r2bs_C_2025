using Unity.VisualScripting;
using UnityEngine;

public class GatiHoko : MonoBehaviour
{
    // ��������������-10�C�Ԃ�����������+10�Ő��-100��+100���܂����l�̏���

    private int _gatiHokoGauge = 0;
    private const int GATIHOKO_MAX = 100;

    public static GatiHoko Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void AddHokoValue(Team teamName)
    {
        switch(teamName)
        {
            case Team.TeamOne:
                _gatiHokoGauge += 10;
                if(_gatiHokoGauge >= GATIHOKO_MAX)
                {
                    // �`�[��1�ɔ\�͕t�^
                    CheckMode(teamName);
                    Destroy(this.gameObject);
                }
                break;
            case Team.TeamTwo:
                _gatiHokoGauge -= 10;
                if (_gatiHokoGauge <= -GATIHOKO_MAX)
                {
                    // �`�[��2�ɔ\�͕t�^
                    CheckMode(teamName);
                    Destroy(this.gameObject);
                }
                break;
        }
    }


    private void CheckMode(Team teamName)
    {
        #region �K�`�z�R���[�h��ǉ������Ƃ��̂��
        //
        // �Q�[�����[�h�̐؂�ւ����ł����炱���̃R�����g�A�E�g���O��
        //
        // ���݂̃Q�[�����[�h���擾
        //switch(// �擾�����Q�[�����[�h)
        //{
        //    case GameMode.GatiHoko:
        //        HokoExplosion()
        //        break;
        //    case GameMode.GatiArea:
        //        StatusUP();
        //        break;
        //}
        #endregion
        // ���������̓Q�[�����[�h��GatiArea�̎��̋t�]�v�f�������Ă���
        StatusUP(teamName);
    }



    /// <summary>
    /// �X�e�[�^�X�A�b�v�p�̊֐�
    /// </summary>
    /// <param name="teamName"></param>
    private void StatusUP(Team teamName)
    {
        PlayerController[] playerScript = Object.FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        // �z�R���󂵂��`�[���̃X�e�[�^�X�����コ����
        for(int i = 0; i < playerScript.Length; i++)
        {
            Team currentTeamName = playerScript[i].CurrentTeamName;
            if(teamName == currentTeamName)
            {
                playerScript[i].SpecialStatusUP();
            }
        }
        
    }




    /// <summary>
    /// �z�R���j�ł��ׂĂ�F�h��֐�
    /// </summary>
    private void HokoExplosion()
    {

    }
}
