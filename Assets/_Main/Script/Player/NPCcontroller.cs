using System.Collections;
#if UNITY_EDITOR
using UnityEditor.SearchService;
#endif
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class NPCcontroller : NPCBase
{
    public bool[] pastIsWall = new bool[4] {false,false,false,false};
    private NavMeshAgent agent;
    Vector3 targetPos = new Vector3(0, 0, 0);
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = false;
        // ボムのPrefabと生成先オブジェクトの取得
        StandardBomb = Resources.Load<GameObject>("Prefab/StandardBomb");
        GameObject bombParentObj = GameObject.Find("BombGenerate");
        BombParent = bombParentObj.transform;

        //チーム分け
        playerIndex = GameObject.FindGameObjectsWithTag("TeamOne").Length + GameObject.FindGameObjectsWithTag("TeamTwo").Length; ;
        NPCTeamTag();
        TeamSplit();

        InitSpecialStatus();

        Camera playerCam = GetComponentInChildren<Camera>();
        //playerCam.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "ignore RayCast", "Water", "UI", "TeamOneTile", "TeamTwoTile", "Player", "Back", "Player" + (playerIndex + 1) + "UI");

        if (!DataBase.Instance.GetAiMode())
        {
            playerCam.gameObject.SetActive(false);
        }
    }
    private void NPCTeamTag()
    {
        if (GameObject.FindGameObjectsWithTag("TeamOne").Length < 2)
        {
            this.gameObject.tag = "TeamOne";
        }
        else
        {
            this.gameObject.tag = "TeamTwo";
        }
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(LoopBomb());
        //NPCMove(new Vector2(teamLocal, 0));
    }

    bool a = false;
    int reCnt = 0;
    bool rFirst = false;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();

        if (GameTimer.instance.IsGameStart())
        {

            if (!a)
            {
                animator.SetBool("isWalking", true);
                a = true;
                agent.updatePosition = true;
                agent.updateRotation = false;
            }

            reCnt++;
            if (!rFirst && reCnt <= 50 * playerIndex)
            {
                return;
            }
            else
            {
                reCnt = 0;
                rFirst = true;
            }



            // 目的地に到達した、またはまだパスがない場合に、次の目的地を設定する
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance || reCnt >= 400)
            {
                Debug.Log("ポジ損修正");
                MoveToNextRandomPoint();
                reCnt = 0;
            }
        }
        Vector3 velocity = agent.velocity;

        // Agentの移動方向をmoveInput(Vector2)に入れる
        moveInput = new Vector2(velocity.x, velocity.z).normalized;
        //bool[] isWall = IsNearWall();



        // ====================================================================================
        // ここは最初からコメントアウトされてた

        //if (isFirst && MapManager.Instance.GetBlockData(CatchPlayerPos().gridPosition.x, CatchPlayerPos().gridPosition.y).name != $"StartObject") isFirst = false;
        /*if (!isFirst && MapManager.Instance.GetBlockData(CatchPlayerPos().gridPosition.x, CatchPlayerPos().gridPosition.y).name == $"StartObject")
        {
            NPCMove(new Vector2(moveInput.x*-1, 0));
        }
        // ====================================================================================




        else */
        //if (chescChangeIsWall(isWall))
        //{
        //    NPCMove(IsGoDir(isWall));
        //}
        //pastIsWall = isWall;
    }

    // =======================================================================================
    // 追加部

    private void MoveToNextRandomPoint()
    {
        //Vector3 targetPos = transform.position;
        //int tries = 0;
        bool foundTarget = false;

        // chase処理
        if (currentNPCState == NPCState.chase)
        {
            currentNPCState = NPCState.chase;
            targetPos = GetPosition("chase");
            foundTarget = true;
        }


        // escape処理
        else if (currentNPCState == NPCState.escape)
        {
            currentNPCState = NPCState.escape;
            targetPos = GetPosition("escape");
            foundTarget = true;
        }


        // exp処理
        else if (currentNPCState == NPCState.exp)
        {
            currentNPCState = NPCState.exp;
            targetPos = GetPosition("exp");
            foundTarget = true;
        }

        // area処理
        else
        {
            currentNPCState = NPCState.area;
            targetPos = GetPosition("area");
            foundTarget = true;
        }


        // ランダム移動
        //do
        //{
        //    tries++;
        //    // -1 or +1 をランダムに出す
        //    int x = Random.Range(0, 2) * 2 - 1;  // → -1 or 1
        //    int z = Random.Range(0, 2) * 2 - 1;
        //    targetPos = transform.position + new Vector3(x, 0, z);

        //} while (!NavMesh.SamplePosition(targetPos, out var hit, 0.5f, NavMesh.AllAreas) && tries < 10);
        float threshold = 0.2f; 

        if (foundTarget)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPos, out hit, 1f, NavMesh.AllAreas))
            { 
                if (Vector3.Distance(transform.position, hit.position) > threshold)
                {
                    agent.SetDestination(hit.position);
                }
                else
                {
                    // ほぼ座標が同じなので歩行アニメ止めて待機
                    animator.SetBool("isWalking", false);
                }
            }
        }

    }



    protected override IEnumerator StartRespawnRoutine()
    {
        // 動けなくする（死亡）
        if (agent != null) agent.enabled = false;
        currentState = PlayerState.Death;
        animator.SetBool("isDeath", true);
        SoundManager.PlaySE("death");
        //Vector2 gridPos = MapManager.Instance.WorldToGridPosition(this.transform.position); 
        Vector2Int pos = MapManager.Instance.GetBlockData((int)this.transform.position.x, (int)this.transform.position.z).gridPosition;
        Vector3 newPos = new Vector3(pos.x, 0f, pos.y);
        ItemGenerator.Instance.DropExp(newPos, this.GetComponent<LevelManager>().CurrentLevel);
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


        yield return null;
        // 動けるようにする（生存）
        if (agent != null) agent.enabled = true;
        currentState = PlayerState.Alive;
        animator.SetBool("isDeath", false);
        Invincibility();
    }

    // =======================================================================================

    /*private bool[] IsNearWall()
    {
        int index = 0;
        RaycastHit hit;
        bool[] result = new bool[4] {false,false,false,false};
        for (int i = -1; i <= 1; i += 2)
        {
            Physics.Raycast(this.gameObject.transform.GetChild(0).position  , new Vector3(i, 0, 0), out hit);
            Debug.DrawRay(this.gameObject.transform.GetChild(0).position, new Vector3(i, 0, 0),Color.red);
            if (hit.distance < 1f && hit.collider.tag == "Wall")
            {
                result[index] = true;
            }
            index++;
        }
        for (int i = -1; i <= 1; i += 2)
        {
            Physics.Raycast(this.gameObject.transform.GetChild(0).position, new Vector3(0, 0, i), out hit);
            Debug.DrawRay(this.gameObject.transform.GetChild(0).position, new Vector3(0, 0, i), Color.red);
            if (hit.distance < 1f && hit.collider.tag == "Wall")
            {
                result[index] = true;
            }
            index++;
        }
        return result;
    }*/

    private bool chescChangeIsWall(bool[] isWall)
    {
        for (int i = 0; i < 4; i++)
        {
            if (isWall[i] != pastIsWall[i])
            {
                return true;
            }
        }
        return false;
    }

    private bool IsFrontWall(bool[] isWall)
    {
        if(moveInput == new Vector2(-1,0) && isWall[0]) return true;
        if (moveInput == new Vector2(1, 0) && isWall[1]) return true;
        if (moveInput == new Vector2(0, -1) && isWall[2]) return true;
        if (moveInput == new Vector2(0, 1) && isWall[3]) return true;
        return false;
    }


    private Vector2 IsGoDir(bool[] isWall)
    {
        // ランダムな方向に動く
        int dirInt = Random.Range(0, 4);
        for (int i = 0; i < 4; i++)
        {
            if(dirInt == i && !isWall[i])
            {
                if(i == 0)
                {
                    return new Vector2(-1,0);
                }
                else if (i == 1)
                {
                    return new Vector2(1, 0);
                }
                else if (i == 2)
                {
                    return new Vector2(0, -1);
                }
                else if (i == 3)
                {
                    return new Vector2(0, 1);
                }
            }
        }
        return IsGoDir(isWall);
    }

    private void NPCMove(Vector2 dir)
    {
        moveInput = dir;
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
    public void NPCBomb()
    {
        if (currentState == PlayerState.Alive && GameTimer.instance.IsGameStart())
        {
            BombPlacement(CatchPlayerPos());
        }
    }

    private IEnumerator LoopBomb()
    {
        yield return new WaitForSeconds(1f);
        NPCBomb();
        StartCoroutine(LoopBomb());
    }
}
