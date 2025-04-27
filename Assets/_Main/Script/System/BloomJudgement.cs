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
    public void AddBloomJudge(string teamName)
    {
        switch(teamName)
        {
            case "TeamOne":
                TeamOneBloomPercent++;
                break;
            case "TeamTwo":
                TeamTwoBloomPercent++;
                break;
        }
        Debug.Log("�h�����Z:  �F" + TeamOneBloomPercent + "�@�@��:" + TeamTwoBloomPercent);
    }



    /// <summary>
    /// �G�̃p�[�Z���g�����炵�Ȃ��玩�g�̃p�[�Z���g�𑝂₷�֐�
    /// </summary>
    /// <param name="teamName"></param>
    public void RemoveBloomJudge(string teamName)
    {
        switch (teamName)
        {
            case "TeamOne":
                TeamTwoBloomPercent--;
                TeamOneBloomPercent++;
                break;
            case "TeamTwo":
                TeamOneBloomPercent--;
                TeamTwoBloomPercent++;
                break;
        }
        Debug.Log("�h������Z:  �F" + TeamOneBloomPercent + "�@�@��:" + TeamTwoBloomPercent);
    }
}
