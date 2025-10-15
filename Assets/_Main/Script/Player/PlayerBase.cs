using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;
using static MapManager;
using static UnityEngine.GraphicsBuffer;

public class PlayerBase : MonoBehaviour
{
    // プレイヤー関連の変数
    protected const int EXP_SIZE = 1;    //Exp1つあたりの経験値量 
    //protected LevelManager _levelManager;    //レベルアップ管理 86
    //protected PlayerStatus _status;  //レベルアップデータ 85 126

    [SerializeField]
    protected float PlayerSpeed = 7f; //プレイヤーの速度
    protected Vector2 moveInput = Vector2.zero; //入力格納
    public Vector2 getMoveInput { get { return moveInput; } } //moveInput_get 
    protected Team TeamName;   // チーム名の保存
    protected Vector3 StartPosition;
    protected int teamLocal; //playerのアングル調整

    protected int SpecialBombCnt = 0;
    protected int SpecialBombRange = 0;
    protected float SpecialPlayerSpeed = 1f;


    public int playerID;
    protected int playerIndex;
    protected int teamOneIndex;
    protected int teamTwoIndex;

    [SerializeField] private Animator blueAnimator;
    [SerializeField] private Animator RedAnimator;

    protected Animator animator;

    public bool isWarpCoolDown;

    protected float oneReward;

    //protected bool isFirst = true;

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

    [SerializeField]
    protected int speedLevel = 1;
    protected int bombRangeLevel = 1; // ボムの爆発範囲
    protected int bombCntLevel = 1;   // ボムの所持数 

    protected int speedExp = 0;
    protected int bombRangeExp = 0; // ボムの爆発範囲
    protected int bombCntExp = 0;   // ボムの所持数 

    [SerializeField]
    protected PlayerUI playerUI;
    protected int NowBombCnt;

    protected bool anifirst = false;

    Transform purple;
    Transform cyan;

    Transform activeChild = null;



    // Face, Body, Hair のRendererを取得
    Renderer faceRenderer;
    Renderer bodyRenderer;
    Renderer hairRenderer;
    Renderer hatRenderer;

    /// <summary>
    /// 自身のチーム名を返す関数
    /// </summary>
    public Team CurrentTeamName => TeamName;

    protected virtual void Start()
    {
        //_status = GetComponent<PlayerStatus>();
        //_levelManager = GetComponent<LevelManager>();
        SetStatus();
        playerID = playerIndex;
    }

