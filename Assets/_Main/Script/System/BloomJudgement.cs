using UnityEngine;

public class BloomJudgement : MonoBehaviour
{
    private int TeamOneBloomPercent = 0;  // 一つ目のチームの咲き誇りパーセンテージ
    private int TeamTwoBloomPercent = 0;  // 一つ目のチームの咲き誇りパーセンテージ



    public static BloomJudgement Instance;
    private void Awake()
    {
        Instance = this;
        TeamOneBloomPercent = 0;
        TeamTwoBloomPercent = 0;
    }

    public int GetTeamOneBloomPer()
    {
        return TeamOneBloomPercent;
    }
    public int GetTeamTwoBloomPer()
    {
        return TeamTwoBloomPercent;
    }



    /// <summary>
    /// パーセントを増やす関数
    /// </summary>
    /// <param name="teamName"></param>
    public void AddBloomJudgement(Team teamName)
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
    /// 敵のパーセントを減らしながら自身のパーセントを増やす関数
    /// </summary>
    /// <param name="teamName"></param>
    public void RemoveBloomJudgement(Team teamName)
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
