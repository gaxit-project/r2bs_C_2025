using UnityEngine;
using UnityEngine.InputSystem;

public class TeamSelectManager : MonoBehaviour
{ 
    public static int playerSum = 0;
    private PlayerTeamData playerData;

    private void Start()
    {
        playerData = Resources.Load<PlayerTeamData>("PlayerData");
        playerData.PlayerTable.Clear();
    }
    public void SavePlayerInput(PlayerInput input)
    {
        int index = input.user.index;

        while (playerData.PlayerTable.Count <= index)
        {
            playerData.PlayerTable.Add(new PlayerData());
        }
        var data = playerData.PlayerTable[input.user.index];
        data.controlScheme = input.currentControlScheme;
        data.devices.Clear();

        foreach (var device in input.user.pairedDevices)
        {
            var info = new SavedDeviceInfo
            {
                descriptionJson = device.description.ToJson()
            };
            data.devices.Add(info);
        }
    }
}
