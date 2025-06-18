using UnityEngine;
using UnityEngine.UIElements;

public class BloomTile : MonoBehaviour
{
    [SerializeField] GameObject[] TeamOneTile;  // チーム1のタイル
    [SerializeField] GameObject[] TeamTwoTile;  // チーム2のタイル

    [SerializeField] Transform TeamOneTileParent;  // チーム1のオブジェクト入れるやつ
    [SerializeField] Transform TeamTwoTileParent;  // チーム2のオブジェクト入れるやつ



    public static BloomTile Instance;
    private void Awake()
    {
        Instance = this;
    }
    /// <summary>
    /// タイルを変更するオブジェクト
    /// </summary>
    /// <param name="teamName"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void TileChange(Team teamName, Renderer renderer, int x, int y)
    {
        GameObject obj = null;  // オブジェクト生成用
        // オブジェクト生成時の座標
        int reversedX = (MapManager.Instance.Width - 1) - x;
        float tileSize = 1f;
        Vector3 spawnPos = new Vector3(reversedX * tileSize + tileSize / 2f, -0.25f, y * tileSize + tileSize / 2f);
        Transform TF = renderer.transform;

        // オブジェクト番号の変数
        int rnd = 0;

        switch (teamName)
        {
            case Team.TeamOne:
                // 同じ色なら何もしない
                if (renderer.gameObject.layer == LayerMask.NameToLayer("TeamOneTile"))
                {
                    break;
                }
                // 違う色なら消す
                else if (renderer.gameObject.layer == LayerMask.NameToLayer("TeamTwoTile"))
                {
                    GameObject.Destroy(TF.GetChild(0).gameObject);
                }
                rnd = Random.Range(0, TeamOneTile.Length);
                obj = Instantiate(TeamOneTile[rnd], spawnPos, Quaternion.identity, TF);
                break;



            case Team.TeamTwo:
                // 違う色なら消す
                if (renderer.gameObject.layer == LayerMask.NameToLayer("TeamOneTile"))
                {
                    GameObject.Destroy(TF.GetChild(0).gameObject);
                }
                // 同じ色なら何もしない
                else if (renderer.gameObject.layer == LayerMask.NameToLayer("TeamTwoTile"))
                {
                    break;
                }
                rnd = Random.Range(0, TeamTwoTile.Length);
                obj = Instantiate(TeamTwoTile[rnd], spawnPos, Quaternion.identity, TF);
                break;
        }
    }
}
