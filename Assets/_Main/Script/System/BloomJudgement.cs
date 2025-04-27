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



    /// <summary>
    /// パーセントを増やす関数
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
    }



    /// <summary>
    /// 敵のパーセントを減らしながら自身のパーセントを増やす関数
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
    }
}
