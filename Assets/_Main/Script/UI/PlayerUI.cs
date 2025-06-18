using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI level;
    [SerializeField]
    private TextMeshProUGUI bombCnt;
    [SerializeField]
    private TextMeshProUGUI bombRange;
    [SerializeField]
    private TextMeshProUGUI speed;
    [SerializeField]
    private TextMeshProUGUI currentExp;
    [SerializeField]
    private TextMeshProUGUI needExp;

    private string iniLevel;
    private string iniBombCnt;
    private string iniBombRange;
    private string iniSpeed;
    private string iniCurrentExp;
    private string iniNeedExp;

    private void Start()
    {
        iniLevel = level.text;
        iniBombCnt = bombCnt.text;
        iniBombRange = bombRange.text;
        iniSpeed = speed.text;
        iniCurrentExp = currentExp.text;
        iniNeedExp = needExp.text;
        addLevel(1);
        addBombCnt(0);
        addBombRange(0);
        addCurrentExp(0);
        addNeedExp(1);
        addSpeed(0);

    }
    public void addLevel(int value)
    {
        level.text = iniLevel + value.ToString();
    }

    private void addBombCnt(int value)
    {
        bombCnt.text = iniBombCnt + (1 + value).ToString();
    }

    private void addBombRange(int value)
    {
        bombRange.text = iniBombRange + (1 + value).ToString();
    }

    private void addSpeed(int value)
    {
        speed.text = iniSpeed + (1 + value).ToString();
    }

    public void addCurrentExp(int value)
    {
        currentExp.text = iniCurrentExp + value.ToString();
    }

    public void addNeedExp(int value)
    {
        needExp.text = iniNeedExp + value.ToString();
    }

    public void addStatusUp(StatusType _status, int value)
    {
        if(_status == StatusType.Speed)
        {
            addSpeed(value);
        }
        else if(_status == StatusType.Power)
        {
            addBombRange(value);
        }
        else if(_status==StatusType.BombCount)
        {
            addBombCnt(value);
        }
    }
}
