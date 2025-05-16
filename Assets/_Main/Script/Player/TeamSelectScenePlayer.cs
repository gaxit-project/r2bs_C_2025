using UnityEngine;
using UnityEngine.InputSystem;

public class TeamSelectScenePlayer : MonoBehaviour
{
    // �v���C���[�֘A�̕ϐ�
    private float PlayerSpeed = 5f; //�v���C���[�̑��x
    private Vector2 moveInput = Vector2.zero; //���͊i�[
    private Team TeamName;   // �`�[�����̕ۑ�
    private bool isTeamOne = false;
    private int teamLocal = 1; //���W�̌����C���p
    private int playerIndex;
    private PlayerTeamData playerData;



    private void Start()
    {
        playerData = Resources.Load<PlayerTeamData>("PlayerData");
        OnTeamSelect();
    }

    private void Update()
    {
        //�v���C���[�̈ړ�
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

    //�v���C���[�̑ޏo
    public void OnLeft()
    {
        Destroy(this.gameObject);
    }

    public void OnTeamSelect()
    {
        if (isTeamOne)
        {
            playerData.PlayerTable[playerIndex].Team = "TeamTwo";
            this.GetComponent<MeshRenderer>().material.color = Color.red;  //�F�ύX
            isTeamOne = false;
        }
        else
        {
            playerData.PlayerTable[playerIndex].Team = "TeamOne";
            this.GetComponent<MeshRenderer>().material.color = Color.blue; //�F�ύX
            isTeamOne = true;
        }
    }




    //�v���C���[�̈ړ�
    private void PlayerMove()
    {
        this.GetComponent<Rigidbody>().linearVelocity = new Vector3(moveInput.x * PlayerSpeed * teamLocal, 0f, moveInput.y * PlayerSpeed * teamLocal);
    }

   /* //�`�[������
    private void TeamSplit()
    {
        players = GameObject.FindGameObjectsWithTag("Player"); //player�̔z��
        if (this.gameObject.tag == "TeamOne")
        {
            this.transform.position = StartPosition;  //���X�n
 
            BombColor = Color.blue;
            TeamName = Team.TeamOne;
            teamLocal = 1; //���W�̌����C��

        }
        else
        {
            this.transform.position = StartPosition;  //���X�n
            this.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);  //�A���O��
            
            BombColor = Color.red;
            TeamName = Team.TeamTwo;
            teamLocal = -1; //���W�̌����C��
        }

    }*/




}
