using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int CurrentLevel = 1;    //�������x�� 1Lv
    public int CurrentExp = 0;      //����EXP

    [Header("PlayerStatus")]
    [SerializeField] private PlayerStatus _status;

    [Header("JobPattern")]
    [SerializeField] private JobLevelPattern _pattern;

    [Header("LevelData")]
    [SerializeField] private LevelExpData _expData;

    /// <summary>
    /// Exp��n���R�[�h
    /// </summary>
    /// <param name="amount"></param>
    public void AddExp(int amount)
    {
        CurrentExp += amount;
        Debug.Log($"�o���l�Q�b�g ���݂�Exp : {CurrentExp}");
        TryLevelUp();
    }

    /// <summary>
    /// ���x�������𖞂����Ă�����A���x���A�b�v�A�����łȂ����break
    /// </summary>
    private void TryLevelUp()
    {
        while (true)
        {
            //�K�v�o���l�擾
            var exp = _expData.ExpTable.Find(e => e.Level == CurrentLevel);

            //���x�������ɖ������Ă��Ȃ��ꍇbreak
            if (exp == null || CurrentExp < exp.ExpToNextLevel) break;

            //���x���A�b�v
            CurrentExp -= exp.ExpToNextLevel;
            CurrentLevel++;

            Debug.Log($"���x���A�b�v! ���݂̃��x��:{CurrentLevel}");

            UpStatus(CurrentLevel);
        }
    }

    /// <summary>
    /// ���x�����オ�����Ƃ����߂Ă����Œ�X�e�[�^�X���㏸������
    /// </summary>
    /// <param name="level"></param>
    private void UpStatus(int level)
    {
        var growth = _pattern.GrowthTable.Find(g => g.Level == level);
        if (growth != null)
        {
            foreach(var stat in growth.LevelStats)
            {
                _status.Add(stat, 1);
            }
        }
        else
        {
            Debug.Log("�����������ݒ肵�Ă�������");
        }

        //player�ɔ��f

    }
}
