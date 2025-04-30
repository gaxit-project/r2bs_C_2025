using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using static MapManager;

public class BombProcess : MonoBehaviour
{
    //爆発終了フラグ
    private bool isLeftPaint = true;  // 左終了フラグ
    private bool isRightPaint = true; // 右終了フラグ
    private bool isUpPaint = true;    // 上終了フラグ
    private bool isDownPaint = true;  // 下終了フラグ

    [SerializeField] private GameObject _hitObject;     // 当たり判定オブジェくト
    private GameObject _hitJudgementObj;// 当たり判定オブジェクトの生成先のオブジェクト
    private Transform _hitObjectParent; // 当たり判定オブジェクトの生成先オブジェクト


    private Coroutine _currentCoroutine;


    // 引数を保存する変数
    private int _bombRange;   // 爆破範囲
    private Color _bombColor; // 爆弾の色
    private MapBlockData _blockData; // 座標
    private Team _teamName;        // チーム名

    [SerializeField] private float _spreadTime = 0.2f;       // 起爆範囲が1マス広がるまでの秒数
    [SerializeField] private float _startSpreadTime = 2.5f;  // 起爆開始までの秒数


    public static BombProcess Instance;
    private void Awake()
    {
        Instance = this;
        _hitObject = Resources.Load<GameObject>("Prefab/HitObject");
        _hitJudgementObj = GameObject.Find("HitObjGenerate");
        _hitObjectParent = _hitJudgementObj.transform;
        this.gameObject.tag = "FlowerBomb";
    }


    private void Update()
    {
        if(!isLeftPaint && !isRightPaint && !isUpPaint && !isDownPaint)
        {
            //時間があればオブジェクトプールへ変更
            this.gameObject.SetActive(false);
            ResetBomb();
        }
    }

