using UnityEngine;
using UnityEngine.UIElements;

public class GatiHokoGenerate : MonoBehaviour
{
    public Transform GatiHokoTileGenerate;  // ガチホコの生成先タイルの親オブジェクト参照
    public Transform GatiHokoObj; // ガチホコの親オブジェクト参照
    private int _gatiHokoTileCnt; // ガチホコの生成先タイルの数を取得

    [SerializeField] GameObject generatePrefab; // ガチホコオブジェクトを取得

    public static GatiHokoGenerate Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        // ガチホコの生成先個数を取得
        _gatiHokoTileCnt = GatiHokoTileGenerate.childCount;
    }



    /// <summary>
    /// ガチホコを生成する
    /// </summary>
    public void GatiHokoObjGenerate()
    {
        // 生成する位置をランダムに取得
        int gatiHokoGenerateCnt = Random.Range(0, _gatiHokoTileCnt);
        Vector3 gatiHokoPosition = MapManager.Instance.GetGatiHokoPosition(gatiHokoGenerateCnt);

        // ブロックの生成
        GameObject obj = null;
        obj = Instantiate(generatePrefab, gatiHokoPosition, Quaternion.identity, GatiHokoObj);
    }
}
