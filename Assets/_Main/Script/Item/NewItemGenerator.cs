using UnityEngine;
using System.Collections.Generic;

public class NewItemGenerator : MonoBehaviour
{
    private const int FIXED_SEED = 1; // 乱数のシード値（デバッグ用に固定）
    private const int BASE_EXP = 10; // 経験値の基本値

    [SerializeField] private GameObject _speedItemPrefab;
    [SerializeField] private GameObject _rangeItemPrefab;
    [SerializeField] private GameObject _countItemPrefab;

    [SerializeField] private int speedItemCnt = 16;
    [SerializeField] private int rangeItemCnt = 14;
    [SerializeField] private int countItemCnt = 10;


    //[SerializeField] private float _dropRate = 0.7f; // 0.0fから1.0f（確率）

    [SerializeField] private bool _debugSeed = false; // デバックモード用
    [SerializeField] private int _boxCount = 0; // ドロップ対象の総数（ボックスの数）

    [SerializeField] private MapManager _mapManager; //アイテムボックスの数を知るため

    public int[] _dropFlags; // 出現判定の配列
    private int _currentDropIndex = 0; // TryDropExpが呼ばれた回数をカウント



    public static NewItemGenerator Instance;
    /// <summary>
    /// デバックモードの時はシード値を固定する
    /// </summary>
    private void Awake()
    {
        Instance = this;
        if (_debugSeed)
        {
            Random.InitState(FIXED_SEED);
        }

        
    }

    private void Start()
    {
        // 確率に基づいて、アイテムが出現する配列を初期化する
        _boxCount = _mapManager.GetBreakWallPrefabLength();
        _dropFlags = GenerateDropFlags(_boxCount, speedItemCnt, rangeItemCnt, countItemCnt);
    }

    /// <summary>
    /// ブロックを壊したときにアイテムをランダムで生成する
    /// 出現数は固定し、位置だけがランダムになる
    /// </summary>
    public void TryDropExp(Vector3 position)
    {
        

        // インデックスが配列サイズを超えた場合は無視（安全策）
        if (_dropFlags == null || _currentDropIndex >= _dropFlags.Length)
        {
            Debug.LogWarning("ドロップ配列の範囲を超えました");
            Debug.Log(_currentDropIndex);
            return;
        }

        // ドロップ配列に基づいて出現判定
        switch (_dropFlags[_currentDropIndex])
        {
            case 1:
                Instantiate(_speedItemPrefab, position + new Vector3(0, 0.7f, 0), Quaternion.Euler(0, 228, 0));
                break;
            case 2:
                Instantiate(_rangeItemPrefab, position + new Vector3(0, 0.7f, 0), Quaternion.Euler(0, 228, 0));
                break;
            case 3:
                Instantiate(_countItemPrefab, position + new Vector3(0, 0.7f, 0), Quaternion.Euler(0, 228, 0));
                break;
            default:
                break;
        }

        _currentDropIndex++; // 呼び出しインデックスを進める
    }

    /// <summary>
    /// 敵を倒したときにアイテムを確定で生成する
    /// （後に経験値計算などに拡張可能な設計）
    /// </summary>
    public void DropExp(Vector3 position, int playerLevel)
    {
        // （仮の経験値計算）
        int exp = playerLevel / 5;
        Debug.Log("経験値獲得: " + exp);
        Debug.Log("経験値をここに生成"+position);
        // positionを受け取って、その場に生成する（同じアイテムを生成するかは未定）

        for (int i = 0; i < exp; i++)
        {
            int rm = Random.Range(1, 4);
            switch (rm)
            {
                case 1:
                    Instantiate(_speedItemPrefab, position + new Vector3(0, 0.7f, 0), Quaternion.Euler(0, 228, 0));
                    break;
                case 2:
                    Instantiate(_rangeItemPrefab, position + new Vector3(0, 0.7f, 0), Quaternion.Euler(0, 228, 0));
                    break;
                case 3:
                    Instantiate(_countItemPrefab, position + new Vector3(0, 0.7f, 0), Quaternion.Euler(0, 228, 0));
                    break;
            }
        }
    }

    /// <summary>
    /// 固定個数のtrueをランダムな位置に配置するbool配列を生成
    /// 出現場所をランダムにして、個数は固定する
    /// </summary>
    private int[] GenerateDropFlags(int totalBoxes, int speedItems, int rangeItems, int countItems)
    {
        List<int> list = new List<int>();
        // true（アイテムあり）とfalse（なし）を指定数追加
        for (int i = 0; i < speedItems; i++) list.Add(1);
        for (int i = 0; i < rangeItems; i++) list.Add(2);
        for (int i = 0; i < countItems; i++) list.Add(3);
        for (int i = list.Count; i < totalBoxes; i++) list.Add(0);

        // Fisher–Yatesアルゴリズム風のシャッフル
        for (int i = 0; i < list.Count; i++)
        {
            int randIndex = Random.Range(i, list.Count);
            int temp = list[i];
            list[i] = list[randIndex];
            list[randIndex] = temp;
        }
        for (int i = 0; i < list.Count; i++)
        {
            int randIndex = Random.Range(i, list.Count);
            int temp = list[i];
            list[i] = list[randIndex];
            list[randIndex] = temp;
        }

        return list.ToArray();
    }

    // デバック用
    /*private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            TryDropExp(new Vector3(0, 0, 0));
        }
    }*/
}
