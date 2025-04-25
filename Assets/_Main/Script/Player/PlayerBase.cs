using UnityEngine;
using static MapManager;

public class PlayerBase : MonoBehaviour
{
    // プレイヤー関連の変数
    protected float Speed;   // プレイヤーのスピード
    // 爆弾関連の変数
    [SerializeField] private GameObject _standardBomb;  // 爆弾を入れる配列
    private Transform BombParent;                  // 爆弾の生成先オブジェクト
    protected int BombRange; // ボムの爆発範囲
    protected int BombCnt;   // ボムの所持数

    private void Start()
    {
        // ボムのPrefabと生成先オブジェクトの取得
        _standardBomb = Resources.Load<GameObject>("Prefab/StandardBomb");
        GameObject bombParentObj = GameObject.Find("BombGenerate");
        BombParent = bombParentObj.transform;
    }




    /// <summary>
    /// プレイヤーの現在いるマップタイルの情報を取得
    /// </summary>
    protected MapBlockData CatchPlayerPos()
    {
        MapBlockData blockData = null;  // データ保存用変数
        RaycastHit hit;                 // レイの変数

        // 真下にレイを飛ばす
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out hit, 10.0f))
        {
            // ワールド座標からグリッド座標に変換
            Vector3 hitPosition = hit.point;
            int x = Mathf.FloorToInt(hitPosition.x / 1f);
            int y = Mathf.FloorToInt(hitPosition.z / 1f);
            int reversedX = (MapManager.Instance.Width - 1) - x;


            // データ取得
            blockData = MapManager.Instance.GetBlockData(reversedX, y);
        }

        return blockData;
    }



    #region 爆弾関連
    /// <summary>
    /// 爆弾を設置する関数
    /// </summary>
    /// <param name="blockData"></param>
    protected void BombPlacement(MapBlockData blockData)
    {
        Vector3 position = blockData.tilePosition; // 現在のポジション取得
        GameObject obj = Instantiate(_standardBomb, position, Quaternion.identity, BombParent);
        // ボムを起爆させる外部関数.Instance.関数名(BombRange);
    }
    #endregion
}
