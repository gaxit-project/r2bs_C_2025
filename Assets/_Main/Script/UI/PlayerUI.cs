using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI level;
    [SerializeField]
    private TextMeshProUGUI bombCnt;
    [SerializeField]
    private TextMeshProUGUI nowBombCnt;
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
    private string iniNowBombCnt;
    private string iniBombRange;
    private string iniSpeed;
    private string iniCurrentExp;
    private string iniNeedExp;

    private int BombCnt = 1;

    public int BombRange = 1;
    public int Speed = 1;

    public int Rlevel = 1;
    public int RnExp = 1;
    public int Rexp = 0;
    public int Rcnt = 1;
    private void Awake()
    {
        iniLevel = level.text;
        iniBombCnt = bombCnt.text;
        iniNowBombCnt = nowBombCnt.text;
        iniBombRange = bombRange.text;
        iniSpeed = speed.text;
        iniCurrentExp = currentExp.text;
        iniNeedExp = needExp.text;
    }
    private void Start()
    {
        addLevel(1);
        addBombCnt(0);
        addNowBombCnt(1);
        addBombRange(0);
        addCurrentExp(0);
        addNeedExp(1);
        addSpeed(0);

    }
    public void addLevel(int value)
    {
        Rlevel = value;
        level.text = iniLevel + value.ToString();
    }

    private void addBombCnt(int value)
    {
        BombCnt += value;
        bombCnt.text = iniBombCnt + BombCnt.ToString();
    }

    public void addNowBombCnt(int value)
    {
        Rcnt = value;
        nowBombCnt.text = iniNowBombCnt + value.ToString();
    }

    public void addBombRange(int value)
    {
        BombRange += value;
        bombRange.text = iniBombRange + BombRange.ToString();
    }

    public void addSpeed(int value)
    {
        Speed += value;
        speed.text = iniSpeed + Speed.ToString();
    }

    public void addCurrentExp(int value)
    {
        Rexp = value;
        currentExp.text = iniCurrentExp + value.ToString();
    }

    public void addNeedExp(int value)
    {
        RnExp = value;
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
