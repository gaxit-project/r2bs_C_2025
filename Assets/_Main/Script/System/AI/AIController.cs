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
        //--------------------------------------------------
        // ① 爆弾プレハブ・生成先を設定（PlayerController と同等）
        //--------------------------------------------------
        StandardBomb = Resources.Load<GameObject>("Prefab/StandardBomb");
        BombParent = GameObject.Find("BombGenerate").transform;

        //--------------------------------------------------
        // ② チーム分け（Hierarchy 上の Tag を参照）
        //--------------------------------------------------
        TeamSplit();            // Tag が “TeamOne” か “TeamTwo” で呼ばれる

        //--------------------------------------------------
        // ③ 特殊ステータス初期化
        //--------------------------------------------------
        InitSpecialStatus();

        //--------------------------------------------------
        // ④ ステートマシン生成（PaintState だけ登録）
        //--------------------------------------------------
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
