using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private Dictionary<StatusType, int> _status = new();
    
    private void Awake()
    {
        InitStatus();
    }

    private void InitStatus()
    {
        foreach (StatusType type in System.Enum.GetValues(typeof(StatusType)))
        {
            _status[type] = 1;
        }
    }

    public void Add(StatusType type,int amount)
    {
        _status[type] += amount;
        Debug.Log($"{type}Ç™{amount}ëùâ¡ÇµÇΩ(åªç›{_status[type]})");
    }

    public int GetValue(StatusType type)
    {
        return _status.TryGetValue(type, out  var value) ? value : 1;
    }
}
