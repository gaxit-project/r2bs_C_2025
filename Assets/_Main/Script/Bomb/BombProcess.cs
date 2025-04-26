using System.Collections;
using UnityEngine;
using static MapManager;
using static UnityEditor.PlayerSettings;

public class BombProcess : MonoBehaviour
{
    //爆発終了フラグ
    private bool isLeftPaint = true;
    private bool isRightPaint = true;
    private bool isUpPaint = true;
    private bool isDownPaint = true;

    public static BombProcess Instance;
    private void Awake()
    {
        Instance = this;
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
    public void StartBombCoutDownCoroutine(int bombRange, Color BombColor, MapBlockData blockData)
    {
        StartCoroutine(StartBombCountDown(bombRange, BombColor, blockData));
    }



    /// <summary>
    /// 爆破までのカウントダウンコルーチン
    /// </summary>
    /// <param name="bombRange"></param>
    /// <param name="blockData"></param>
    /// <returns></returns>
    IEnumerator StartBombCountDown(int bombRange, Color BombColor, MapBlockData blockData)
    {
        yield return new WaitForSeconds(2.5f);
        MapSetting(bombRange, BombColor, blockData);
    }




    /// <summary>
    /// 色を塗る場所を確かめる関数
    /// </summary>
    /// <param name="bombRange"></param>
    /// <param name="blockData"></param>
    private void MapSetting(int bombRange, Color BombColor, MapBlockData blockData)
    {
        StartCoroutine(LeftPaint(bombRange, BombColor, blockData));
        StartCoroutine(RightPaint(bombRange, BombColor, blockData));
        StartCoroutine(UpPaint(bombRange, BombColor, blockData));
        StartCoroutine(DownPaint(bombRange, BombColor, blockData));
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
    /// 左側を塗る
    /// </summary>
    /// <param name="bombRange"></param>
    /// <param name="BombColor"></param>
    /// <param name="blockData"></param>
    /// <returns></returns>
    IEnumerator LeftPaint(int bombRange, Color BombColor, MapBlockData blockData)
    {
        Vector2Int pos = blockData.gridPosition;
        // 右を確かめる
        for (int i = 0; i <= bombRange; i++)
        {
            if (MapManager.Instance.GetBlockData(pos.x - i, pos.y).name == "GroundObject")
            {
                Debug.Log("ペイント！");
                PaintMap(pos.x - i, pos.y, BombColor);
            }
            else
            {
                Debug.Log("ペイント終了！");
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        isLeftPaint = false;
    }


    /// <summary>
    /// 右側を塗る
    /// </summary>
    /// <param name="bombRange"></param>
    /// <param name="BombColor"></param>
    /// <param name="blockData"></param>
    /// <returns></returns>
    IEnumerator RightPaint(int bombRange, Color BombColor, MapBlockData blockData)
    {
        Vector2Int pos = blockData.gridPosition;
        // 右を確かめる
        for (int i = 0; i <= bombRange; i++)
        {
            if (MapManager.Instance.GetBlockData(pos.x + i, pos.y).name == "GroundObject")
            {
                PaintMap(pos.x + i, pos.y, BombColor);
            }
            else
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        isRightPaint = false;
    }



    /// <summary>
    /// 上側を塗る
    /// </summary>
    /// <param name="bombRange"></param>
    /// <param name="BombColor"></param>
    /// <param name="blockData"></param>
    /// <returns></returns>
    IEnumerator UpPaint(int bombRange, Color BombColor, MapBlockData blockData)
    {
        Vector2Int pos = blockData.gridPosition;
        // 右を確かめる
        for (int i = 0; i <= bombRange; i++)
        {
            if (MapManager.Instance.GetBlockData(pos.x, pos.y + i).name == "GroundObject")
            {
                PaintMap(pos.x, pos.y + i, BombColor);
            }
            else
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        isUpPaint = false;
    }



    /// <summary>
    /// 下側を塗る
    /// </summary>
    /// <param name="bombRange"></param>
    /// <param name="BombColor"></param>
    /// <param name="blockData"></param>
    /// <returns></returns>
    IEnumerator DownPaint(int bombRange, Color BombColor, MapBlockData blockData)
    {
        Vector2Int pos = blockData.gridPosition;
        // 右を確かめる
        for (int i = 0; i <= bombRange; i++)
        {
            if (MapManager.Instance.GetBlockData(pos.x, pos.y - i).name == "GroundObject")
            {
                PaintMap(pos.x, pos.y - i, BombColor);
            }
            else
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        isDownPaint = false;
    }

    public void PaintMap(int x, int y, Color BombColor)
    {
        Renderer renderer = MapManager.Instance.GetBlockData(x, y).instance.GetComponent<Renderer>();
        renderer.material.color = BombColor;
    }
}
