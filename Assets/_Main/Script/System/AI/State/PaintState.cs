using System.Collections.Generic;
using UnityEngine;

public sealed class PaintState : AIStateBase
{
    public override float Priority => 5f;

    private readonly PathFinder _finder;
    private readonly List<Vector2Int> _path = new();
    private Vector2Int _target;

    public PaintState(PathFinder finder) => _finder = finder;

    public override bool IsValid(AIContext ctx) => ctx.UnpaintedTiles.Count > 0;

    public override void Execute(AIContext ctx)
    {
        // パスが無ければ新しく作る
        if (_path.Count == 0)
        {
            _target = GetNearest(ctx.UnpaintedTiles, ctx.SelfPos);
            var p = _finder.FindPath(ctx.SelfPos, _target);
            if (p != null) _path.AddRange(p);
        }

        // 経路を進む
        if (_path.Count > 0)
        {
            Vector2Int next = _path[0];
            Vector2 dir = new (Mathf.Sign(next.x - ctx.SelfPos.x),Mathf.Sign(next.y - ctx.SelfPos.y));
            ctx.Owner.SetMoveInput(dir);

            if (ctx.SelfPos == next) _path.RemoveAt(0);
        }
        else
        {
            ctx.Owner.SetMoveInput(Vector2.zero);
            ctx.Owner.SetBombRequest(true);       // 到達したら爆弾
        }
    }

    // PaintState.cs 内のメソッドを修正
    private static Vector2Int GetNearest(IReadOnlyList<Vector2Int> list, Vector2Int from)
    {
        Vector2Int best = list[0];
        int bestDist = Mathf.Abs(best.x - from.x) + Mathf.Abs(best.y - from.y);

        for (int i = 1; i < list.Count; i++)
        {
            var v = list[i];
            int d = Mathf.Abs(v.x - from.x) + Mathf.Abs(v.y - from.y);
            if (d < bestDist) { best = v; bestDist = d; }
        }
        return best;
    }

}
