using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private float _dropRate = 0.7f; // 0.0f?1.0f�i�m���j
    [SerializeField] private bool _useFixedSeed = false; //�f�o�b�N���[�h�p
    [SerializeField] private int _fixedSeed = 1; //�����̃V�[�h�l

    private static bool _isSeedInItialized = false; //�V�[�h�l�����������ꂽ���ǂ���


    /// <summary>
    /// �u���b�N���󂵂��Ƃ��ɃA�C�e���������_���Ő�������
    /// </summary>
    public void TryDropItemFromBlock(Vector3 position)
    {
        InitSeedIfNeeded();
        Debug.Log("������" + Random.value);

        if (Random.value <= _dropRate)
        {

            //position���󂯎���āC���̏�ɐ�������
            //Instantiate(_itemPrefab, position, Quaternion.identity);
        }

    }

    /// <summary>
    /// �f�o�b�N���[�h�̎��̓V�[�h�l���Œ肷��
    /// </summary>
    private void InitSeedIfNeeded()
    {
        if (_useFixedSeed && !_isSeedInItialized)
        {
            Random.InitState(_fixedSeed);
            _isSeedInItialized = true;
        }
    }

    /// <summary>
    /// �G��|�����Ƃ��ɃA�C�e�����m��Ő�������
    /// �i��Ɍo���l�v�Z�ȂǂɊg���\�Ȑ݌v�j
    /// </summary>
    public void DropItemFromEnemy(Vector3 position, int enemyLevel, int playerLevel)
    {
        // �g���������������݌v�i���̌o���l�v�Z�j
        int exp = CalculateExperience(enemyLevel, playerLevel);
        Debug.Log("�o���l�l��: " + exp);

        //position���󂯎���āC���̏�ɐ�������(�����A�C�e���𐶐����邩�͖���)
        Instantiate(_itemPrefab, position, Quaternion.identity);
    }

    /// <summary>
    /// ���x�����ɉ����Čo���l���v�Z����i����g���\�j
    /// </summary>
    private int CalculateExperience(int enemyLevel, int playerLevel)
    {
        int baseExp = 10;
        int levelDifference = enemyLevel - playerLevel;
        float multiplier = 1.0f + (levelDifference * 0.1f);
        return Mathf.Max(1, Mathf.RoundToInt(baseExp * multiplier)); //1�ƃ��x�����ɂ���ďo���ꂽ�l�̂����C�傫����(����)��Ԃ�
    }


    /*private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            TryDropItemFromBlock(new Vector3(0, 0, 0));
        }
    }*/
}
