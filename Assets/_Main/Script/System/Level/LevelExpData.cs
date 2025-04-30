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
    public int Level;            // ���x���i��F2, 3, 4,...�j
    public int ExpToNextLevel;    // ���̃��x���ɍs���܂łɕK�v�Ȍo���l
}