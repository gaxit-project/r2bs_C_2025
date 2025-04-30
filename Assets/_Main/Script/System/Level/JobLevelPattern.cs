using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JobLevelPattern", menuName = "Scriptable Objects/JobLevelPattern")]
public class JobLevelPattern : ScriptableObject
{
    public string JobName;
    public List<LevelUp> GrowthTable;
}
