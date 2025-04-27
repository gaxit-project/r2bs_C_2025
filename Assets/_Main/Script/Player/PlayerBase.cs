using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;
using static MapManager;

public class PlayerBase : MonoBehaviour
{
    // プレイヤー関連の変数
    protected float playerSpeed = 10f; //プレイヤーの速度
    protected Vector2 moveInput = Vector2.zero; //入力格納
    // 爆弾関連の変数
    [SerializeField] protected GameObject _standardBomb;  // 爆弾を入れる配列
    protected Transform BombParent;                  // 爆弾の生成先オブジェクト
    protected int BombRange = 5; // ボムの爆発範囲
    protected int BombCnt = 1;   // ボムの所持数
    protected Color BombColor = Color.black;
    //プレイヤーを格納する配列
    private GameObject[] players = null;

    protected void Update()
    {
        //プレイヤーの移動
        PlayerMove();
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


    //プレイヤーの移動入力
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log("こんちくわ");
    }



    //爆弾設置
    public void OnBomb()
    {
        BombPlacement(CatchPlayerPos());
    }




    //プレイヤーの退出
    public void OnLeft()
    {
        Destroy(this.gameObject);
    }




    //プレイヤーの移動
    protected void PlayerMove()
    {
        var move = new Vector3(moveInput.x, 0f, moveInput.y) * playerSpeed * Time.deltaTime; //Timeはポーズ画面時止まるよう
        transform.Translate(move);
    }

    //チーム分け
    protected void TeamSplit()
    {
        players = GameObject.FindGameObjectsWithTag("Player"); //playerの配列
        if(players.Length % 2 == 1)
        {
            this.transform.position = new Vector3(10.5f, 0, 1.5f);  //リス地
            this.GetComponent<MeshRenderer>().material.color = Color.blue;  //
            BombColor = Color.blue;
        }
        else
        {
            this.transform.position = new Vector3(10.5f, 0, 23.5f);  //リス地
            this.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);  //アングル
            this.GetComponent<MeshRenderer>().material.color = Color.red;  //色変更
            BombColor = Color.red;
        }
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
        BombProcess BP = obj.GetComponent<BombProcess>();
        BP.StartBombCoutDownCoroutine(BombRange, BombColor, blockData);
    }
    #endregion
}
