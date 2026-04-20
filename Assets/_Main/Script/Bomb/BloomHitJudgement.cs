using System.Collections;
using UnityEngine;
using static BloomJudgement;

public class BloomHitJudgment : MonoBehaviour
{
    private Team _teamName;           // チーム名
    private PlayerBase PB;

    public static BloomHitJudgment Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void StartJudgementCountDownCoroutine(Team teamName, PlayerBase _PB, int x, int y)
    {
        _teamName = teamName;
        PB = _PB;
        StartCoroutine(StartJudgementCoutDown(x, y));
    }

    IEnumerator StartJudgementCoutDown(int x, int y)
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(this.gameObject);
        MapManager.Instance.GetBlockData(x, y).isHitJudge = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TeamOne" && _teamName == Team.TeamTwo && !other.gameObject.GetComponent<PlayerBase>().isInvincibility)
        {
            // 相手がすでに死亡状態なら無視する
            GameObject obj = other.gameObject;
            PlayerBase PC = obj.GetComponent<PlayerBase>();
            if (PC.currentState == PlayerBase.PlayerState.Death) return;


            int killID = NewLog.Instance.GetKillEventID();
            PC.InitSpecialStatus();
            PC.RespawnPlayer(killID);
            PB.addReward(DataBase.Instance.attackEnemy);
            // スプレットシートへログ送信
            NewLog.Instance.SendLog(EventType.Kill.ToString(), this.transform.position,
                PB.GetTeamName().ToString(), PB.CharacterType.ToString() + PB.playerID.ToString(), killID.ToString(), "",
                PB.GetBombCntLevel(), PB.GetBombRangeLevel(), PB.GetSpeedLevel(), GatiArea.Instance.GetCurrentAreaState());
        }
        else if (other.tag == "TeamTwo" && _teamName == Team.TeamOne&& !other.gameObject.GetComponent<PlayerBase>().isInvincibility)
        {
            // 相手がすでに死亡状態なら無視する
            GameObject obj = other.gameObject;
            PlayerBase PC = obj.GetComponent<PlayerBase>();
            if (PC.currentState == PlayerBase.PlayerState.Death) return;

            int killID = NewLog.Instance.GetKillEventID();
            PC.InitSpecialStatus();
            PC.RespawnPlayer(killID);
            PB.addReward(DataBase.Instance.attackEnemy);
            // スプレットシートへログ送信
            NewLog.Instance.SendLog(EventType.Kill.ToString(), this.transform.position,
                PB.GetTeamName().ToString(), PB.CharacterType.ToString() + PB.playerID.ToString(), killID.ToString(), "",
                PB.GetBombCntLevel(), PB.GetBombRangeLevel(), PB.GetSpeedLevel(), GatiArea.Instance.GetCurrentAreaState());
        }
        if (other.tag == "FlowerBomb")
        {
            GameObject obj = other.gameObject;
            BombProcess BP = obj.GetComponent<BombProcess>();
            // 連鎖関数の呼び出し
            BP.ChainBloom();
        }
        if (other.tag == "GatiHoko")
        {
            GatiHoko.Instance.AddHokoValue(_teamName);
        }
    }

}
