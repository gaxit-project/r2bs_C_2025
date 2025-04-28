using UnityEngine;

public class BloomJudgement : MonoBehaviour
{
    private int TeamOneBloomPercent = 0;  // ��ڂ̃`�[���̍炫�ւ�p�[�Z���e�[�W
    private int TeamTwoBloomPercent = 0;  // ��ڂ̃`�[���̍炫�ւ�p�[�Z���e�[�W



    public static BloomJudgement Instance;
    private void Awake()
    {
        Instance = this;
        TeamOneBloomPercent = 0;
        TeamTwoBloomPercent = 0;
    }



    /// <summary>
    /// �p�[�Z���g�𑝂₷�֐�
    /// </summary>
    /// <param name="teamName"></param>
    public void AddBloomJudge(Team teamName)
    {
        switch(teamName)
        {
            case Team.TeamOne:
                TeamOneBloomPercent++;
                break;
            case Team.TeamTwo:
                TeamTwoBloomPercent++;
                break;
        }
    }



    /// <summary>
    /// �G�̃p�[�Z���g�����炵�Ȃ��玩�g�̃p�[�Z���g�𑝂₷�֐�
    /// </summary>
    /// <param name="teamName"></param>
    public void RemoveBloomJudge(Team teamName)
    {
        switch (teamName)
        {
            case Team.TeamOne:
                TeamTwoBloomPercent--;
                TeamOneBloomPercent++;
                break;
            case Team.TeamTwo:
                TeamOneBloomPercent--;
                TeamTwoBloomPercent++;
                break;
        }
    }
}
