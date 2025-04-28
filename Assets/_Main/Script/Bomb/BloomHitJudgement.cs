using System.Collections;
using UnityEngine;

public class BloomHitJudgment : MonoBehaviour
{
    private string _teamName;           // チーム名

    public static BloomHitJudgment Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void StartJudgementCountDownCoroutine(string teamName)
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
        if(other.tag == "TeamOne" && _teamName == "TeamTwo")
        {
            GameObject obj = other.gameObject;
            PlayerController PC = obj.GetComponent<PlayerController>();
            Debug.Log("ヒット");
            PC.RespawnPlayer();
        }
        else if (other.tag == "TeamTwo" && _teamName == "TeamOne")
        {
            GameObject obj = other.gameObject;
            PlayerController PC = obj.GetComponent<PlayerController>();
            Debug.Log("ヒット");
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
