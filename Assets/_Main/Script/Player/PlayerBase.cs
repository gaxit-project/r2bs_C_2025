using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;
using static MapManager;

public class PlayerBase : MonoBehaviour
{
    // プレイヤー関連の変数
    protected const int EXP_SIZE = 1;    //Exp1つあたりの経験値量 
    protected LevelManager _levelManager;    //レベルアップ管理
    protected PlayerStatus _status;  //レベルアップデータ

    [SerializeField]
    protected float PlayerSpeed = 5f; //プレイヤーの速度
    protected Vector2 moveInput = Vector2.zero; //入力格納
    public Vector2 getMoveInput { get { return moveInput; } } //moveInput_get 
    protected Team TeamName;   // チーム名の保存
    protected Vector3 StartPosition;
    protected int teamLocal; //playerのアングル調整

    protected int SpecialBombCnt = 0;
    protected int SpecialBombRange = 0;
    protected float SpecialPlayerSpeed = 1f;

    protected int playerIndex;
    protected int teamOneIndex;
    protected int teamTwoIndex;

    [SerializeField] private Animator blueAnimator;
    [SerializeField] private Animator RedAnimator;

    private Animator animator;

    // プレイヤーの状態を管理する (0: 生存, 1: 死亡)

    public enum PlayerState
    {
        Alive,
        Death
    }
    public PlayerState currentState;
    protected PlayerTeamData playerData;
    // 爆弾関連の変数
    protected GameObject StandardBomb;  // 爆弾を入れる配列
    [SerializeField] protected GameObject BlueBomb;
    [SerializeField] protected GameObject RedBomb;
    protected Transform BombParent;                  // 爆弾の生成先オブジェクト
    [SerializeField]
    protected int BombRange = 5; // ボムの爆発範囲
    protected int BombCnt = 1;   // ボムの所持数 
    protected Color BombColor = Color.black; // 爆弾の色の設定
    [SerializeField]
    protected int BloomBombMax = 5;          // 爆弾の所持数のマックスの設定
    protected List<GameObject> BloomBombPool = new(); // ボムを入れるリスト

    [SerializeField]
    private GameObject purpleChan;
    [SerializeField]
    private GameObject cyanChan;


    protected void Start()
    {
        _status = GetComponent<PlayerStatus>();
        _levelManager = GetComponent<LevelManager>();
        SetStatus();
    }

    protected void Update()
    {
        //プレイヤーの移動
        PlayerMove();
    }

    /// <summary>
    /// レベルの変更処理
    /// </summary>

    public void SetStatus()
    {
        SetStatusInternal();
    }

    protected virtual void SetStatusInternal()
    {
        if (_status == null) return;
        PlayerSpeed = 2.0f + (_status.GetValue(StatusType.Speed) - 1) * 0.5f;
        BombRange = 1 + (_status.GetValue(StatusType.Power) - 1);
        BloomBombMax = 1 + (_status.GetValue(StatusType.BombCount) - 1);
        for (int i = BloomBombPool.Count; i < BloomBombMax; i++)
        {
            addSetBomb();
        }
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
        if ((GameTimer.instance.IsGameStart()))
        {
            if (moveInput == Vector2.zero)
            {
                animator.SetBool("isWalking", false);
            }
            else
            {
                animator.SetBool("isWalking", true);
            }
        }
    }




    //爆弾設置
    public void OnBomb(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (currentState == PlayerState.Alive && GameTimer.instance.IsGameStart())
        {
            BombPlacement(CatchPlayerPos());
        }
    }




    //プレイヤーの退出
    public void OnLeft()
    {
        Destroy(this.gameObject);
    }

    public void OnPose(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        MainGameManager.instance.OnSwithPosw();
    }





    //プレイヤーの移動
    protected void PlayerMove()
    {
        Vector3 moveValue = new Vector3(moveInput.x * PlayerSpeed * SpecialPlayerSpeed * teamLocal, 0f, moveInput.y * PlayerSpeed * SpecialPlayerSpeed * teamLocal * Time.timeScale);
        if (currentState == PlayerState.Alive && GameTimer.instance.IsGameStart())
        {
            //SoundManager.PlaySE("walk");
            this.GetComponent<Rigidbody>().linearVelocity = moveValue;
        }
        else
        {
            this.GetComponent<Rigidbody>().linearVelocity = new Vector3(0, 0, 0);
        }
    }

