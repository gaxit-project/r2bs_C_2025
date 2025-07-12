using UnityEngine;

/// <summary>
/// AIContext を構築するユーティリティ
/// </summary>
public static class AIContextBuilder
{
    public static AIContext Build(AIController owner)
    {
        if (MapManager.Instance == null || !MapManager.Instance.IsReady)
            return null;

        Vector2Int selfPos = MapManager.Instance.GetBlockData((int)owner.transform.position.x, (int)owner.transform.position.z).gridPosition;

        //塗られてないとこ取得
        var unpaintedTiles = MapManager.Instance.GetUnpaintedTiles(owner.CurrentTeamName);

        var isDead = PlayerBase.PlayerState.Death == owner.GetComponent<PlayerBase>().currentState;

        return new AIContext
        {
            Owner = owner,
            SelfPos = selfPos,
            SelfTeam = owner.CurrentTeamName,
            UnpaintedTiles = unpaintedTiles,
            BombRange = 1, // 暫定
            isDead = isDead,
        };
    }
}
