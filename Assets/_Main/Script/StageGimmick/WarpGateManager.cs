using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class WarpGateManager : MonoBehaviour
{
    public List<int> warpNum = new List<int>();

    public static WarpGateManager Instance;
    void Awake()
    {
        Instance = this;
    }



    public void AddWarpGateNumber(int num)
    {
        warpNum.Add(num);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
