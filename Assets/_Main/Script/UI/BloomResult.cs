using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BloomResult : MonoBehaviour
{
    public TextMeshProUGUI TeamOnePer;
    public TextMeshProUGUI TeamTwoPer;
    public TextMeshProUGUI Winner;

    /// <summary>
    /// Result�f�[�^�iScriptableObject�j
    /// </summary>
    private ResultData _ResultData;
    void Start()
    {
        // Resources �t�H���_����Result�f�[�^��ǂݍ���
        _ResultData = Resources.Load<ResultData>("ResultData");

        TeamOnePer.text = _ResultData.TeamOneBloomPercent.ToString();
        TeamTwoPer.text = _ResultData.TeamTwoBloomPercent.ToString();
        if(_ResultData.TeamOneBloomPercent > _ResultData.TeamTwoBloomPercent)
        {
            Winner.text = "�`�[���̏���";
        }
        else
        {
            Winner.text = "�ԃ`�[���̏���";
        }
    }
}
