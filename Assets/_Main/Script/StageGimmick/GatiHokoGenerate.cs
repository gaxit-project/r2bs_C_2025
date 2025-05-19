using UnityEngine;
using UnityEngine.UIElements;

public class GatiHokoGenerate : MonoBehaviour
{
    public Transform GatiHokoTileGenerate;  // �K�`�z�R�̐�����^�C���̐e�I�u�W�F�N�g�Q��
    public Transform GatiHokoObj; // �K�`�z�R�̐e�I�u�W�F�N�g�Q��
    private int _gatiHokoTileCnt; // �K�`�z�R�̐�����^�C���̐����擾

    [SerializeField] GameObject generatePrefab; // �K�`�z�R�I�u�W�F�N�g���擾

    public static GatiHokoGenerate Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        // �K�`�z�R�̐���������擾
        _gatiHokoTileCnt = GatiHokoTileGenerate.childCount;
    }



    /// <summary>
    /// �K�`�z�R�𐶐�����
    /// </summary>
    public void GatiHokoObjGenerate()
    {
        // ��������ʒu�������_���Ɏ擾
        int gatiHokoGenerateCnt = Random.Range(0, _gatiHokoTileCnt);
        Vector3 gatiHokoPosition = MapManager.Instance.GetGatiHokoPosition(gatiHokoGenerateCnt);

        // �u���b�N�̐���
        GameObject obj = null;
        obj = Instantiate(generatePrefab, gatiHokoPosition, Quaternion.identity, GatiHokoObj);
    }
}
