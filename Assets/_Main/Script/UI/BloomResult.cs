using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BloomResult : MonoBehaviour
{
    public TextMeshProUGUI TeamOnePer;
    public TextMeshProUGUI TeamTwoPer;
    public TextMeshProUGUI Winner;

    /// <summary>
    /// Resultデータ（ScriptableObject）
    /// </summary>
    private ResultData _ResultData;
    void Start()
    {
        _ResultData = Resources.Load<ResultData>("ResultData");

#if !UNITY_EDITOR
    ResultDataIO.Load(_ResultData);
#endif

        TeamOnePer.text = _ResultData.TeamOneBloomPercent.ToString();
        TeamTwoPer.text = _ResultData.TeamTwoBloomPercent.ToString();

        if (_ResultData.TeamOneBloomPercent == _ResultData.TeamTwoBloomPercent)
        {
            Winner.text = "Draw";
        }
        else if (_ResultData.TeamOneBloomPercent > _ResultData.TeamTwoBloomPercent)
        {
            Winner.text = "TeamOneWin";
        }
        else
        {
            Winner.text = "TeamTwoWin";
        }
    }
}