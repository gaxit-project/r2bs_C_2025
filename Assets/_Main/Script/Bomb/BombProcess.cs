using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using static MapManager;
using static UnityEditor.PlayerSettings;

public class BombProcess : MonoBehaviour
{
    //爆発終了フラグ
    private bool isLeftPaint = true;
    private bool isRightPaint = true;
    private bool isUpPaint = true;
    private bool isDownPaint = true;

    [SerializeField] private GameObject _hitObject;     // 当たり判定オブジェくト
    private GameObject _hitJudgementObj;// 当たり判定オブジェクトの生成先のオブジェクト
    private Transform _hitObjectParent; // 当たり判定オブジェクトの生成先オブジェクト

    public static BombProcess Instance;
    private void Awake()
    {
        Instance = this;
        _hitObject = Resources.Load<GameObject>("Prefab/HitObject");
        _hitJudgementObj = GameObject.Find("HitObjGenerate");
        _hitObjectParent = _hitJudgementObj.transform;
    }

    private void Update()
    {
        if(!isLeftPaint && !isRightPaint && !isUpPaint && !isDownPaint)
        {
            //時間があればオブジェクトプールへ変更
            Destroy(this.gameObject);
        }
    }



    /// <summary>
    /// コルーチン呼び出し関数
    /// </summary>
    /// <param name="bombRange"></param>
    /// <param name="blockData"></param>
    public void StartBombCoutDownCoroutine(int bombRange, Color BombColor, MapBlockData blockData, string teamName)
    {
        StartCoroutine(StartBombCountDown(bombRange, BombColor, blockData, teamName));
    }



    /// <summary>
    /// 爆破までのカウントダウンコルーチン
    /// </summary>
    /// <param name="bombRange"></param>
    /// <param name="blockData"></param>
    /// <returns></returns>
    IEnumerator StartBombCountDown(int bombRange, Color BombColor, MapBlockData blockData, string teamName)
    {
        yield return new WaitForSeconds(2.5f);
        MapSetting(bombRange, BombColor, blockData, teamName);
    }




    /// <summary>
    /// 色を塗る場所を確かめる関数
    /// </summary>
    /// <param name="bombRange"></param>
    /// <param name="blockData"></param>
    private void MapSetting(int bombRange, Color BombColor, MapBlockData blockData, string teamName)
    {
        StartCoroutine(PaintJudge(Vector2Int.left, bombRange, BombColor, blockData, teamName));
        StartCoroutine(PaintJudge(Vector2Int.right, bombRange, BombColor, blockData, teamName));
        StartCoroutine(PaintJudge(Vector2Int.up, bombRange, BombColor, blockData, teamName));
        StartCoroutine(PaintJudge(Vector2Int.down, bombRange, BombColor, blockData, teamName));
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
    /// <param name="bombRange"></param>
    /// <param name="BombColor"></param>
    /// <param name="blockData"></param>
    /// <returns></returns>
    IEnumerator PaintJudge(Vector2Int direction, int bombRange, Color BombColor, MapBlockData blockData, string teamName)
    {
        Vector2Int pos = blockData.gridPosition;
        // 右を確かめる
        for (int i = 0; i <= bombRange; i++)
        {
            int targetX = pos.x + direction.x * i;
            int targetY = pos.y + direction.y * i;


            if (MapManager.Instance.GetBlockData(targetX, targetY).name == "GroundObject")
            {

                PaintMap(targetX, targetY, BombColor, teamName);
            }
            else if (MapManager.Instance.GetBlockData(targetX, targetY).name == "BreakWallObject")
            {
                // 破壊処理＋床生成処理
                yield return new WaitForSeconds(0.2f);
                MapManager.Instance.ChageBlock(targetX, targetY);

                break;
            }
            else
            {
                break;
            }
            yield return new WaitForSeconds(0.2f);
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
    /// <param name="BombColor"></param>
    public void PaintMap(int x, int y, Color BombColor, string teamName)
    {
        Renderer renderer = MapManager.Instance.GetBlockData(x, y).instance.GetComponent<Renderer>();
        // 当たり判定の生成
        Vector3 position = MapManager.Instance.GetBlockData(x, y).tilePosition;
        GameObject obj = Instantiate(_hitObject, position, Quaternion.identity, _hitObjectParent);
        BloomHitJudgment BHJ = obj.GetComponent<BloomHitJudgment>();
        BHJ.StartJudgementCountDownCoroutine(teamName);
        switch (teamName)
        {
            case "TeamOne":
                renderer.gameObject.layer = LayerMask.NameToLayer("TeamOneTile");
                break;
            case "TeamTwo":
                renderer.gameObject.layer = LayerMask.NameToLayer("TeamTwoTile");
                break;
        }
        renderer.material.color = BombColor;
    }
}
