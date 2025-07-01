using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    private string stageName = "stage5";
    private bool areaBool = false;
    private bool resultBool = true;

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
