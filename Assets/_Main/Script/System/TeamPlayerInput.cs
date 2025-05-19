using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Users;

public class PlayerSpawner : MonoBehaviour
{
    private PlayerTeamData playerData; // ScriptableObjectをインスペクタで割り当て
    [SerializeField] private GameObject playerPrefab;   // プレイヤープレハブ（PlayerInput付き）

    void Start()
    {
        playerData = Resources.Load<PlayerTeamData>("PlayerData");
        for (int i = 0; i < playerData.PlayerTable.Count; i++)
        {
            SpawnPlayerFromSavedData(playerData.PlayerTable[i], i);
        }
    }

    public void SpawnPlayerFromSavedData(PlayerData data, int index)
    {
        var matchedDevices = new List<InputDevice>();

        foreach (var saved in data.devices)
        {
            var targetDesc = InputDeviceDescription.FromJson(saved.descriptionJson);

            foreach (var device in InputSystem.devices)
            {
                var desc = device.description;

                if (desc.interfaceName == targetDesc.interfaceName &&
                    desc.product == targetDesc.product &&
                    desc.manufacturer == targetDesc.manufacturer)
                {
                    matchedDevices.Add(device);
                    break;
                }
            }
        }

        if (matchedDevices.Count == 0)
        {
            Debug.LogWarning($"No devices matched for player {index}.");
            return;
        }

        var playerInput = PlayerInput.Instantiate(
            playerPrefab,
            playerIndex: index,
            controlScheme: data.controlScheme,
            pairWithDevice: matchedDevices[0]
        );


        for (int i = 1; i < matchedDevices.Count; i++)
        {
            InputUser.PerformPairingWithDevice(matchedDevices[i], user: playerInput.user);
        }

        Debug.Log($"Spawned player {index} with {matchedDevices.Count} device(s).");
    }
}
