using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;
using static MapManager;

public class PlayerBase : MonoBehaviour
{
    // �v���C���[�֘A�̕ϐ�
    protected const int EXP_SIZE = 1;    //Exp1������̌o���l�� 
    protected LevelManager _levelManager;    //���x���A�b�v�Ǘ�
    protected PlayerStatus _status;  //���x���A�b�v�f�[�^

    [SerializeField]
    protected float PlayerSpeed = 5f; //�v���C���[�̑��x
    protected Vector2 moveInput = Vector2.zero; //���͊i�[
    protected Team TeamName;   // �`�[�����̕ۑ�
    protected Vector3 StartPosition;
    protected int teamLocal; //player�̃A���O������

    protected int SpecialBombCnt = 0;
    protected int SpecialBombRange = 0;
    protected float SpecialPlayerSpeed = 1f;

    protected static int playerIndex = -1;
    protected static int teamOneIndex = -1;
    protected static int teamTwoIndex = 1;

    // �v���C���[�̏�Ԃ��Ǘ����� (0: ����, 1: ���S)
    public enum PlayerState
    {
        Alive,
        Death
    }
    public PlayerState currentState;
    protected PlayerTeamData playerData;
    // ���e�֘A�̕ϐ�
    [SerializeField] protected GameObject StandardBomb;  // ���e������z��
    protected Transform BombParent;                  // ���e�̐�����I�u�W�F�N�g
    [SerializeField]
    protected int BombRange = 5; // �{���̔����͈�
    protected int BombCnt = 1;   // �{���̏����� 
    protected Color BombColor = Color.black; // ���e�̐F�̐ݒ�
    [SerializeField]
    protected int BloomBombMax = 5;          // ���e�̏������̃}�b�N�X�̐ݒ�
    protected List<GameObject> BloomBombPool = new(); // �{�������郊�X�g


    protected void Start()
    {
        _status = GetComponent<PlayerStatus>();
        _levelManager = GetComponent<LevelManager>();
        SetStatus();
    }

    protected void Update()
    {
        //�v���C���[�̈ړ�
        PlayerMove();
    }

    /// <summary>
    /// ���x���̕ύX����
    /// </summary>

    public void SetStatus()
    {
        SetStatusInternal();
    }

    protected virtual void SetStatusInternal()
    {
        if (_status == null) return;
        PlayerSpeed = 2.0f + (_status.GetValue(StatusType.Speed) - 1) * 0.5f;
        BombRange = 1 + (_status.GetValue(StatusType.Power) - 1);
        BloomBombMax = 1 + (_status.GetValue(StatusType.BombCount) - 1);
        InitializePool();
    }



