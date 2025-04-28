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
        TeamSplit();
    }


    public void RespawnPlayer()
    {
        Respawn();
    }
}
