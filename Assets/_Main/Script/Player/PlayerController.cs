using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : PlayerBase
{
    private void Awake()
    {
        // ボムのPrefabと生成先オブジェクトの取得
        StandardBomb = Resources.Load<GameObject>("Prefab/StandardBomb");
        GameObject bombParentObj = GameObject.Find("BombGenerate");
        BombParent = bombParentObj.transform;

        //チーム分け
        playerData = Resources.Load<PlayerTeamData>("PlayerData");
        playerIndex++;
        this.gameObject.tag = playerData.PlayerTable[playerIndex].Team;
        TeamSplit();

        InitSpecialStatus();
    }

    public void RespawnPlayer()
    {
        Respawn();
    }


    /// <summary>
    /// 自身のチーム名を返す関数
    /// </summary>
    public Team CurrentTeamName => TeamName;



    /// <summary>
    /// 特殊ステータスをアップさせる
    /// </summary>
    public void SpecialStatusUP()
    {
        SpecialBombCnt = 2;
        SpecialBombRange = 2;
        SpecialPlayerSpeed = 1.5f;
    }
    /// <summary>
    /// 特殊ステータスを元に戻す
    /// </summary>
    public void InitSpecialStatus()
    {
        SpecialBombCnt = 0;
        SpecialBombRange = 0;
        SpecialPlayerSpeed = 1f;
    }

}