    //チーム分け
    protected void TeamSplit()
    {
        if (this.gameObject.tag == "TeamOne")
        {
            teamOneIndex = GameObject.FindGameObjectsWithTag("TeamOne").Length - 1;
            StartPosition = MapManager.Instance.GetStartPosition(teamOneIndex);

            this.transform.position = StartPosition;  //リス地
            purpleChan.SetActive(false);
            cyanChan.SetActive(true);//チーム変更
            StandardBomb = BlueBomb;
            animator = blueAnimator;
            BombColor = new Color32(0, 0, 255, 100);
            TeamName = Team.TeamOne;
            teamLocal = 1; //座標の向き修正

        }
        else
        {
            teamTwoIndex = GameObject.FindGameObjectsWithTag("TeamTwo").Length + 1;
            StartPosition = MapManager.Instance.GetStartPosition(teamTwoIndex);

            this.transform.position = StartPosition;  //リス地
            this.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);  //アングル
            purpleChan.SetActive(true);
            cyanChan.SetActive(false);//チーム変更
            StandardBomb = RedBomb;
            animator = RedAnimator;
            BombColor = new Color32(255, 0, 0, 100);
            TeamName = Team.TeamTwo;
            teamLocal = 1; //座標の向き修正
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
        Rigidbody rb = GetComponent<Rigidbody>();
        float duration = 4f;
        float timer = 0f;
        Vector3 flyDirection = (-transform.right + Vector3.up * 1.2f).normalized;
        switch (TeamName)
        {
            case Team.TeamOne:
                flyDirection = (-transform.right + Vector3.up * 1.2f).normalized; // 左上
                break;
            case Team.TeamTwo:
                flyDirection = (transform.right + Vector3.up * 1.2f).normalized; // 右上
                break;
        }
        float forcePower = 100f;
        rb.constraints = RigidbodyConstraints.None;

        while (timer < duration)
        {
            rb.AddForce(flyDirection * forcePower, ForceMode.Force);
            timer += Time.deltaTime;
            yield return null;
        }
        // 4秒間待機
        //yield return new WaitForSeconds(4f);

        // フェードアウト処理（仮）
        Debug.Log("Fade Out Start");

        // リスポーン処理（仮）
        switch (TeamName)
        {
            case Team.TeamOne:
                transform.position = StartPosition;
                break;
            case Team.TeamTwo:
                transform.position = StartPosition;
                break;
        }
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotation;


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
        if (!MapManager.Instance.GetBlockData(blockData.gridPosition.x, blockData.gridPosition.y).isBomb)
        {
            Vector3 position = blockData.tilePosition;

            GameObject obj = GetBomb();
            if (obj == null)
            {
                return;
            }
            SoundManager.PlaySE("bloom");
            // ここでリセット！
            obj.transform.SetParent(BombParent);
            obj.transform.position = position;
            obj.transform.rotation = Quaternion.Euler(-90,0,0);
            obj.SetActive(true); // 再利用だから必ず有効化
            obj.tag = "FlowerBomb";
            BombProcess BP = obj.GetComponent<BombProcess>();
            BP.VarSetting(BombRange + SpecialBombRange, BombColor, blockData, TeamName);
            BP.StartBombCoutDownCoroutine(BombRange + SpecialBombRange, BombColor, blockData, TeamName);
            MapManager.Instance.GetBlockData(blockData.gridPosition.x, blockData.gridPosition.y).isBomb = true;
        }
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

    private void addSetBomb()
    {
        GameObject BloomBomb = Instantiate(StandardBomb, BombParent);
        BloomBomb.SetActive(false);
        BloomBombPool.Add(BloomBomb);
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Exp")
        {
            _levelManager.AddExp(EXP_SIZE);
            Destroy(collision.gameObject);
        }
    }
}
