using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class DebugStageSelect : MonoBehaviour
{
    private string stageName;
    private bool areaBool = false;
    private bool resultBool = true;
    public void OnDebugF1()
    {
        stageName = "stage1";
        DataBase.Instance.SetStageName(stageName);
        Debug.Log("Stage:" + stageName);
    }
    public void OnDebugF2()
    {
        stageName = "stage2";
        DataBase.Instance.SetStageName(stageName);
        Debug.Log("Stage:" + stageName);
    }
    public void OnDebugF3()
    {
        stageName = "stage3";
        DataBase.Instance.SetStageName(stageName);
        Debug.Log("Stage:" + stageName);
    }
    public void OnDebugF4()
    {
        stageName = "stage4";
        DataBase.Instance.SetStageName(stageName);
        Debug.Log("Stage:" + stageName);
    }
    public void OnDebugF5()
    {
        stageName = "stage5";
        DataBase.Instance.SetStageName(stageName);
        Debug.Log("Stage:" + stageName);
    }
    public void OnDebugF6()
    {
        stageName = "stage6";
        DataBase.Instance.SetStageName(stageName);
        Debug.Log("Stage:" + stageName);
    }
    public void OnDebugF7()
    {
        stageName = "stage7";
        DataBase.Instance.SetStageName(stageName);
        Debug.Log("Stage:" + stageName);
    }
    public void OnDebugF8()
    {
        stageName = "stage8";
        DataBase.Instance.SetStageName(stageName);
        Debug.Log("Stage:" + stageName);
    }
    public void OnDebugF9()
    {
        stageName = "stage9";
        DataBase.Instance.SetStageName(stageName);
        Debug.Log("Stage:" + stageName);
    }

    public void OnDebugF12()
    {
        areaBool = !areaBool;
        if (areaBool)
        {
            Debug.Log("Area:On");
        }
        else
        {
            Debug.Log("Area:Off");
        }
        DataBase.Instance.SetAreaBool(areaBool);
    }

    public void OnDebugF13()
    {
        DataBase.Instance.SetAreaBool(true);
        Debug.Log("Area:On");
    }

    public void OnDebugF14()
    {
        DataBase.Instance.SetAreaBool(false);
        Debug.Log("Area:Off");
    }

    public void OnDebugF15()
    {
        DataBase.Instance.SetResultBool(true);
        Debug.Log("gatieria");
    }

    public void OnDebugF16()
    {
        DataBase.Instance.SetResultBool(false);
        Debug.Log("nawabari");
    }
}
