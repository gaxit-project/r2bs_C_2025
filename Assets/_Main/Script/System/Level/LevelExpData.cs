using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelExpData", menuName = "Scriptable Objects/LevelExpData")]
public class LevelExpData : ScriptableObject
{
    public List<LevelExp> ExpTable;
}

[System.Serializable]
public class LevelExp
{
    public int Level;            // レベル（例：2, 3, 4,...）
    public int ExpToNextLevel;    // 次のレベルに行くまでに必要な経験値
}