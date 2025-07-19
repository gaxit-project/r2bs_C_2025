using System.Collections;
#if UNITY_EDITOR
using UnityEditor.SearchService;
#endif
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCcontroller : PlayerBase
{
    public bool[] pastIsWall = new bool[4] {false,false,false,false};
    private void Awake()
    {
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
        playerCam.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "ignore RayCast", "Water", "UI", "TeamOneTile", "TeamTwoTile", "Player", "Back", "Player" + (playerIndex + 1) + "UI");
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
        NPCMove(new Vector2(teamLocal, 0));
    }

    protected void FixedUpdate()
    {
        base.Update();
        bool[] isWall = IsNearWall();
        //if (isFirst && MapManager.Instance.GetBlockData(CatchPlayerPos().gridPosition.x, CatchPlayerPos().gridPosition.y).name != $"StartObject") isFirst = false;
        /*if (!isFirst && MapManager.Instance.GetBlockData(CatchPlayerPos().gridPosition.x, CatchPlayerPos().gridPosition.y).name == $"StartObject")
        {
            NPCMove(new Vector2(moveInput.x*-1, 0));
        }
        else */if(chescChangeIsWall(isWall))
        {
            NPCMove(IsGoDir(isWall));
        }
        pastIsWall = isWall;
    }

    private bool[] IsNearWall()
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
    }

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
