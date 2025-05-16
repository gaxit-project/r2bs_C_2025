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
    public PlayerInput playerInput; //コントローラーデバイス情報
    public string Team; //チーム情報
}
