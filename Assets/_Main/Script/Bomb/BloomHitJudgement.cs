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
        if(other.tag == "TeamOne" && _teamName == Team.TeamTwo)
        {
            GameObject obj = other.gameObject;
            PlayerBase PC = obj.GetComponent<PlayerBase>();
            PC.InitSpecialStatus();
            PC.RespawnPlayer();
            PB.addReward(10);
        }
        else if (other.tag == "TeamTwo" && _teamName == Team.TeamOne)
        {
            GameObject obj = other.gameObject;
            PlayerBase PC = obj.GetComponent<PlayerBase>();
            PC.InitSpecialStatus();
            PC.RespawnPlayer();
            PB.addReward(10);
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
