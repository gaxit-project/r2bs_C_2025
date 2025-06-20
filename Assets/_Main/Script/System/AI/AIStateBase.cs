/// <summary>
/// すべての AI ステートの共通基底クラス
/// </summary>
public abstract class AIStateBase
{
    //小さいほど高優先度
    public abstract float Priority { get; }


    //情報から動作可能か
    public abstract bool IsValid(AIContext context);

    ///実行A
    public abstract void Execute(AIContext context);
}
