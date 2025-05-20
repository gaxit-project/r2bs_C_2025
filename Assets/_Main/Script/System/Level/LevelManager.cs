using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int CurrentLevel = 1;    //初期レベル 1Lv
    public int CurrentExp = 0;      //初期EXP

    [Header("PlayerStatus")]
    [SerializeField] private PlayerStatus _status;

    [Header("JobPattern")]
    [SerializeField] private JobLevelPattern _pattern;

    [Header("LevelData")]
    [SerializeField] private LevelExpData _expData;

    /// <summary>
    /// Expを渡すコード
    /// </summary>
    /// <param name="amount"></param>
    public void AddExp(int amount)
    {
        CurrentExp += amount;
        Debug.Log($"経験値ゲット 現在のExp : {CurrentExp}");
        TryLevelUp();
    }

    /// <summary>
    /// レベル条件を満たしていたら、レベルアップ、そうでなければbreak
    /// </summary>
    private void TryLevelUp()
    {
        while (true)
        {
            //必要経験値取得
            var exp = _expData.ExpTable.Find(e => e.Level == CurrentLevel);

            //レベル条件に満たしていない場合break
            if (exp == null || CurrentExp < exp.ExpToNextLevel) break;

            //レベルアップ
            CurrentExp -= exp.ExpToNextLevel;
            CurrentLevel++;

            Debug.Log($"レベルアップ! 現在のレベル:{CurrentLevel}");

            UpStatus(CurrentLevel);
        }
    }

    /// <summary>
    /// レベルが上がったとき決めていた固定ステータスを上昇させる
    /// </summary>
    /// <param name="level"></param>
    private void UpStatus(int level)
    {
        var growth = _pattern.GrowthTable.Find(g => g.Level == level);
        if (growth != null)
        {
            foreach(var stat in growth.LevelStats)
            {
                _status.Add(stat, 1);
            }
        }
        else
        {
            Debug.Log("正しく成長設定してください");
        }

        //playerに反映

    }
}
