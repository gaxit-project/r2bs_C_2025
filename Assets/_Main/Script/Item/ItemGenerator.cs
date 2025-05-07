using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    private const int FIXED_SEED = 1; //乱数のシード値
    private const int BASE_EXP = 10;

    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private float _dropRate = 0.7f; // 0.0fから1.0f（確率）
    [SerializeField] private bool _debugSeed = false; //デバックモード用

    /// <summary>
    /// デバックモードの時はシード値を固定する
    /// </summary>
    private void Awake()
    {
        if (_debugSeed)
        {
            Random.InitState(FIXED_SEED);
        }
    }
    /// <summary>
    /// ブロックを壊したときにアイテムをランダムで生成する
    /// </summary>
    public void TryDropExp(Vector3 position)
    {
        Debug.Log("乱数は" + Random.value);

        if (Random.value <= _dropRate)
        {

            //positionを受け取って，その場に生成する
            //Instantiate(_itemPrefab, position, Quaternion.identity);
        }

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

        //positionを受け取って，その場に生成する(同じアイテムを生成するかは未定)
        Instantiate(_itemPrefab, position, Quaternion.identity);
    }

    /// <summary>
    /// レベル差に応じて経験値を計算する（今後拡張可能）
    /// </summary>
    private int CalcExp(int enemyLevel, int playerLevel)
    {
        int baseExp = BASE_EXP;
        int levelDifference = enemyLevel - playerLevel;
        float multiplier = 1.0f + (levelDifference * 0.1f); //仮計算用
        return Mathf.Max(1, Mathf.RoundToInt(baseExp * multiplier)); //1とレベル差によって出された値のうち，大きい方(整数)を返す
    }


    //デバック用
    /*private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            TryDropExp(new Vector3(0, 0, 0));
        }
    }*/
}
