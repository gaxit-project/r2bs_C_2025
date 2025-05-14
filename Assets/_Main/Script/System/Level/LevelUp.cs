using System.Collections.Generic;
using UnityEngine;

//能力の種類(追加可能)
public enum StatusType
{
    Speed,
    Power,
    BombCount
}

/// <summary>
/// レベルアップのステータス
/// </summary>
[System.Serializable]
public class LevelUp
{
    public int Level;
    public List<StatusType> LevelStats;
}
