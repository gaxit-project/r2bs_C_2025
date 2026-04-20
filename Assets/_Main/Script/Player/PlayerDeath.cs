using UnityEngine;
using System.Collections;

/// <summary>
/// プレイヤーの死亡とリスポーンを管理するクラス
/// </summary>
public class PlayerDeath : MonoBehaviour
{
    // プレイヤーの状態を管理する (0: 生存, 1: 死亡)
    public int playerStatus = 0;

    private void Start()
    {
        // 初期化処理が必要ならここに記述
    }

    private void Update()
    {
        // テスト用：キー押下でリスポーン処理を試す
        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }
    }

    /// <summary>
    /// プレイヤーのリスポーンを開始する
    /// </summary>
    public void Respawn()
    {
        StartCoroutine(StartRespawnRoutine());
    }

    /// <summary>
    /// リスポーン処理を行うコルーチン
    /// </summary>
    private IEnumerator StartRespawnRoutine()
    {
        // 動けなくする（死亡）
        playerStatus = 1;

        // フェードイン処理（仮）
        Debug.Log("Fade In Start");

        // 4秒間待機
        yield return new WaitForSeconds(4f);

        // フェードアウト処理（仮）
        Debug.Log("Fade Out Start");

        // リスポーン処理（仮）
        transform.position =new Vector3(10.5f,0,1.5f);

        // 動けるようにする（生存）
        playerStatus = 0;
    }
}
