using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerTeamData", menuName = "Scriptable Objects/PlayerTeamData")]

public class PlayerTeamData : ScriptableObject
{
    public List<PlayerData> PlayerTable;
}

[System.Serializable]

public class PlayerData
{ 
    public string controlScheme;　//スキーム
    public List<SavedDeviceInfo> devices = new List<SavedDeviceInfo>(); //デバイス情報
    public string Team; //チーム情報
}

[System.Serializable]
public class SavedDeviceInfo
{
    public string descriptionJson; // InputDeviceDescription を JSON で保存
    public int sessionDeviceId; //固有デバイスID
}
