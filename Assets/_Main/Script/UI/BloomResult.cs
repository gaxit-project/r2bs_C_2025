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
        // Resources フォルダからResultデータを読み込む
        _ResultData = Resources.Load<ResultData>("ResultData");

        TeamOnePer.text = _ResultData.TeamOneBloomPercent.ToString();
        TeamTwoPer.text = _ResultData.TeamTwoBloomPercent.ToString();
        if(_ResultData.TeamOneBloomPercent > _ResultData.TeamTwoBloomPercent)
        {
            Winner.text = "青チームの勝ち";
        }
        else
        {
            Winner.text = "赤チームの勝ち";
        }
    }
}
