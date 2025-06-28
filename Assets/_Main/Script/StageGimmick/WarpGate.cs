using System.Collections;
using UnityEngine;
using TMPro;

public class WarpGate : MonoBehaviour
{
    public int groupId;
    public int typeId;
    public Vector3 myGridPosition;

    public Transform parentObj;

    public TextMeshProUGUI[] playerCountdownTexts = new TextMeshProUGUI[4]; // プレイヤーごとのカウントダウンUI

    private void Start()
    {
        GameObject bombParentObj = GameObject.Find("WarpGateGenerate");
        parentObj = bombParentObj.transform;

        // 初期化
        foreach (var text in playerCountdownTexts)
        {
            if (text != null)
                text.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TeamOne") || other.CompareTag("TeamTwo"))
        {
            var player = other.GetComponent<PlayerBase>();
            if (!player.isWarpCoolDown)
            {
                int warpID = (groupId % 2 == 0) ? groupId + 1 : groupId - 1;

                // ワープ先を探す
                for (int i = 0; i < parentObj.childCount; i++)
                {
                    WarpGate gate = parentObj.GetChild(i).GetComponent<WarpGate>();
                    if (gate.groupId == warpID && gate.typeId == this.typeId)
                    {
                        // ワープ処理
                        player.WarpPosition(gate.myGridPosition);
                        player.isWarpCoolDown = true;
                        StartCoroutine(CoolDown(player));

                        // カウントダウン演出
                        StartCoroutine(ShowCountdownOnAllGates(player.playerID));
                        break;
                    }
                }
            }
        }
    }

    IEnumerator CoolDown(PlayerBase player)
    {
        yield return new WaitForSeconds(5f);
        player.isWarpCoolDown = false;
    }

    // ゲートをくぐったプレイヤーのみすべてのゲートに演出を出す
    IEnumerator ShowCountdownOnAllGates(int playerId)
    {
        for (int i = 0; i < parentObj.childCount; i++)
        {
            WarpGate gate = parentObj.GetChild(i).GetComponent<WarpGate>();
            gate.StartCoroutine(gate.ShowCountdown(playerId, 5));
        }
        yield return null;
    }

    public IEnumerator ShowCountdown(int playerId, int seconds)
    {
        var text = playerCountdownTexts[playerId];
        text.enabled = true;

        for (int i = seconds; i > 0; i--)
        {
            text.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        text.enabled = false;
    }
}
