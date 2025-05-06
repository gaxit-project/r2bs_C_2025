using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private float _dropRate = 0.7f; // 0.0f?1.0f（確率）
    [SerializeField] private bool _useFixedSeed = false; //デバックモード用
    [SerializeField] private int _fixedSeed = 1; //乱数のシード値

    private static bool _isSeedInItialized = false; //シード値が初期化されたかどうか


    /// <summary>
    /// ブロックを壊したときにアイテムをランダムで生成する
    /// </summary>
    public void TryDropItemFromBlock(Vector3 position)
    {
        InitSeedIfNeeded();
        Debug.Log("乱数は" + Random.value);

        if (Random.value <= _dropRate)
        {

            //positionを受け取って，その場に生成する
            //Instantiate(_itemPrefab, position, Quaternion.identity);
        }

    }

    /// <summary>
    /// デバックモードの時はシード値を固定する
    /// </summary>
    private void InitSeedIfNeeded()
    {
        if (_useFixedSeed && !_isSeedInItialized)
        {
            Random.InitState(_fixedSeed);
            _isSeedInItialized = true;
        }
    }

    /// <summary>
    /// 敵を倒したときにアイテムを確定で生成する
    /// （後に経験値計算などに拡張可能な設計）
    /// </summary>
    public void DropItemFromEnemy(Vector3 position, int enemyLevel, int playerLevel)
    {
        // 拡張性を持たせた設計（仮の経験値計算）
        int exp = CalculateExperience(enemyLevel, playerLevel);
        Debug.Log("経験値獲得: " + exp);

        //positionを受け取って，その場に生成する(同じアイテムを生成するかは未定)
        Instantiate(_itemPrefab, position, Quaternion.identity);
    }

    /// <summary>
    /// レベル差に応じて経験値を計算する（今後拡張可能）
    /// </summary>
    private int CalculateExperience(int enemyLevel, int playerLevel)
    {
        int baseExp = 10;
        int levelDifference = enemyLevel - playerLevel;
        float multiplier = 1.0f + (levelDifference * 0.1f);
        return Mathf.Max(1, Mathf.RoundToInt(baseExp * multiplier)); //1とレベル差によって出された値のうち，大きい方(整数)を返す
    }


    /*private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            TryDropItemFromBlock(new Vector3(0, 0, 0));
        }
    }*/
}
