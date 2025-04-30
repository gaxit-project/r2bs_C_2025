using System.Collections;
using UnityEngine;
using static BloomJudgement;

public class BloomHitJudgment : MonoBehaviour
{
    private Team _teamName;           // チーム名

    public static BloomHitJudgment Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void StartJudgementCountDownCoroutine(Team teamName)
    {
        _teamName = teamName;
        StartCoroutine(StartJudgementCoutDown());
    }

    IEnumerator StartJudgementCoutDown()
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(this.gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "TeamOne" && _teamName == Team.TeamTwo)
        {
            GameObject obj = other.gameObject;
            PlayerController PC = obj.GetComponent<PlayerController>();
            PC.RespawnPlayer();
        }
        else if (other.tag == "TeamTwo" && _teamName == Team.TeamOne)
        {
            GameObject obj = other.gameObject;
            PlayerController PC = obj.GetComponent<PlayerController>();
            PC.RespawnPlayer();
        }
        if (other.tag == "FlowerBomb")
        {
            GameObject obj = other.gameObject;
            BombProcess BP = obj.GetComponent<BombProcess>();
            // 連鎖関数の呼び出し
            BP.ChainBloom();
        }
    }

}
