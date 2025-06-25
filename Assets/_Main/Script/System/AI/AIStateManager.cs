using System.Collections.Generic;
using System.Diagnostics;

public sealed class AIStateManager
{
    private readonly List<AIStateBase> _states = new();
    private AIStateBase _current;
    private readonly AIController _owner;

    public AIStateManager(AIController owner)
    {
        _owner = owner;
        var finder = new PathFinder(MapManager.Instance);
        _states.Add(new PaintState(finder));
    }


    public void Update()
    {
        UnityEngine.Debug.Log("StateManager‚®‚é‚®‚é");
        AIContext ctx = AIContextBuilder.Build(_owner);

        AIStateBase next = null;
        float best = float.MaxValue;
        foreach (var s in _states)
            if (s.IsValid(ctx) && s.Priority < best) { best = s.Priority; next = s; }

        _current = next;
        _current?.Execute(ctx);
    }
}
