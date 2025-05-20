using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MapManager : MonoBehaviour
{
    private string _csvFileName = "stage3";       // csv読み込み
    public Transform TileParent;      // 床オブジェクトの生成先オブジェクト
    public Transform WallParent;      // 壁オブジェクトの生成先オブジェクト
    public Transform BreakWallParent; // 壊れる壁オブジェクトの生成先オブジェクト
    public Transform StartTileParent; // 壊れる壁オブジェクトの生成先オブジェクト
    public Transform GatiAreaTileParent; // ガチエリアの生成先オブジェクト
    public Transform GatiHokoTileParent; // ガチエリアの生成先オブジェクト
    [SerializeField] private GameObject[] parentObject; // ゲームリセット時の消す親オブジェクト

    private float _tileSize = 1f;             // 1マスのサイズ
    private MapBlockData[,] _mapGrid;         // 保存先の二次元配列

    private int _width;                       // マップの横幅
    private int _height;                      // マップの立幅


    private const int KEY_SELECT_NUM = 10;

    // ステージ用のPrefab
    [SerializeField] private GameObject[] _groundPrefab;     // 歩行可能マス key: 0～9
    [SerializeField] private GameObject[] _wallPrefab;       // ブロックマス key: 10～19
    [SerializeField] private GameObject[] _breakWallPrefab;  // 壊れる壁マス key: 20～29
    [SerializeField] private GameObject[] _itemBoxPrefab;    // アイテムマス key: 30～39
    [SerializeField] private GameObject[] _startTilePrefab;    // アイテムマス key: 30～39

    [SerializeField] private Vector3[] _startPosition;    // スタートポジションを入れる配列
    [SerializeField] private Vector3[] _gatiHokoPosition;    // ガチホコポジションを入れる配列

    /// <summary>
    /// アイテムボックスの数を取得する関数
    /// </summary>
    /// <returns></returns>
    public int GetBreakWallPrefabLength()
    {
        return _wallPrefab.Length;
    }

    /// <summary>
    /// マップのブロックごとの設定
    /// </summary>
    public class MapBlockData
    {
        public int key;             // オブジェクトの属性キー
        public string name;         // オブジェクトの名称
        public bool isWalkable;     // 歩行可能かどうかのフラグ
        public Vector3 tilePosition;// オブジェクトのポジション
        public Vector2Int gridPosition; // オブジェクトの二次元配列の位置を参照できる
        public GameObject instance; // 生成されたオブジェクトの参照
    }
    public int Width => _width;   // ボム用のタイルの横幅をpublic化

    public static MapManager Instance;
    private void Awake()
    {
        Instance = this;
        // ステージ作成
        CreateMap();
    }



    # region マップを生成する関数

    /// <summary>
    /// マップを出現させる
    /// </summary>
    public void CreateMap()
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


                string name = "Unknown";
                GameObject generatePrefab = null;
                bool isWalkable = false;

                // ブロックのカテゴリーとタイプの設定
                int category = key / KEY_SELECT_NUM;
                int type = key % KEY_SELECT_NUM;


                // ブロック生成
                switch (category)
                {
                    // 通路ブロック
                    case 0:
                        position = new Vector3(reversedX * _tileSize + _tileSize / 2f, -0.5f, y * _tileSize + _tileSize / 2f);
                        name = "GroundObject";
                        isWalkable = true;
                        generatePrefab = _groundPrefab[type];
                        CreateMap(generatePrefab, TileParent, x, y, key, name, isWalkable, position);
                        break;



                    // 壁ブロック
                    case 1: 
                        name = $"WallObject";
                        generatePrefab = _wallPrefab[type];
                        CreateMap(generatePrefab, WallParent, x, y, key, name, isWalkable, position);
                        break;



                    // 壊せるブロック
                    case 2: 
                        name = $"BreakWallObject";
                        generatePrefab = _breakWallPrefab[type];
                        CreateMap(generatePrefab, BreakWallParent, x, y, key, name, isWalkable, position);
                        break;



                    // アイテム付きブロック
                    case 3:
                        name = $"ItemWallObject";
                        generatePrefab = _itemBoxPrefab[type];
                        CreateMap(generatePrefab, WallParent, x, y, key, name, isWalkable, position);
                        break;




                    // ガチホコタイル
                    case 7:
                        // ガチホコのポジションを保存
                        GatiHokoPosition(type, position);
                        position = new Vector3(reversedX * _tileSize + _tileSize / 2f, -0.5f, y * _tileSize + _tileSize / 2f);
                        name = "GatiHokoObject";
                        isWalkable = true;
                        generatePrefab = _groundPrefab[0];
                        CreateMap(generatePrefab, GatiHokoTileParent, x, y, key, name, isWalkable, position);
                        break;


                    // ガチエリアタイル
                    case 8:
                        position = new Vector3(reversedX * _tileSize + _tileSize / 2f, -0.5f, y * _tileSize + _tileSize / 2f);
                        name = "GatiAreaObject";
                        isWalkable = true;
                        generatePrefab = _groundPrefab[type];
                        CreateMap(generatePrefab, GatiAreaTileParent, x, y, key, name, isWalkable, position);
                        break;




                    // リスポブロック
                    case 9:
                        name = $"StartObject";
                        generatePrefab = _startTilePrefab[0];
                        if(type == 5)
                        {
                            generatePrefab = _startTilePrefab[1];
                        }
                        // スタートポジションを保存
                        StartPosition(type, position);
                        isWalkable = true;
                        position = new Vector3(reversedX * _tileSize + _tileSize / 2f, -0.5f, y * _tileSize + _tileSize / 2f);
                        CreateMap(generatePrefab, StartTileParent, x, y, key, name, isWalkable, position);
                        break;



                    // それ以外は読み込まない
                    default:
                        break;
                }



            }
        }
    }


    private void CreateMap(GameObject prefaba,Transform parentName, int x, int y, int key, string name, bool isWalkable, Vector3 position)
    {
        // ブロックの生成
        GameObject obj = null;
        obj = Instantiate(prefaba, position, Quaternion.identity, parentName);


        // ブロックの情報を二次元配列に保存
        _mapGrid[x, y] = new MapBlockData
        {
            key = key,
            name = name,
            isWalkable = isWalkable,
            tilePosition = position,
            gridPosition = new Vector2Int(x, y),
            instance = obj
        };
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


    /// <summary>
    /// 壊れる壁から床ブロックに変更する
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void ChangeBlock(int x, int y)
    {
        MapBlockData blockPosition = GetBlockData(x, y);
        Vector3 position = new Vector3(blockPosition.tilePosition.x, -0.5f, blockPosition.tilePosition.z);
        // ブロックの削除
        MapBlockData blockData = _mapGrid[x, y];
        Destroy(blockData.instance);
        blockData.instance = null;


        // 床ブロックの生成
        CreateMap(_groundPrefab[0], WallParent, x, y, 0, "GroundObject", true, position);
    }


    public void StartPosition(int i,Vector3 startPosition)
    {
        _startPosition[i - 1] = startPosition;
    }
    public Vector3 GetStartPosition(int i)
    {
        return _startPosition[i];
    }



    /// <summary>
    /// ガチホコの生成場所を設定
    /// </summary>
    /// <param name="i"></param>
    /// <param name="startPosition"></param>
    public void GatiHokoPosition(int i, Vector3 startPosition)
    {
        _gatiHokoPosition[i - 1] = startPosition;
    }
    /// <summary>
    /// ガチホコの生成場所を返す
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public Vector3 GetGatiHokoPosition(int i)
    {
        return _gatiHokoPosition[i];
    }


    #region マップの状態を確認する関数

    /// <summary>
    /// keyがなんなのかを確認する
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private string GetNameByKey(int key)
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
    private bool IsWalkableTile(int key)
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