    /// <summary>
    /// 爆弾を元の位置に戻す
    /// </summary>
    public void ResetBomb()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        _currentCoroutine = null;
        isLeftPaint = isRightPaint = isUpPaint = isDownPaint = true;
    }




    /// <summary>
    /// コルーチン呼び出し関数
    /// </summary>
    public void StartBombCoutDownCoroutine(int bombRange, Color BombColor, MapBlockData blockData, Team teamName)
    {
        VarSetting(bombRange, BombColor, blockData, teamName);
        _currentCoroutine = StartCoroutine(StartBombCountDown());
    }



    /// <summary>
    /// 爆破までのカウントダウンコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator StartBombCountDown()
    {
        yield return new WaitForSeconds(_startSpreadTime);
        MapSetting();
    }




    /// <summary>
    /// 色を塗る場所を確かめる関数
    /// </summary>
    /// <param name="bombRange"></param>
    /// <param name="blockData"></param>
    private void MapSetting()
    {
        this.gameObject.tag = "Untagged";
        StartCoroutine(PaintJudge(Vector2Int.left));
        StartCoroutine(PaintJudge(Vector2Int.right));
        StartCoroutine(PaintJudge(Vector2Int.up));
        StartCoroutine(PaintJudge(Vector2Int.down));
        #region もし即座に起爆したいときにここをコメント外す
        //Vector2Int pos = blockData.gridPosition;

        //// 右を確かめる
        //for (int i = 0; i <= bombRange; i++)
        //{
        //    if (MapManager.Instance.GetBlockData(pos.x + i, pos.y).name == "GroundObject")
        //    {
        //        PaintMap(pos.x + i, pos.y, BombColor);
        //    }
        //    else
        //    {
        //        break;
        //    }
        //}


        //// 左を確かめる
        //for (int i = 0; i <= bombRange; i++)
        //{
        //    if (MapManager.Instance.GetBlockData(pos.x - i, pos.y).name == "GroundObject")
        //    {
        //        PaintMap(pos.x - i, pos.y, BombColor);
        //    }
        //    else
        //    {
        //        break;
        //    }
        //}


        //// 上を確かめる
        //for (int i = 0; i <= bombRange; i++)
        //{
        //    if (MapManager.Instance.GetBlockData(pos.x, pos.y + i).name == "GroundObject")
        //    {
        //        PaintMap(pos.x, pos.y + i, BombColor);
        //    }
        //    else
        //    {
        //        break;
        //    }
        //}


        //// 下を確かめる
        //for (int i = 0; i <= bombRange; i++)
        //{
        //    if (MapManager.Instance.GetBlockData(pos.x, pos.y - i).name == "GroundObject")
        //    {
        //        PaintMap(pos.x, pos.y - i, BombColor);
        //    }
        //    else
        //    {
        //        break;
        //    }
        //}
        #endregion
    }



    /// <summary>
    /// 塗れるかの判定を取る
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    IEnumerator PaintJudge(Vector2Int direction)
    {
        Vector2Int pos = _blockData.gridPosition;
        // 右を確かめる
        for (int i = 0; i <= _bombRange; i++)
        {
            int targetX = pos.x + direction.x * i;
            int targetY = pos.y + direction.y * i;


            if (MapManager.Instance.GetBlockData(targetX, targetY).name == "GroundObject")
            {

                PaintMap(targetX, targetY);
            }
            else if (MapManager.Instance.GetBlockData(targetX, targetY).name == "BreakWallObject")
            {
                // 破壊処理＋床生成処理
                yield return new WaitForSeconds(_spreadTime);
                MapManager.Instance.ChangeBlock(targetX, targetY);

                break;
            }
            else
            {
                break;
            }
            yield return new WaitForSeconds(_spreadTime);
        }
        if (direction == Vector2Int.left) isLeftPaint = false;
        else if (direction == Vector2Int.right) isRightPaint = false;
        else if (direction == Vector2Int.up) isUpPaint = false;
        else if (direction == Vector2Int.down) isDownPaint = false;
    }

  
    /// <summary>
    /// マップに色を付ける
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void PaintMap(int x, int y)
    {
        Renderer renderer = MapManager.Instance.GetBlockData(x, y).instance.GetComponent<Renderer>();
        // 当たり判定の生成
        Vector3 position = MapManager.Instance.GetBlockData(x, y).tilePosition;
        GameObject obj = Instantiate(_hitObject, position, Quaternion.identity, _hitObjectParent);
        BloomHitJudgment BHJ = obj.GetComponent<BloomHitJudgment>();
        BHJ.StartJudgementCountDownCoroutine(_teamName);
        switch (_teamName)
        {
            case Team.TeamOne:
                // レンダーが違うときに塗り割合を変更する
                if (renderer.gameObject.layer == LayerMask.NameToLayer("TeamTwoTile")) BloomJudgement.Instance.RemoveBloomJudgement(_teamName);
                // 白紙の時は塗り割合を加算する
                else if(renderer.gameObject.layer != LayerMask.NameToLayer("TeamOneTile"))BloomJudgement.Instance.AddBloomJudgement(_teamName);
                // レンダー変更
                renderer.gameObject.layer = LayerMask.NameToLayer("TeamOneTile");
                break;
            case Team.TeamTwo:
                // レンダーが違うときに塗り割合を変更する
                if (renderer.gameObject.layer == LayerMask.NameToLayer("TeamOneTile")) BloomJudgement.Instance.RemoveBloomJudgement(_teamName);
                // 白紙の時は塗り割合を加算する
                else if(renderer.gameObject.layer != LayerMask.NameToLayer("TeamTwoTile"))BloomJudgement.Instance.AddBloomJudgement(_teamName);
                // レンダー変更
                renderer.gameObject.layer = LayerMask.NameToLayer("TeamTwoTile");
                break;
        }
        renderer.material.color = _bombColor;
    }


    /// <summary>
    /// 引数のセッティング
    /// </summary>
    /// <param name="bombRange"></param>
    /// <param name="BombColor"></param>
    /// <param name="blockData"></param>
    /// <param name="teamName"></param>
    public void VarSetting(int bombRange, Color BombColor, MapBlockData blockData, Team teamName)
    {
        _bombRange = bombRange;
        _bombColor = BombColor;
        _blockData = blockData;
        _teamName = teamName;
    }



    /// <summary>
    /// 連鎖用の関数
    /// </summary>
    public void ChainBloom()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        Debug.Log("ばいよえーーーん");
        MapSetting();
    }
}
