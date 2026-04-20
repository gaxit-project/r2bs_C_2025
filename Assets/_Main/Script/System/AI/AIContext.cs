using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI が意思決定に用いる共有データ
/// </summary>
public sealed class AIContext
{
    public AIController Owner { get; set; }   //AIプレイヤー
    public Vector2Int SelfPos { get; set; } //現在地点
    public Team SelfTeam { get; set; }      //所属チーム名  
    public IReadOnlyList<Vector2Int> UnpaintedTiles { get; set; }   //塗られていない場所
    public int BombRange { get; set; }                              //ボム射程  
    public bool isDead { get; set; }                                //死亡しているか
}
