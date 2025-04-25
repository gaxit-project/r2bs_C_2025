using UnityEngine;
using static MapManager;

public class PlayerBase : MonoBehaviour
{
    // �v���C���[�֘A�̕ϐ�
    protected float Speed;   // �v���C���[�̃X�s�[�h
    // ���e�֘A�̕ϐ�
    [SerializeField] private GameObject _standardBomb;  // ���e������z��
    private Transform BombParent;                  // ���e�̐�����I�u�W�F�N�g
    protected int BombRange; // �{���̔����͈�
    protected int BombCnt;   // �{���̏�����

    private void Start()
    {
        // �{����Prefab�Ɛ�����I�u�W�F�N�g�̎擾
        _standardBomb = Resources.Load<GameObject>("Prefab/StandardBomb");
        GameObject bombParentObj = GameObject.Find("BombGenerate");
        BombParent = bombParentObj.transform;
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



    #region ���e�֘A
    /// <summary>
    /// ���e��ݒu����֐�
    /// </summary>
    /// <param name="blockData"></param>
    protected void BombPlacement(MapBlockData blockData)
    {
        Vector3 position = blockData.tilePosition; // ���݂̃|�W�V�����擾
        GameObject obj = Instantiate(_standardBomb, position, Quaternion.identity, BombParent);
        // �{�����N��������O���֐�.Instance.�֐���(BombRange);
    }
    #endregion
}
