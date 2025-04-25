using System.Globalization;
using UnityEngine;

public class MapData : MonoBehaviour
{
    private string _csvFileName = "stage1";       // csv読み込み
    public Transform TileParent;      // 床オブジェクトの生成先オブジェクト
    public Transform WallParent;      // 壁オブジェクトの生成先オブジェクト
    public Transform BreakWallParent; // 壊れる壁オブジェクトの生成先オブジェクト
    [SerializeField] private GameObject[] parentObject; // ゲームリセット時の消す親オブジェクト

    private float _tileSize = 1f;             // 1マスのサイズ
    private MapBlockData[,] _mapGrid;         // 保存先の二次元配列

    private int _width;                       // マップの横幅
    private int _height;                      // マップの立幅


    private const int KEYSELECTNUM = 10;

    // ステージ用のPrefab
    [SerializeField] private GameObject[] _groundPrefab;     // 歩行可能マス key: 0～9
    [SerializeField] private GameObject[] _wallPrefab;       // ブロックマス key: 10～19
    [SerializeField] private GameObject[] _breakablePrefab;  // 壊れる壁マス key: 20～29
    [SerializeField] private GameObject[] _itemBoxPrefab;    // アイテムマス key: 30～39

    /// <summary>
    /// マップのブロックごとの設定
    /// </summary>
    public class MapBlockData
    {
        public int key;             // オブジェクトの属性キー
        public string name;         // オブジェクトの名称
        public bool isWalkable;     // 歩行可能かどうかのフラグ
        public Vector3 tilePosition;// オブジェクトのポジション
        public GameObject instance; // 生成されたオブジェクトの参照
    }
    public int Width => _width;   // ボム用のタイルの横幅をpublic化

    public static MapData Instance;
    private void Awake()
    {
        Instance = this;
        // ステージ作成
        //LoadMap();
    }



    # region マップを生成する関数

    /// <summary>
    /// マップを出現させる
    /// </summary>
    public void LoadMap()
    {
        if (_csvFileName == null)
        {
            Debug.LogError("csvファイルが存在しません");
            return;
        }
        
        TextAsset csvData = Resources.Load<TextAsset>(_csvFileName);
        string[] lines = csvData.text.Trim().Split('\n');

        _height = lines.Length;
        _width = lines[0].Split(',').Length;
        _mapGrid = new MapBlockData[_width, _height];

        for (int y = 0; y < _height; y++)
        {
            string[] row = lines[y].Trim().Split(',');

            for (int x = 0; x < _width; x++)
            {
                int key = int.Parse(row[x]);

                // ブロックを設置するマスを計算
                int reversedX = (_width - 1) - x;
                Vector3 position = new Vector3(reversedX * _tileSize + _tileSize / 2f, 0f, y * _tileSize + _tileSize / 2f);
    
                // ブロックの状態の宣言
                GameObject obj = null;
                bool isWalkable = false;
                string name = "Unknown";

                // ブロックのカテゴリーとタイプの設定
                int category = key / KEYSELECTNUM;
                int type = key % KEYSELECTNUM;


                // ブロック生成
                switch (category)
                {
                    // 通路ブロック
                    case 0: 
                        obj = Instantiate(_groundPrefab[0], position, Quaternion.identity, TileParent);
                        name = "GroundObject";
                        isWalkable = true;
                        break;



                    // 壁ブロック
                    case 1: 
                        obj = Instantiate(_wallPrefab[type], position, Quaternion.identity, WallParent);
                        name = $"WallObject";
                        break;



                    // 壊せるブロック
                    case 2: 
                        obj = Instantiate(_breakablePrefab[type], position, Quaternion.identity, BreakWallParent);
                        name = $"BreakWallObject";
                        break;



                    // アイテム付きブロック
                    case 3:
                        obj = Instantiate(_itemBoxPrefab[type], position, Quaternion.identity, TileParent);
                        name = $"ItemWallObject";
                        break;
                }


                // ブロックの情報を二次元配列に保存
                _mapGrid[x, y] = new MapBlockData
                {
                    key = key,
                    name = name,
                    isWalkable = isWalkable,
                    tilePosition = position,
                    instance = obj
                };
            }
        }
    }

    #endregion



    #region マップを選択する関数
    /// <summary>
    /// マップ選択用関数
    /// </summary>
    /// <param name="i"></param>
    public void SelectMap(int i)
    {
        _csvFileName = "stage" + i;
    }
    #endregion



    #region マップを初期化する関数
    /// <summary>
    /// マップを初期化
    /// </summary>
    public void ResetMap()
    {
        for (int i = 0; i < parentObject.Length; i++)
        {
            int count = parentObject[i].transform.childCount;
            for (int j = count - 1; j >= 0; j--)
            {
                Transform child = parentObject[i].transform.GetChild(j);
                GameObject.Destroy(child.gameObject);
            }
        }
    }
    #endregion



    #region マップの状態を確認する関数

    /// <summary>
    /// keyがなんなのかを確認する
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    string GetNameByKey(int key)
    {
        switch (key)
        {
            case 0: return "Ground";
            case 1: return "Wall";
            case 2: return "BreakWall";
            default: return "Unknown";
        }
    }


    /// <summary>
    /// 通行可能かどうかを確認する
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    bool IsWalkableTile(int key)
    {
        return key == 0;
    }



    /// <summary>
    /// ブロックの状態を確認できる
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    #region 関数の使い方
    // こう使う
    // MapBlockData data = GetBlockData(1, 1);
    // Debug.Log("position"+data.tilePosition);
    // Debug.Log("key"+data.key);
    // Debug.Log("name"+data.name);
    // Debug.Log("walk"+data.isWalkable);
    #endregion
    public MapBlockData GetBlockData(int x, int y)
    {
        if (x >= 0 && x < _width && y >= 0 && y < _height)
            return _mapGrid[x, y];
        return null;
    }

    #endregion
}
