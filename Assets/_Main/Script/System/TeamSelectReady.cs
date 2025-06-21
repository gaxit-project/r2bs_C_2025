using UnityEngine;

public class TeamSelectReady : MonoBehaviour
{

    bool[] isReady = new bool[] { true, true, true, true };
    bool isAllReady = false;

    public static TeamSelectReady Instance;

    [SerializeField] public GameObject AllReady;

    bool isFirstFlag=true;
    private void Awake()
    {
        Instance = this;
        AllReady.SetActive(false);
        isFirstFlag = true;
    }

    private void Update()
    {
        CheckReady();
        Debug.Log(isReady[0] + "" + isReady[1] + isReady[2] + isReady[3]);
    }

    public void ReadyFlag(int playerIndex)
    {
        isReady[playerIndex] = true;
    }

    public void CancelFlag(int playerIndex)
    {
        Debug.Log("ƒLƒƒƒ“ƒZƒ‹ŠEŒG");
        isReady[playerIndex] = false;
    }



    private void CheckReady()
    {
        if (isReady[0] && isReady[1] && isReady[2] && isReady[3] && !isFirstFlag)
        {
            AllReady.SetActive(true);
            isAllReady = true;
        }
        else
        {
            isAllReady = false;
            AllReady.SetActive(false);
        }
    }


    public bool GetReady()
    {
        return isAllReady;
    }


    public bool GetCurrentReady(int playerIndex)
    {
        if(isFirstFlag)
        {
            isFirstFlag = false;
        }
        return isReady[playerIndex];
    }



    public void ResetFlag(int playerIndex)
    {
        Debug.Log(playerIndex);
        isAllReady = false;
        for (int i = 0; i < playerIndex + 1; i++)
        {
            isReady[i] = false;
        }
        Debug.Log(isReady[playerIndex]);
    }
}
