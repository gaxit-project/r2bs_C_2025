using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCBase : PlayerBase
{
    // 敵を取得
    public List<GameObject> enemy = new List<GameObject>();
    // 敵を取得
    public List<GameObject> team = new List<GameObject>();

    // 経験値をサーチする者たち
    float expSearchInterval = 0.5f;
    float expTimer = 0f;
    GameObject expTargets;
    [SerializeField] private float expSearchRange = 10f;      // 経験値を探す範囲
    [SerializeField] private LayerMask expLayer;             // 経験値のレイヤー

    float expPSearchInterval = 0.5f;
    float expPTimer = 0f;
    GameObject expPTargets;
    [SerializeField] private float expPSearchRange = 10f;      // 経験値を探す範囲
    [SerializeField] private LayerMask expPLayer;             // 経験値のレイヤー


    // 敵をサーチする者たち
    float enemySearchInterval = 0.3f;
    float enemyTimer = 0f;
    GameObject nearestEnemy;
    [SerializeField] private float enemySearchRange = 2f;
    [SerializeField] private LayerMask enemyLayer;

    // 味方をサーチする者たち
    float teamSearchInterval = 0.3f;
    float teamTimer = 0f;
    GameObject nearestTeam;
    [SerializeField] private float teamSearchRange = 2f;
    [SerializeField] private LayerMask teamLayer;


    // エリアをサーチする者たち
    float areaSearchInterval = 1f;
    float areaTimer = 0f;
    GameObject goArea;


    float _startTimer;
    float _waveTimer;

    public enum NPCState
    {
        chase,     // 敵を追う
        paint,     // 色塗り(基本使わん)
        area,      // エリア取り
        exp,       // 経験値あつめ
        expP,
        escape,    // 逃げ
        escapeT,
    }

    public NPCState currentNPCState;


    protected override void Start()
    {
        base.Start();
        // 敵を取得
        PlayerBase[] allPlayers = Object.FindObjectsByType<PlayerBase>(FindObjectsSortMode.None);

        foreach (var player in allPlayers)
        {
            if (player != this && player.CurrentTeamName != this.CurrentTeamName)
            {
                enemy.Add(player.gameObject);
            }else if(player != this && player.CurrentTeamName == this.CurrentTeamName)
            {
                team.Add(player.gameObject);
            }
        }
        enemyLayer = 1 << LayerMask.NameToLayer("Player");
        teamLayer = 1 << LayerMask.NameToLayer("Player");
        expLayer = 1 << LayerMask.NameToLayer("Exp");
        expPLayer = 1 << LayerMask.NameToLayer("ExpP");


        _startTimer = GameTimer.instance.StartTime;
        _waveTimer = _startTimer * 0.6f;

        SearchArea();
        SearchEnemy();
        SearchTeam();
        SearchEXP();
        SearchEXPP();
    }


    private new void Update()
    {
        // インターバル時間を計算
        expTimer += Time.deltaTime;
        expPTimer += Time.deltaTime;
        enemyTimer += Time.deltaTime;
        teamTimer += Time.deltaTime;
        areaTimer += Time.deltaTime;
    }

    protected virtual void FixedUpdate()
    {
        base.Update();

        // 経験値の場所をサーチ
        if (expTimer >= expSearchInterval)
        {
            SearchEXP();
            expTimer = 0f;
        }

        // 経験値の場所をサーチ
        if (expPTimer >= expPSearchInterval)
        {
            SearchEXPP();
            expPTimer = 0f;
        }

        // 敵の場所をサーチ
        if (enemyTimer >= enemySearchInterval)
        {
            SearchEnemy();
            enemyTimer = 0f;
        }

        // 味方の場所をサーチ
        if (teamTimer >= teamSearchInterval)
        {
            SearchTeam();
            teamTimer = 0f;
        }

        // エリアの場所をサーチ
        if (areaTimer >= areaSearchInterval)
        {
            SearchArea();
            areaTimer = 0f;
        }


        if (GameTimer.instance.CurrentTime >= _waveTimer && expSearchRange != 1f)
        {
            expSearchRange = 1f;
            expSearchInterval = 2f;
            expPSearchRange = 1f;
            expPSearchInterval = 2f;
            areaSearchInterval = 0.5f;
            Debug.Log("変更！！！");
        }

        // ステート更新
        StateChange();
    }


    protected void StateChange()
    {
        // 敵が近くにいたらchase(あとからレベル差も考慮出来たらおもしろそう)
        //if (nearestEnemy != null /*&& bombs > 0*/)
        //{
        //currentNPCState = NPCState.chase;
        //Debug.Log("チェイスに移動");
        //}
        if (nearestTeam != null /*&& bombs == 0*/)
        {
            currentNPCState = NPCState.escapeT;
            Debug.Log("逃げるT");
        }

        // 敵が近くにいて爆弾を持ってなければescape
        else if (nearestEnemy != null /*&& bombs == 0*/)
        {
            currentNPCState = NPCState.escape;
            Debug.Log("逃げる");
        }


        // 一定の範囲かつガチエリアにいないとき or 最初の1分くらいはexp
        else if (expPTargets != null)
        {
            currentNPCState = NPCState.expP;
            Debug.Log("経験値稼ぎP");
        }

        else if (expTargets != null)
        {
            currentNPCState = NPCState.exp;
            Debug.Log("経験値稼ぎ");
        }

        // 基本はarea
        else if(goArea != null) 
        {
            currentNPCState = NPCState.area;
            Debug.Log("エリア塗り");
        }
    }


    
    /// <summary>
    /// 経験値をサーチ
    /// </summary>
    void SearchEXP()
    {
        Collider[] results = Physics.OverlapSphere(transform.position, expSearchRange, expLayer);
        foreach (var c in results)
        {
            Debug.Log($"HitExp: {c.name}");
        }
        GameObject found = results
            .Select(c => c.gameObject)
            .OrderBy(go => Vector3.Distance(transform.position, go.transform.position))
            .FirstOrDefault();

        expTargets = found;
    }

    /// <summary>
    /// 経験値をサーチ
    /// </summary>
    void SearchEXPP()
    {
        Collider[] results = Physics.OverlapSphere(transform.position, expPSearchRange, expPLayer);
        foreach (var c in results)
        {
            Debug.Log($"HitExpP: {c.name}");
        }
        GameObject found = results
            .Select(c => c.gameObject)
            .OrderBy(go => Vector3.Distance(transform.position, go.transform.position))
            .FirstOrDefault();
        if(found != null && Vector3.Distance(transform.position, found.transform.position) > 0.5)
        {
            expPTargets = found;
        }
        else
        {
            expPTargets = null;
        }
    }



    /// <summary>
    /// 敵をサーチ
    /// </summary>
    void SearchEnemy()
    {
        Collider[] results = Physics.OverlapSphere(transform.position, enemySearchRange, enemyLayer);
        if (results.Length == 0)
        {
            nearestEnemy = null;
            return;
        }
        

        // enemyだけを取得する
        var filtered = results
            .Select(c => c.gameObject)
            .Where(go => enemy.Contains(go))
            .ToList();

        if (filtered.Count == 0)
        {
            nearestEnemy = null;
            return;
        }

        // 一番近いものを選ぶ
        nearestEnemy = filtered
            .OrderBy(go => Vector3.Distance(transform.position, go.transform.position))
            .First();
    }

    /// <summary>
    /// 味方をサーチ
    /// </summary>
    void SearchTeam()
    {
        Collider[] results = Physics.OverlapSphere(transform.position, teamSearchRange, teamLayer);
        if (results.Length == 0)
        {
            nearestTeam = null;
            return;
        }


        // teamだけを取得する
        var filtered = results
            .Select(c => c.gameObject)
            .Where(go => team.Contains(go))
            .ToList();

        if (filtered.Count == 0)
        {
            nearestTeam = null;
            return;
        }

        // 一番近いものを選ぶ
        nearestTeam = filtered
            .OrderBy(go => Vector3.Distance(transform.position, go.transform.position))
            .First();
    }



    /// <summary>
    /// エリアをサーチ
    /// </summary>
    void SearchArea()
    {
        Vector3 myPos = transform.position;

        // 一番近いエリアを取得
        float minAreaDist = Mathf.Infinity;
        int targetAreaIndex = -1;

        for (int i = 0; i < GatiArea.Instance.currentAreaTeam.Length; i++)
        {
            if (GatiArea.Instance.currentAreaTeam[i] != this.CurrentTeamName)
            {
                Transform parent = GatiArea.Instance.GatiAreaGenerate[i];
                if (parent.childCount == 0) continue;

                float dist = Vector3.Distance(myPos, parent.GetChild(0).position);
                if (dist < minAreaDist)
                {
                    minAreaDist = dist;
                    targetAreaIndex = i;
                }
            }
        }

        if (targetAreaIndex >= 0)
        {
            // エリアの中で近いものを選出
            if (targetAreaIndex >= 0)
            {
                Transform parent = GatiArea.Instance.GatiAreaGenerate[targetAreaIndex];

                GameObject nearestTile = null;
                float minDist = Mathf.Infinity;

                foreach (Transform tile in parent)
                {
                    // 既に自チームで塗られているタイルは除外
                    int layer = tile.gameObject.layer;
                    if ((this.CurrentTeamName == Team.TeamOne && layer == LayerMask.NameToLayer("TeamOneTile")) ||
                         (this.CurrentTeamName == Team.TeamTwo && layer == LayerMask.NameToLayer("TeamTwoTile")))
                    {
                        continue;
                    }

                    float d = Vector3.Distance(myPos, tile.position);
                    if (d < minDist)
                    {
                        nearestTile = tile.gameObject;
                        minDist = d;
                    }
                }

                goArea = nearestTile;
            }
        }
        else
        {
            ForceChase();
        }
    }

    private void ForceChase()
    {
        // enemyリスト自体の中から一番近いものを直接探す
        if (enemy.Count > 0)
        {
            GameObject nearest = null;
            float minDist = Mathf.Infinity;
            foreach (var e in enemy)
            {
                float d = Vector3.Distance(transform.position, e.transform.position);
                if (d < minDist)
                {
                    minDist = d;
                    nearest = e;
                }
            }
            goArea = nearest;
        }
    }



    private void OnDrawGizmosSelected()
    {
        // 敵探索範囲 (赤色)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemySearchRange);

        // 経験値探索範囲 (青色)
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, expSearchRange);
    }

    // 値を返す
    protected Vector3 GetPosition(string name)
    {
        switch(name)
        {
            case "chase":
                //Debug.Log(nearestEnemy.transform.position);
                return nearestEnemy.transform.position;

            case "escape":
                Vector3 dir = (nearestEnemy.transform.position - transform.position).normalized;
                //Debug.Log(transform.position - dir * 3f);
                return transform.position - dir * 2f;

            case "escapeT":
                Vector3 dirT = (nearestTeam.transform.position - transform.position).normalized;
                //Debug.Log(transform.position - dir * 3f);
                return transform.position - dirT * 2f;

            case "exp":
                ///Debug.Log(expTargets.transform.position);
                return expTargets.transform.position;

            case "expP":
                ///Debug.Log(expPTargets.transform.position);
                return expPTargets.transform.position;


            case "area":
                return goArea.transform.position;
            default:
                return nearestEnemy.transform.position;
        }
    }
}