    /// <summary>
    /// �v���C���[�̌��݂���}�b�v�^�C���̏����擾
    /// </summary>
    protected MapBlockData CatchPlayerPos()
    {
        MapBlockData blockData = null;  // �f�[�^�ۑ��p�ϐ�
        RaycastHit hit;                 // ���C�̕ϐ�

        // �^���Ƀ��C���΂�
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out hit, 10.0f))
        {
            // ���[���h���W����O���b�h���W�ɕϊ�
            Vector3 hitPosition = hit.point;
            int x = Mathf.FloorToInt(hitPosition.x / 1f);
            int y = Mathf.FloorToInt(hitPosition.z / 1f);
            int reversedX = (MapManager.Instance.Width - 1) - x;


            // �f�[�^�擾
            blockData = MapManager.Instance.GetBlockData(reversedX, y);
        }

        return blockData;
    }


    //�v���C���[�̈ړ�����
    public void OnMove(InputAction.CallbackContext context)
    {
        if(currentState == PlayerState.Alive)
        {
            moveInput = context.ReadValue<Vector2>();
        }
    }



    //���e�ݒu
    public void OnBomb(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        BombPlacement(CatchPlayerPos());
    }




    //�v���C���[�̑ޏo
    public void OnLeft()
    {
        Destroy(this.gameObject);
    }




    //�v���C���[�̈ړ�
    protected void PlayerMove()
    {
        this.GetComponent<Rigidbody>().linearVelocity = new Vector3(moveInput.x * PlayerSpeed * SpecialPlayerSpeed * teamLocal, 0f, moveInput.y * PlayerSpeed * SpecialPlayerSpeed * teamLocal);
    }

    //�`�[������
    protected void TeamSplit()
    {
        if (this.gameObject.tag == "TeamOne")
        {
            teamOneIndex++;
            StartPosition = MapManager.Instance.GetStartPosition(teamOneIndex);

            this.transform.position = StartPosition;  //���X�n
            this.GetComponent<MeshRenderer>().material.color = Color.blue; //�F�ύX
            BombColor = Color.blue;
            TeamName = Team.TeamOne;
            teamLocal = 1; //���W�̌����C��

        }
        else
        {
            teamTwoIndex++;
            StartPosition = MapManager.Instance.GetStartPosition(teamTwoIndex);

            this.transform.position = StartPosition;  //���X�n
            this.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);  //�A���O��
            this.GetComponent<MeshRenderer>().material.color = Color.red;  //�F�ύX
            BombColor = Color.red;
            TeamName = Team.TeamTwo;
            teamLocal = -1; //���W�̌����C��
        }

    }



    /// <summary>
    /// �v���C���[�̃��X�|�[�����J�n����
    /// </summary>
    protected void Respawn()
    {
        StartCoroutine(StartRespawnRoutine());
    }

    /// <summary>
    /// ���X�|�[���������s���R���[�`��
    /// </summary>
    private IEnumerator StartRespawnRoutine()
    {
        // �����Ȃ�����i���S�j
        currentState = PlayerState.Death;

        // �t�F�[�h�C�������i���j
        Debug.Log("Fade In Start");

        // 4�b�ԑҋ@
        yield return new WaitForSeconds(4f);

        // �t�F�[�h�A�E�g�����i���j
        Debug.Log("Fade Out Start");

        // ���X�|�[�������i���j
        switch(TeamName)
        {
            case Team.TeamOne:
                transform.position = StartPosition;
                break;
            case Team.TeamTwo:
                transform.position = StartPosition;
                break;
        }
        

        // ������悤�ɂ���i�����j
        currentState = PlayerState.Alive;
    }


    #region ���e�֘A
    /// <summary>
    /// ���e��ݒu����֐�
    /// </summary>
    /// <param name="blockData"></param>
    protected void BombPlacement(MapBlockData blockData)
    {
        Vector3 position = blockData.tilePosition;

        GameObject obj = GetBomb();
        if (obj == null)
        {
            return;
        }

        // �����Ń��Z�b�g�I
        obj.transform.SetParent(BombParent);
        obj.transform.position = position;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true); // �ė��p������K���L����
        obj.tag = "FlowerBomb";
        BombProcess BP = obj.GetComponent<BombProcess>();
        BP.VarSetting(BombRange + SpecialBombRange, BombColor, blockData, TeamName);
        BP.StartBombCoutDownCoroutine(BombRange + SpecialBombRange, BombColor, blockData, TeamName);
    }





    /// <summary>
    /// ���e�̌��������ݒ肷��
    /// </summary>
    private void InitializePool()
    {
        foreach(var bloomBomb in BloomBombPool)
        {
            Destroy(bloomBomb);
        }
        BloomBombPool.Clear();
        for (int i = 0; i < BloomBombMax; i++)
        {
            GameObject BloomBomb = Instantiate(StandardBomb, BombParent);
            BloomBomb.SetActive(false);
            BloomBombPool.Add(BloomBomb);
        }
    }


    /// <summary>
    /// ���e���擾���Ă���
    /// </summary>
    /// <returns></returns>
    public GameObject GetBomb()
    {
        foreach (var bomb in BloomBombPool)
        {
            if (!bomb.activeInHierarchy)
            {
                return bomb;
            }
        }

        return null;
    }

    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Exp")
        {
            _levelManager.AddExp(EXP_SIZE);
            Destroy(collision.gameObject);
        }
    }
}
