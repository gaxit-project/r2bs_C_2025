using System.Collections;
using UnityEngine;

public class BloomHitJudgment : MonoBehaviour
{
    private string _teamName;           // �`�[�����̕ϐ�������

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
            Debug.Log("�_���[�W���󂯂�");
        }
        else if (other.tag == "TeamTwo" && _teamName == "TeamOne")
        {
            Debug.Log("�_���[�W���󂯂�");
        }
    }

}
