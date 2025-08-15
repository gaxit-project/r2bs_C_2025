using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    private string stageName = "stage5";
    private bool areaBool = false;
    private bool resultBool = true;

    //Reward
    public float getExp = 1f;
    public float placeBomb = 0.1f;
    public float upLevel = 3f;
    public float attackEnemy = 10f;
    public float getErea = 30f;
    public float win = 100f;
    public float death = -10f;
    public float lossErea = -30f;
    public float lose = -100f;
    public float paintErea = 1f;
    public float paintTile = 0.5f;

    /// <summary>
    /// シングルトンインスタンス
    /// </summary>
    public static DataBase Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetStageName(string stageName)
    {
        this.stageName = stageName;
    }

    public void SetAreaBool(bool areaBool)
    {
        this.areaBool = areaBool;
    }

    public string GetStageName()
    {
        return stageName;
    }

    public bool GetAreaBool()
    {
        return areaBool;
    }

    public void SetResultBool(bool resultBool)
    {
        this.resultBool = resultBool; 
    }

    public bool GetRssultBool()
    {
        return resultBool;
    }
}
