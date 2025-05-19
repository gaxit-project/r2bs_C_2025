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
    public string controlScheme;
    public List<SavedDeviceInfo> devices = new List<SavedDeviceInfo>();
    public string Team; //ƒ`[ƒ€î•ñ
}

[System.Serializable]
public class SavedDeviceInfo
{
    public string descriptionJson; // InputDeviceDescription ‚ğ JSON ‚Å•Û‘¶
}
