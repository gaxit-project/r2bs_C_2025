using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// チーム選択シーンにおけるプレイヤーの操作と状態管理クラス
/// </summary>
public class TeamSelectScenePlayer : MonoBehaviour
{
    /// <summary>プレイヤーの移動速度</summary>
    private float _playerSpeed = 5f;

    /// <summary>移動入力値</summary>
    private Vector2 _moveInput = Vector2.zero;

    /// <summary>現在のチーム（未使用変数）</summary>
    private Team _teamName;

    /// <summary>現在TeamOneに所属しているか</summary>
    private bool _isTeamOne = false;

    /// <summary>チームによる座標修正用の係数</summary>
    private int _teamLocal = 1;

    /// <summary>プレイヤーのインデックス（保存用）</summary>
    private int _playerIndex;

    /// <summary>プレイヤーデータ格納ScriptableObject</summary>
    private PlayerTeamData _playerData;

    /// <summary>
    /// 初期化処理（インデックス登録とデータの読み込み）
    /// </summary>
    private void Start()
    {
        _playerData = Resources.Load<PlayerTeamData>("PlayerData");
        _playerIndex = TeamSelectManager.PlayerSum;
        TeamSelectManager.PlayerSum++;
    }

    /// <summary>
    /// 毎フレーム移動処理を実行
    /// </summary>
    private void Update()
    {
        MovePlayer();
    }

    /// <summary>
    /// 入力イベントで移動方向を更新
    /// </summary>
    /// <param name="context">移動入力</param>
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// プレイヤーが退出した際の処理
    /// </summary>
    public void OnLeft()
    {
        Destroy(this.gameObject);
    }

    /// <summary>
    /// チーム選択時の処理（チームの切り替えと色変更）
    /// </summary>
    /// <param name="context">入力コンテキスト</param>
    public void OnTeamSelect(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        if (_isTeamOne)
        {
            _playerData.PlayerTable[_playerIndex].Team = "TeamTwo";
            GetComponent<MeshRenderer>().material.color = Color.red;  // 赤に変更
            _isTeamOne = false;
        }
        else
        {
            _playerData.PlayerTable[_playerIndex].Team = "TeamOne";
            GetComponent<MeshRenderer>().material.color = Color.blue; // 青に変更
            _isTeamOne = true;
        }
    }

    /// <summary>
    /// プレイヤーを入力に応じて移動させる
    /// </summary>
    private void MovePlayer()
    {
        var rb = GetComponent<Rigidbody>();
        rb.linearVelocity = new Vector3(_moveInput.x * _playerSpeed * _teamLocal, 0f, _moveInput.y * _playerSpeed * _teamLocal);
    }
}
