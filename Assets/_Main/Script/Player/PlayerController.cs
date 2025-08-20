using System.Collections;
#if UNITY_EDITOR
using UnityEditor.SearchService;
#endif
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
        playerIndex = this.GetComponent<PlayerInput>().user.index;
        this.gameObject.tag = playerData.PlayerTable[playerIndex].Team;
        TeamSplit();

        InitSpecialStatus();

        Camera playerCam = GetComponentInChildren<Camera>();
        playerCam.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "ignore RayCast", "Water", "UI", "TeamOneTile", "TeamTwoTile", "Player", "Back","Exp" ,"Tile" ,"Wall","Player" + (playerIndex + 1) +"UI");
    }  

}
