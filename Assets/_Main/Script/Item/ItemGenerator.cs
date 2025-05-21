using UnityEngine;
using System.Collections.Generic;

public class ItemGenerator : MonoBehaviour
{
    private const int FIXED_SEED = 1; // 乱数のシード値（デバッグ用に固定）
    private const int BASE_EXP = 10; // 経験値の基本値

    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private float _dropRate = 0.7f; // 0.0fから1.0f（確率）
    [SerializeField] private bool _debugSeed = false; // デバックモード用
    [SerializeField] private int _boxCount = 0; // ドロップ対象の総数（ボックスの数）

    [SerializeField] private MapManager _mapManager; //アイテムボックスの数を知るため

    private bool[] _dropFlags; // 出現判定の配列
    private int _currentDropIndex = 0; // TryDropExpが呼ばれた回数をカウント



    public static ItemGenerator Instance;
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

        // 確率に基づいて、アイテムが出現する配列を初期化する
        _boxCount = _mapManager.GetBreakWallPrefabLength();
        int itemCount = Mathf.RoundToInt(_boxCount * _dropRate);
        _dropFlags = GenerateDropFlags(_boxCount, itemCount);
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
        if (_dropFlags[_currentDropIndex])
        {
            Instantiate(_itemPrefab, position, Quaternion.identity);
        }

        _currentDropIndex++; // 呼び出しインデックスを進める
    }

    /// <summary>
    /// 敵を倒したときにアイテムを確定で生成する
    /// （後に経験値計算などに拡張可能な設計）
    /// </summary>
    public void DropExp(Vector3 position, int enemyLevel, int playerLevel)
    {
        // 拡張性を持たせた設計（仮の経験値計算）
        int exp = CalcExp(enemyLevel, playerLevel);
        Debug.Log("経験値獲得: " + exp);

        // positionを受け取って、その場に生成する（同じアイテムを生成するかは未定）
        Instantiate(_itemPrefab, position, Quaternion.identity);
    }

    /// <summary>
    /// レベル差に応じて経験値を計算する（今後拡張可能）
    /// </summary>
    private int CalcExp(int enemyLevel, int playerLevel)
    {
        int baseExp = BASE_EXP;
        int levelDifference = enemyLevel - playerLevel;
        float multiplier = 1.0f + (levelDifference * 0.1f); // 仮計算用
        return Mathf.Max(1, Mathf.RoundToInt(baseExp * multiplier)); // 1とレベル差によって出された値のうち、大きい方（整数）を返す
    }

    /// <summary>
    /// 固定個数のtrueをランダムな位置に配置するbool配列を生成
    /// 出現場所をランダムにして、個数は固定する
    /// </summary>
    private bool[] GenerateDropFlags(int totalBoxes, int totalItems)
    {
        List<bool> list = new List<bool>();

        // true（アイテムあり）とfalse（なし）を指定数追加
        for (int i = 0; i < totalItems; i++) list.Add(true);
        for (int i = totalItems; i < totalBoxes; i++) list.Add(false);

        // Fisher–Yatesアルゴリズム風のシャッフル
        for (int i = 0; i < list.Count; i++)
        {
            int randIndex = Random.Range(i, list.Count);
            bool temp = list[i];
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
