using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "ResultData", menuName = "Scriptable Objects/ResultData")]

public class ResultData : ScriptableObject
{
      // 1つ目のチームの咲き誇り数
      // 2つ目のチームの咲き誇り数
    public int blueTile;
    public int redTile;

    public int blueArea;
    public int redArea;

}