    protected virtual void Update()
    {
        //プレイヤーの移動
        PlayerMove();
        NowBombCnt = 0;
        foreach (var bomb in BloomBombPool)
        {
            if (!bomb.activeInHierarchy)
            {
                NowBombCnt++;
            }
        }
        playerUI.addNowBombCnt(NowBombCnt);
        if ((GameTimer.instance.IsGameStart()) && !anifirst)
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

    /// <summary>
    /// レベルの変更処理
    /// </summary>

    public void SetStatus()
    {
        SetStatusInternal();
    }

    /*protected virtual void SetStatusInternal()
    {
        if (_status == null) return;
        PlayerSpeed = 2.0f + (_status.GetValue(StatusType.Speed) - 1) * 0.5f;
        BombRange = 1 + (_status.GetValue(StatusType.Power) - 1);
        BloomBombMax = 1 + (_status.GetValue(StatusType.BombCount) - 1);
        for (int i = BloomBombPool.Count; i < BloomBombMax; i++)
        {
            addSetBomb();
        }
    }*/

    protected void SetStatusInternal()
    {
        PlayerSpeed = 2.0f + (speedLevel-1) * 0.3f;
        BombRange = 1 + bombRangeLevel-1;
        BloomBombMax = 1 + bombCntLevel-1;
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
    public virtual void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if ((GameTimer.instance.IsGameStart()))
        {
            anifirst = true;
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
    public virtual void OnBomb(InputAction.CallbackContext context)
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
        Vector3 moveValue = new Vector3(moveInput.x * PlayerSpeed * SpecialPlayerSpeed * Time.timeScale, 0f, moveInput.y * PlayerSpeed * SpecialPlayerSpeed  * Time.timeScale);
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
    protected virtual IEnumerator StartRespawnRoutine()
    {
        // 動けなくする（死亡）
        currentState = PlayerState.Death;
        animator.SetBool("isDeath", true);
        SoundManager.PlaySE("death");
        //Vector2 gridPos = MapManager.Instance.WorldToGridPosition(this.transform.position); 
        Vector2Int pos = MapManager.Instance.GetBlockData((int)this.transform.position.x, (int)this.transform.position.z).gridPosition;
        Vector3 newPos = new Vector3(pos.x, 0f, pos.y);
        NewItemGenerator.Instance.DropExp(newPos, speedLevel+bombRangeLevel+bombCntLevel);
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
        rb.constraints &= ~RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotation;

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
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotation;


        // 動けるようにする（生存）
        currentState = PlayerState.Alive;
        animator.SetBool("isDeath", false);
        Invincibility();
    }
    public bool isInvincibility = false;
    protected Coroutine blinkCoroutine;
    protected Coroutine InvincibilityCoroutine;
    public void Invincibility()
    {
        if (InvincibilityCoroutine != null)
        {
            StopCoroutine(InvincibilityCoroutine);
            InvincibilityCoroutine = null;
        }
        InvincibilityCoroutine = StartCoroutine(InvincibilityTimer());
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
        blinkCoroutine = StartCoroutine(BlinkCoroutine(4.75f, 0.2f));
    }

    IEnumerator InvincibilityTimer()
    {
        isInvincibility = true;
        Debug.Log("無敵化！！");
        yield return new WaitForSeconds(5f);
        isInvincibility = false;
        Debug.Log("無敵化解除！！");
    }
    protected IEnumerator BlinkCoroutine(float duration, float interval)
    {
        float timer = 0f;
        // 子オブジェクトを名前で取得
        purple = transform.Find("purpleChan");
        cyan = transform.Find("cyanChan");
        // どちらがアクティブか判定
        if (purple != null && purple.gameObject.activeInHierarchy)
        {
            activeChild = purple;
            hatRenderer = activeChild.Find("Root/J_Bip_C_Hips/J_Bip_C_Spine/straw hat")?.GetComponentInChildren<Renderer>(true);
        }
        else if (cyan != null && cyan.gameObject.activeInHierarchy)
        {
            activeChild = cyan;
            hatRenderer = activeChild.Find("Root/J_Bip_C_Hips/J_Bip_C_Spine/silk_hat")?.GetComponentInChildren<Renderer>(true);
        }

        faceRenderer = activeChild.Find("Face")?.GetComponent<Renderer>();
        bodyRenderer = activeChild.Find("Body")?.GetComponent<Renderer>();
        hairRenderer = activeChild.Find("Hair")?.GetComponent<Renderer>();

        yield return null;

        while (timer < duration)
        {
            // 表示/非表示を切り替え
            faceRenderer.enabled = !faceRenderer.enabled;
            bodyRenderer.enabled = !bodyRenderer.enabled;
            hairRenderer.enabled = !hairRenderer.enabled;
            hatRenderer.enabled = !hatRenderer.enabled;

            // interval秒待機
            yield return new WaitForSeconds(interval);

            timer += interval;
        }

        // 最後に必ず表示状態に戻す
        faceRenderer.enabled = true;
        bodyRenderer.enabled = true;
        hairRenderer.enabled = true;
        hatRenderer.enabled = true;
    }

    public void WarpPosition(Vector3 warpPos)
    {
        transform.position = new Vector3(warpPos.x, 0f, warpPos.z);
    }


    #region 爆弾関連
    /// <summary>
    /// 爆弾を設置する関数
    /// </summary>
    /// <param name="blockData"></param>
    protected void BombPlacement(MapBlockData blockData)
    {
        if (!MapManager.Instance.GetBlockData(blockData.gridPosition.x, blockData.gridPosition.y).isBomb　&& MapManager.Instance.GetBlockData(blockData.gridPosition.x, blockData.gridPosition.y).name != "WarpRLObject" && MapManager.Instance.GetBlockData(blockData.gridPosition.x, blockData.gridPosition.y).name != "WarpUDObject" && MapManager.Instance.GetBlockData(blockData.gridPosition.x, blockData.gridPosition.y).name != "StartObject")
        {
            Vector3 position = blockData.tilePosition;

            GameObject obj = GetBomb();
            if (obj == null)
            {
                return;
            }
            SoundManager.PlaySE("bloom");
            addReward(DataBase.Instance.placeBomb);
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
        BloomBomb.GetComponent<BombProcess>().PB = this;
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

    private void OnTriggerEnter(Collider other)
    {
        /*if(collision.transform.tag == "Exp" && currentState == PlayerState.Alive)
        {
            _levelManager.AddExp(EXP_SIZE);
            SoundManager.PlaySE("getxp");
            Destroy(collision.gameObject);
            addReward(DataBase.Instance.getExp);
        }*/
        if (other.transform.tag == "SpeedExp" && currentState == PlayerState.Alive)
        {
            speedExp++;
            if (speedExp >= speedLevel)
            {
                speedLevel++;
                speedExp = 0;
                SetStatus();
                SoundManager.PlaySE("PowerUp");
                playerUI.addSpeed(1);
            }
            else
            {
                SoundManager.PlaySE("getxp");
            }
            Destroy(other.gameObject);
            addReward(DataBase.Instance.getExp);
            
        }
        if (other.transform.tag == "RangeExp" && currentState == PlayerState.Alive)
        {
            bombRangeExp++;
            if (bombRangeExp >= bombRangeLevel)
            {
                bombRangeLevel++;
                bombRangeExp = 0;
                SetStatus();
                SoundManager.PlaySE("PowerUp");
                playerUI.addBombRange(1);
            }
            else
            {
                SoundManager.PlaySE("getxp");
            }
            Destroy(other.gameObject);
            addReward(DataBase.Instance.getExp);
        }
        if (other.transform.tag == "CountExp" && currentState == PlayerState.Alive)
        {
            bombCntExp++;
            if (bombCntExp >= bombCntLevel)
            {
                bombCntLevel++;
                bombCntExp = 0;
                SetStatus();
                SoundManager.PlaySE("PowerUp");
            }
            else
            {
                SoundManager.PlaySE("getxp");
            }
            Destroy(other.gameObject);
            addReward(DataBase.Instance.getExp);
        }
    }

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

    public void RespawnPlayer()
    {
        //isFirst = true;
        addReward(DataBase.Instance.death);
        Respawn();
    }

    public void addReward(float point)
    {
        oneReward += point;
    }
}
