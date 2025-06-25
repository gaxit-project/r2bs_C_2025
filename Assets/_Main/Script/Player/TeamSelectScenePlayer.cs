using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static PlayerBase;

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
    private bool _isTeamOne = true;

    /// <summary>チームによる座標修正用の係数</summary>
    private int _teamLocal = 1;

    /// <summary>プレイヤーのインデックス（保存用）</summary>
    private int _playerIndex;

    /// <summary>プレイヤーデータ格納ScriptableObject</summary>
    private PlayerTeamData _playerData;

    private float rotateSpeed = 8f; //playerのアングルspeed

    [SerializeField]
    private GameObject purpleChan;
    [SerializeField]
    private GameObject cyanChan;


    [SerializeField] public GameObject selectUI;
    [SerializeField] public Image teamUI;
    [SerializeField] public RawImage playerPinkUI;
    [SerializeField] public RawImage playerBlueUI;
    [SerializeField] public RawImage backPinkUI;
    [SerializeField] public RawImage backBlueUI;
    [SerializeField] public RawImage ReadyUI;
    [SerializeField] public Image NotFoundUI;
    [SerializeField] public TextMeshProUGUI nPText;





    public enum PlayerState
    {
        Alive,
        Death
    }
    public PlayerState currentState;


    public static TeamSelectScenePlayer Instance;
    /// <summary>
    /// 初期化処理（インデックス登録とデータの読み込み）
    /// </summary>
    private void Awake()
    {
        Instance = this;
        _playerData = Resources.Load<PlayerTeamData>("PlayerData");
        _playerIndex = _playerData.PlayerTable.Count-1;
        // UI取得
        selectUI = GameObject.Find("char" + _playerIndex);
        ReadyUI = selectUI.transform.Find("Ready" + _playerIndex).GetComponent<RawImage>();
        NotFoundUI = selectUI.transform.Find("NotFound" + _playerIndex).GetComponent<Image>();
        teamUI = selectUI.transform.Find("teamUI" + _playerIndex).GetComponent<Image>();
        Transform playerUI = selectUI.transform.Find("player" + _playerIndex);
        playerPinkUI = playerUI.Find("pink" + _playerIndex).GetComponent<RawImage>();
        playerBlueUI = playerUI.Find("blue" + _playerIndex).GetComponent<RawImage>();
        Transform backImageUI = selectUI.transform.Find("backImage" + _playerIndex);
        backPinkUI = backImageUI.Find("backPink" + _playerIndex).GetComponent<RawImage>();
        backBlueUI = backImageUI.Find("backBlue" + _playerIndex).GetComponent<RawImage>();
        nPText = selectUI.transform.Find("nP" + _playerIndex).GetComponent<TextMeshProUGUI>();
        nPText.text = _playerIndex + 1 + "P";
        playerPinkUI.gameObject.SetActive(true);
        playerBlueUI.gameObject.SetActive(false);
        teamUI.color = new Color32(255, 105, 180, 255);
        TeamSelectReady.Instance.GameObjectSetting(this.gameObject);
        // readyのリセット
        TeamSelectReady.Instance.ResetFlag(_playerIndex);
        //for(int i = 0; i < _playerIndex + 1; i++)
        //{
        //    ReadyUI.gameObject.SetActive(false);
        //}
        if (!TeamSelectReady.Instance.GetCurrentReady(_playerIndex))
        {
            if (_isTeamOne)
            {
                _playerData.PlayerTable[_playerIndex].Team = "TeamTwo";
                //purpleChan.SetActive(true);
                //cyanChan.SetActive(false);//チーム変更
                ChangeUI(_isTeamOne);
                _isTeamOne = false;
            }
            else
            {
                _playerData.PlayerTable[_playerIndex].Team = "TeamOne";
                //purpleChan.SetActive(false);
                //cyanChan.SetActive(true);//チーム変更
                ChangeUI(_isTeamOne);
                _isTeamOne = true;
            }
        }
        NotFoundUI.gameObject.SetActive(false);
    }

    /// <summary>
    /// 毎フレーム移動処理を実行
    /// </summary>
    private void Update()
    {
        MovePlayer();
        if (Input.GetKeyDown(KeyCode.H))
        {
            Cancel();
        }
    }

    public void ResetReadyUI()
    {
        ReadyUI.gameObject.SetActive(false);
    }

    public void Ready()
    {
        TeamSelectReady.Instance.ReadyFlag(_playerIndex);
        ReadyUI.gameObject.SetActive(true);
    }


    public void Cancel()
    {
        TeamSelectReady.Instance.CancelFlag(_playerIndex);
        ReadyUI.gameObject.SetActive(false);
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

    public void OnPose(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        MainGameManager.instance.OnSwithPosw();
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
        if (!TeamSelectReady.Instance.GetCurrentReady(_playerIndex))
        {
            if (_isTeamOne)
            {
                _playerData.PlayerTable[_playerIndex].Team = "TeamTwo";
                //purpleChan.SetActive(true);
                //cyanChan.SetActive(false);//チーム変更
                ChangeUI(_isTeamOne);
                _isTeamOne = false;
            }
            else
            {
                _playerData.PlayerTable[_playerIndex].Team = "TeamOne";
                //purpleChan.SetActive(false);
                //cyanChan.SetActive(true);//チーム変更
                ChangeUI(_isTeamOne);
                _isTeamOne = true;
            }
        }
#if !UNITY_EDITOR
    // ビルド環境なら変更後に保存
    PlayerDataIO.Save(_playerData.PlayerTable);
#endif
    }


    private void ChangeUI(bool teamOne)
    {
        if(teamOne)
        {
            playerBlueUI.gameObject.SetActive(false);
            playerPinkUI.gameObject.SetActive(true);
            backBlueUI.gameObject.SetActive(false);
            backPinkUI.gameObject.SetActive(true);
            //teamUI.color = new Color32(255, 105, 180, 255);
        }
        else
        {
            playerBlueUI.gameObject.SetActive(true);
            playerPinkUI.gameObject.SetActive(false);
            backBlueUI.gameObject.SetActive(true);
            backPinkUI.gameObject.SetActive(false);
            //teamUI.color = new Color32(135, 206, 250, 255);
        }
    }





    /// <summary>
    /// プレイヤーを入力に応じて移動させる
    /// </summary>
    private void MovePlayer()
    {
        Vector3 moveValue = new Vector3(_moveInput.x * _playerSpeed * _teamLocal, 0f, _moveInput.y * _playerSpeed * _teamLocal);
        if (currentState == PlayerState.Alive)
        {
            this.GetComponent<Rigidbody>().linearVelocity = moveValue;
            transform.forward = Vector3.Slerp(transform.forward, moveValue, Time.deltaTime * rotateSpeed); //angle変更
        }
        else
        {
            this.GetComponent<Rigidbody>().linearVelocity = new Vector3(0, 0, 0);
            transform.forward = Vector3.Slerp(transform.forward, moveValue, 0);
        }
    }
}
