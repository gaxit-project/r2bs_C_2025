using System.Collections.Generic;
using UnityEngine;

public sealed class PaintState : AIStateBase
{
    public override float Priority => 5f;

    private readonly PathFinder _finder;
    private readonly List<Vector2> _path = new();
    private Vector2Int _target;

    public PaintState(PathFinder finder) => _finder = finder;

    public override bool IsValid(AIContext ctx) => ctx.UnpaintedTiles.Count > 0;

    public override void Execute(AIContext ctx)
    {
        Debug.Log("‚Ø‚¢‚ñ‚Æ’†");
        // ƒpƒX‚ª–³‚¯‚ê‚ÎV‚µ‚­ì‚é
        if (_path.Count == 0)
        {
            _target = GetNearest(ctx.UnpaintedTiles, ctx.SelfPos);
            var p = _finder.FindPath(ctx.SelfPos, _target);
            if (p != null)
            {

                _path.Clear();
                foreach (var pathpoint in p)
                {
                    _path.Add(new Vector3(pathpoint.x, pathpoint.y));
                }
            }
            Debug.Log(_target);
        }

        // Œo˜H‚ği‚Ş
        if (_path.Count > 0)
        {
            Vector2 next = _path[0];
            Vector2 dir = new (Mathf.Sign(next.x - ctx.SelfPos.x),Mathf.Sign(next.y - ctx.SelfPos.y));
            ctx.Owner.SetMoveInput(dir);

            if (ctx.SelfPos == next) _path.RemoveAt(0);
        }
        else
        {
            Debug.Log("”š’eİ’u‚µ‚Ä‚¢‚¢‚æ‚P‚P‚P‚P‚P");
            ctx.Owner.SetMoveInput(Vector2.zero);
            ctx.Owner.SetBombRequest(true);       // “’B‚µ‚½‚ç”š’e
        }
    }

    // PaintState.cs “à‚Ìƒƒ\ƒbƒh‚ğC³
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
