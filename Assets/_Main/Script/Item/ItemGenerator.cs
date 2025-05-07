using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    private const int FIXED_SEED = 1; //�����̃V�[�h�l
    private const int BASE_EXP = 10;

    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private float _dropRate = 0.7f; // 0.0f����1.0f�i�m���j
    [SerializeField] private bool _debugSeed = false; //�f�o�b�N���[�h�p

    /// <summary>
    /// �f�o�b�N���[�h�̎��̓V�[�h�l���Œ肷��
    /// </summary>
    private void Awake()
    {
        if (_debugSeed)
        {
            Random.InitState(FIXED_SEED);
        }
    }
    /// <summary>
    /// �u���b�N���󂵂��Ƃ��ɃA�C�e���������_���Ő�������
    /// </summary>
    public void TryDropExp(Vector3 position)
    {
        Debug.Log("������" + Random.value);

        if (Random.value <= _dropRate)
        {

            //position���󂯎���āC���̏�ɐ�������
            //Instantiate(_itemPrefab, position, Quaternion.identity);
        }

    }

    
    /// <summary>
    /// �G��|�����Ƃ��ɃA�C�e�����m��Ő�������
    /// �i��Ɍo���l�v�Z�ȂǂɊg���\�Ȑ݌v�j
    /// </summary>
    public void DropExp(Vector3 position, int enemyLevel, int playerLevel)
    {
        // �g���������������݌v�i���̌o���l�v�Z�j
        int exp = CalcExp(enemyLevel, playerLevel);
        Debug.Log("�o���l�l��: " + exp);

        //position���󂯎���āC���̏�ɐ�������(�����A�C�e���𐶐����邩�͖���)
        Instantiate(_itemPrefab, position, Quaternion.identity);
    }

    /// <summary>
    /// ���x�����ɉ����Čo���l���v�Z����i����g���\�j
    /// </summary>
    private int CalcExp(int enemyLevel, int playerLevel)
    {
        int baseExp = BASE_EXP;
        int levelDifference = enemyLevel - playerLevel;
        float multiplier = 1.0f + (levelDifference * 0.1f); //���v�Z�p
        return Mathf.Max(1, Mathf.RoundToInt(baseExp * multiplier)); //1�ƃ��x�����ɂ���ďo���ꂽ�l�̂����C�傫����(����)��Ԃ�
    }


    //�f�o�b�N�p
    /*private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            TryDropExp(new Vector3(0, 0, 0));
        }
    }*/
}
