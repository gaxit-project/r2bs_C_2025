using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// AI 専用コントローラ。<br/>
/// PlayerBase の共通挙動（移動・爆弾・ステータス）を再利用しつつ、
/// AIStateManager で意思決定を行う。
/// </summary>
public sealed class AIController : PlayerBase
{
    private AIStateManager _stateManager;   // ステートマシン

    private void Awake()
    {
        StandardBomb = Resources.Load<GameObject>("Prefab/StandardBomb");
        GameObject bombParentObj = GameObject.Find("BombGenerate");
        BombParent = bombParentObj.transform;
        TeamSplit();            // Tag が “TeamOne” か “TeamTwo” で呼ばれる
        InitSpecialStatus();
    }

    protected override void Start()
    {
        base.Start();
        _stateManager = new AIStateManager(this);
    }
    /// <summary>
    /// 毎フレーム：AI 思考 → 共通移動処理の順に実行。
    /// </summary>
    protected override void Update()
    {
        _stateManager.Update(); // AIStateMachine による行動決定
        base.Update();          // PlayerMove() など共通処理
    }

    // AI が移動方向を注入する
    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;                    // PlayerBase の protected 変数
    }

    // AI が爆弾を置きたいときに呼ぶ
    public void SetBombRequest(bool request)
    {
        if (!request) return;

        var block = CatchPlayerPos();         // PlayerBase の protected メソッド
        if (block != null) BombPlacement(block);
    }

}
