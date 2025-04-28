using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;
using static MapManager;

public class PlayerBase : MonoBehaviour
{
    // �v���C���[�֘A�̕ϐ�
    protected float PlayerSpeed = 10f; //�v���C���[�̑��x
    protected Vector2 moveInput = Vector2.zero; //���͊i�[
    protected Team TeamName;   // �`�[�����̕ۑ�
    protected Vector3 StartPosition;
    // �v���C���[�̏�Ԃ��Ǘ����� (0: ����, 1: ���S)
    public enum PlayerState
    {
        Alive,
        Death
    }
    public PlayerState currentState;
    // ���e�֘A�̕ϐ�
    [SerializeField] protected GameObject StandardBomb;  // ���e������z��
    protected Transform BombParent;                  // ���e�̐�����I�u�W�F�N�g
    protected int BombRange = 5; // �{���̔����͈�
    protected int BombCnt = 1;   // �{���̏����� 
    protected Color BombColor = Color.black; // ���e�̐F�̐ݒ�
    protected int BloomBombMax = 5;          // ���e�̏������̃}�b�N�X�̐ݒ�
    protected List<GameObject> BloomBombPool = new List<GameObject>(); // �{�������郊�X�g
    //�v���C���[���i�[����z��
    private GameObject[] players = null;


    protected void Start()
    {
        InitializePool();
    }

    protected void Update()
    {
        //�v���C���[�̈ړ�
        PlayerMove();
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
        if(currentState != PlayerState.Alive)
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
        var move = new Vector3(moveInput.x, 0f, moveInput.y) * PlayerSpeed * Time.deltaTime; //Time�̓|�[�Y��ʎ��~�܂�悤
        transform.Translate(move);
    }

    //�`�[������
    protected void TeamSplit()
    {
        players = GameObject.FindGameObjectsWithTag("Player"); //player�̔z��
        StartPosition = MapManager.Instance.GetStartPosition(players.Length);
        if (players.Length % 2 == 1)
        {
            this.transform.position = StartPosition;  //���X�n
            this.GetComponent<MeshRenderer>().material.color = Color.blue;
            BombColor = Color.blue;
            TeamName = Team.TeamOne;
            this.gameObject.tag = "TeamOne";
        }
        else
        {
            this.transform.position = StartPosition;  //���X�n
            this.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);  //�A���O��
            this.GetComponent<MeshRenderer>().material.color = Color.red;  //�F�ύX
            BombColor = Color.red;
            TeamName = Team.TeamTwo;
            this.gameObject.tag = "TeamTwo";
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
        BP.VarSetting(BombRange, BombColor, blockData, TeamName);
        BP.StartBombCoutDownCoroutine(BombRange, BombColor, blockData, TeamName);
    }





    /// <summary>
    /// ���e�̌��������ݒ肷��
    /// </summary>
    private void InitializePool()
    {
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
}
