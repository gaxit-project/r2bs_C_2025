using UnityEngine;

/// <summary>
/// AIContext を構築するユーティリティ
/// </summary>
public static class AIContextBuilder
{
    public static AIContext Build(AIController owner)
    {
        Vector2Int selfPos = Vector2Int.RoundToInt(owner.transform.position);

        //塗られてないとこ取得
        var unpaintedTiles = MapManager.Instance.GetUnpaintedTiles(owner.CurrentTeamName);

        return new AIContext
        {
            Owner = owner,
            SelfPos = selfPos,
            SelfTeam = owner.CurrentTeamName,
            UnpaintedTiles = unpaintedTiles,
            BombRange = 1 // 暫定
        };
    }
}
