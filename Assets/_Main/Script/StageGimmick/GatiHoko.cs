using Unity.VisualScripting;
using UnityEngine;

public class GatiHoko : MonoBehaviour
{
    // 蒼が当たったら-10，赤が当たったら+10で先に-100か+100溜まった人の勝ち

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
                    // チーム1に能力付与
                    CheckMode(teamName);
                    Destroy(this.gameObject);
                }
                break;
            case Team.TeamTwo:
                _gatiHokoGauge -= 10;
                if (_gatiHokoGauge <= -GATIHOKO_MAX)
                {
                    // チーム2に能力付与
                    CheckMode(teamName);
                    Destroy(this.gameObject);
                }
                break;
        }
    }


    private void CheckMode(Team teamName)
    {
        #region ガチホコモードを追加したときのやつ
        //
        // ゲームモードの切り替えができたらここのコメントアウトを外す
        //
        // 現在のゲームモードを取得
        //switch(// 取得したゲームモード)
        //{
        //    case GameMode.GatiHoko:
        //        HokoExplosion()
        //        break;
        //    case GameMode.GatiArea:
        //        StatusUP();
        //        break;
        //}
        #endregion
        // ここから先はゲームモードがGatiAreaの時の逆転要素を書いていく
        StatusUP(teamName);
    }



    /// <summary>
    /// ステータスアップ用の関数
    /// </summary>
    /// <param name="teamName"></param>
    private void StatusUP(Team teamName)
    {
        PlayerController[] playerScript = Object.FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        // ホコを壊したチームのステータスを向上させる
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
    /// ホコ爆破ですべてを色塗る関数
    /// </summary>
    private void HokoExplosion()
    {

    }
}
