using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;
using static MapManager;

public class PlayerBase : MonoBehaviour
{
    // �v���C���[�֘A�̕ϐ�
    protected float playerSpeed = 10f; //�v���C���[�̑��x
    protected Vector2 moveInput = Vector2.zero; //���͊i�[
    // ���e�֘A�̕ϐ�
    [SerializeField] protected GameObject _standardBomb;  // ���e������z��
    protected Transform BombParent;                  // ���e�̐�����I�u�W�F�N�g
    protected int BombRange = 5; // �{���̔����͈�
    protected int BombCnt = 1;   // �{���̏�����
    protected Color BombColor = Color.black;
    //�v���C���[���i�[����z��
    private GameObject[] players = null;

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
        moveInput = context.ReadValue<Vector2>();
        Debug.Log("���񂿂���");
    }



    //���e�ݒu
    public void OnBomb()
    {
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
        var move = new Vector3(moveInput.x, 0f, moveInput.y) * playerSpeed * Time.deltaTime; //Time�̓|�[�Y��ʎ��~�܂�悤
        transform.Translate(move);
    }

    //�`�[������
    protected void TeamSplit()
    {
        players = GameObject.FindGameObjectsWithTag("Player"); //player�̔z��
        if(players.Length % 2 == 1)
        {
            this.transform.position = new Vector3(10.5f, 0, 1.5f);  //���X�n
            this.GetComponent<MeshRenderer>().material.color = Color.blue;  //
            BombColor = Color.blue;
        }
        else
        {
            this.transform.position = new Vector3(10.5f, 0, 23.5f);  //���X�n
            this.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);  //�A���O��
            this.GetComponent<MeshRenderer>().material.color = Color.red;  //�F�ύX
            BombColor = Color.red;
        }
    }



    #region ���e�֘A
    /// <summary>
    /// ���e��ݒu����֐�
    /// </summary>
    /// <param name="blockData"></param>
    protected void BombPlacement(MapBlockData blockData)
    {
        Vector3 position = blockData.tilePosition; // ���݂̃|�W�V�����擾
        GameObject obj = Instantiate(_standardBomb, position, Quaternion.identity, BombParent);
        BombProcess BP = obj.GetComponent<BombProcess>();
        BP.StartBombCoutDownCoroutine(BombRange, BombColor, blockData);
    }
    #endregion
}
