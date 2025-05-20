using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;
using static MapManager;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerBase : MonoBehaviour
{
    // プレイヤー関連の変数
    protected float PlayerSpeed = 5f; //プレイヤーの速度
    protected Vector2 moveInput = Vector2.zero; //入力格納
    protected Team TeamName;   // チーム名の保存
    protected Vector3 StartPosition;
    protected int teamLocal; //playerのアングル調整

    protected int SpecialBombCnt = 0;
    protected int SpecialBombRange = 0;
    protected float SpecialPlayerSpeed = 1f;

    protected static int playerIndex = -1;
    protected static int teamOneIndex = -1;
    protected static int teamTwoIndex = 1;

    // プレイヤーの状態を管理する (0: 生存, 1: 死亡)
    public enum PlayerState
    {
        Alive,
        Death
    }
    public PlayerState currentState;
    protected PlayerTeamData playerData;
    // 爆弾関連の変数
    [SerializeField] protected GameObject StandardBomb;  // 爆弾を入れる配列
    protected Transform BombParent;                  // 爆弾の生成先オブジェクト
    protected int BombRange = 5; // ボムの爆発範囲
    protected int BombCnt = 1;   // ボムの所持数 
    protected Color BombColor = Color.black; // 爆弾の色の設定
    protected int BloomBombMax = 5;          // 爆弾の所持数のマックスの設定
    protected List<GameObject> BloomBombPool = new(); // ボムを入れるリスト


    protected void Start()
    {
        InitializePool();
    }

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
        if(currentState == PlayerState.Alive)
        {
            moveInput = context.ReadValue<Vector2>();
        }
    }



    //爆弾設置
    public void OnBomb(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
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
        this.GetComponent<Rigidbody>().linearVelocity = new Vector3(moveInput.x * PlayerSpeed * SpecialPlayerSpeed * teamLocal, 0f, moveInput.y * PlayerSpeed * SpecialPlayerSpeed * teamLocal);
    }

    //チーム分け
    protected void TeamSplit()
    {
        if (this.gameObject.tag == "TeamOne")
        {
            teamOneIndex++;
            StartPosition = MapManager.Instance.GetStartPosition(teamOneIndex);

            this.transform.position = StartPosition;  //リス地
            this.GetComponent<MeshRenderer>().material.color = Color.blue; //色変更
            BombColor = Color.blue;
            TeamName = Team.TeamOne;
            teamLocal = 1; //座標の向き修正

        }
        else
        {
            teamTwoIndex++;
            StartPosition = MapManager.Instance.GetStartPosition(teamTwoIndex);

            this.transform.position = StartPosition;  //リス地
            this.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);  //アングル
            this.GetComponent<MeshRenderer>().material.color = Color.red;  //色変更
            BombColor = Color.red;
            TeamName = Team.TeamTwo;
            teamLocal = -1; //座標の向き修正
        }

    }



    /// <summary>
    /// プレイヤーのリスポーンを開始する
    /// </summary>
    protected void Respawn()
    {
        StartCoroutine(StartRespawnRoutine());
    }

    /// <summary>
    /// リスポーン処理を行うコルーチン
    /// </summary>
    private IEnumerator StartRespawnRoutine()
    {
        // 動けなくする（死亡）
        currentState = PlayerState.Death;

        // フェードイン処理（仮）
        Debug.Log("Fade In Start");

        // 4秒間待機
        yield return new WaitForSeconds(4f);

        // フェードアウト処理（仮）
        Debug.Log("Fade Out Start");

        // リスポーン処理（仮）
        switch(TeamName)
        {
            case Team.TeamOne:
                transform.position = StartPosition;
                break;
            case Team.TeamTwo:
                transform.position = StartPosition;
                break;
        }
        

        // 動けるようにする（生存）
        currentState = PlayerState.Alive;
    }


    #region 爆弾関連
    /// <summary>
    /// 爆弾を設置する関数
    /// </summary>
    /// <param name="blockData"></param>
    protected void BombPlacement(MapBlockData blockData)
    {
        Vector3 position = blockData.tilePosition;

        GameObject obj = GetBomb();
        if (obj == null)
        {
            return;
        }

        // ここでリセット！
        obj.transform.SetParent(BombParent);
        obj.transform.position = position;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true); // 再利用だから必ず有効化
        obj.tag = "FlowerBomb";
        BombProcess BP = obj.GetComponent<BombProcess>();
        BP.VarSetting(BombRange + SpecialBombRange, BombColor, blockData, TeamName);
        BP.StartBombCoutDownCoroutine(BombRange + SpecialBombRange, BombColor, blockData, TeamName);
    }





    /// <summary>
    /// 爆弾の個数を初期設定する
    /// </summary>
    private void InitializePool()
    {
        for (int i = 0; i < BloomBombMax; i++)
        {
            GameObject BloomBomb = Instantiate(StandardBomb, BombParent);
            BloomBomb.SetActive(false);
            BloomBombPool.Add(BloomBomb);
        }
    }


    /// <summary>
    /// 爆弾を取得してくる
    /// </summary>
    /// <returns></returns>
    public GameObject GetBomb()
    {
        foreach (var bomb in BloomBombPool)
        {
            if (!bomb.activeInHierarchy)
            {
                return bomb;
            }
        }

        return null;
    }

    #endregion
}
