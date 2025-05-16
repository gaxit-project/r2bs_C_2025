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
    public PlayerInput playerInput; //�R���g���[���[�f�o�C�X���
    public string Team; //�`�[�����
}
