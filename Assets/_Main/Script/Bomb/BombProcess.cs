using System.Collections;
using UnityEngine;
using static MapManager;
using static UnityEditor.PlayerSettings;

public class BombProcess : MonoBehaviour
{
    public static BombProcess Instance;
    private void Awake()
    {
        Instance = this;
    }



    /// <summary>
    /// コルーチン呼び出し関数
    /// </summary>
    /// <param name="bombRange"></param>
    /// <param name="blockData"></param>
    public void startBombCoutDownCoroutine(int bombRange, string BombColor, MapBlockData blockData)
    {
        StartCoroutine(startBombCountDown(bombRange, BombColor, blockData));
    }



    /// <summary>
    /// 爆破までのカウントダウンコルーチン
    /// </summary>
    /// <param name="bombRange"></param>
    /// <param name="blockData"></param>
    /// <returns></returns>
    IEnumerator startBombCountDown(int bombRange, string BombColor, MapBlockData blockData)
    {
        yield return new WaitForSeconds(2.5f);
        MapSetting(bombRange, BombColor, blockData);

        Destroy(this.gameObject);
    }




    /// <summary>
    /// 色を塗る場所を確かめる関数
    /// </summary>
    /// <param name="bombRange"></param>
    /// <param name="blockData"></param>
    private void MapSetting(int bombRange, string BombColor, MapBlockData blockData)
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
        }


        // 左を確かめる
        for (int i = 0; i <= bombRange; i++)
        {
            if (MapManager.Instance.GetBlockData(pos.x - i, pos.y).name == "GroundObject")
            {
                PaintMap(pos.x - i, pos.y, BombColor);
            }
            else
            {
                break;
            }
        }


        // 上を確かめる
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
        }


        // 下を確かめる
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
        }        
    }



    public void PaintMap(int x, int y, string BombColor)
    {
        Renderer renderer = MapManager.Instance.GetBlockData(x, y).instance.GetComponent<Renderer>();
        switch(BombColor)
        {
            case "red":
                renderer.material.color = Color.red;
                break;
            case "blue":
                renderer.material.color = Color.blue;
                break;
        }
    }
}
