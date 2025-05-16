using UnityEngine;
using UnityEngine.InputSystem;

public class TeamSelectScenePlayer : MonoBehaviour
{
    // プレイヤー関連の変数
    private float PlayerSpeed = 5f; //プレイヤーの速度
    private Vector2 moveInput = Vector2.zero; //入力格納
    private Team TeamName;   // チーム名の保存
    private bool isTeamOne = false;
    private int teamLocal = 1; //座標の向き修正用
    private int playerIndex;
    private PlayerTeamData playerData;



    private void Start()
    {
        playerData = Resources.Load<PlayerTeamData>("PlayerData");
        OnTeamSelect();
    }

    private void Update()
    {
        //プレイヤーの移動
        PlayerMove();
    }

    public void GetPlayerIndex(PlayerInput playerInput)
    {
        playerIndex = playerInput.user.index;
        playerData.PlayerTable.Add(null);
        playerData.PlayerTable[playerIndex].playerInput = playerInput;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    //プレイヤーの退出
    public void OnLeft()
    {
        Destroy(this.gameObject);
    }

    public void OnTeamSelect()
    {
        if (isTeamOne)
        {
            playerData.PlayerTable[playerIndex].Team = "TeamTwo";
            this.GetComponent<MeshRenderer>().material.color = Color.red;  //色変更
            isTeamOne = false;
        }
        else
        {
            playerData.PlayerTable[playerIndex].Team = "TeamOne";
            this.GetComponent<MeshRenderer>().material.color = Color.blue; //色変更
            isTeamOne = true;
        }
    }




    //プレイヤーの移動
    private void PlayerMove()
    {
        this.GetComponent<Rigidbody>().linearVelocity = new Vector3(moveInput.x * PlayerSpeed * teamLocal, 0f, moveInput.y * PlayerSpeed * teamLocal);
    }

   /* //チーム分け
    private void TeamSplit()
    {
        players = GameObject.FindGameObjectsWithTag("Player"); //playerの配列
        if (this.gameObject.tag == "TeamOne")
        {
            this.transform.position = StartPosition;  //リス地
 
            BombColor = Color.blue;
            TeamName = Team.TeamOne;
            teamLocal = 1; //座標の向き修正

        }
        else
        {
            this.transform.position = StartPosition;  //リス地
            this.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);  //アングル
            
            BombColor = Color.red;
            TeamName = Team.TeamTwo;
            teamLocal = -1; //座標の向き修正
        }

    }*/




}
