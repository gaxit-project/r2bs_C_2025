using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BloomResult : MonoBehaviour
{
    public TextMeshProUGUI TeamOnePer;
    public TextMeshProUGUI TeamTwoPer;
    public TextMeshProUGUI Winner;
    void Start()
    {
        TeamOnePer.text = BloomJudgement.Instance.GetTeamOneBloomPer().ToString();
        TeamTwoPer.text = BloomJudgement.Instance.GetTeamTwoBloomPer().ToString();
        if(BloomJudgement.Instance.GetTeamOneBloomPer() > BloomJudgement.Instance.GetTeamTwoBloomPer())
        {
            Winner.text = "青チームの勝ち";
        }
        else
        {
            Winner.text = "赤チームの勝ち";
        }
    }
}
