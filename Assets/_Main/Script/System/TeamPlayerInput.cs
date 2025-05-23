using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Users;

/// <summary>
/// セーブデータに基づいてプレイヤーをスポーンするクラス
/// </summary>
public class TeamPlayerInput : MonoBehaviour
{
    /// <summary>
    /// プレイヤーデータ（ScriptableObject）
    /// </summary>
    private PlayerTeamData _playerData;

    /// <summary>
    /// プレイヤーのプレハブ（PlayerInput コンポーネント付き）
    /// </summary>
    [SerializeField]
    private GameObject _playerPrefab;

    private void Start()
    {
        // Resources フォルダからプレイヤーデータを読み込む
        _playerData = Resources.Load<PlayerTeamData>("PlayerData");
        // 保存された各プレイヤー情報からプレイヤーをスポーン
        for (int i = 0; i < _playerData.PlayerTable.Count; i++)
        {
            SpawnPlayerFromSavedData(_playerData.PlayerTable[i], i);
        }
    }

    /// <summary>
    /// セーブデータを元にプレイヤーをスポーンする
    /// </summary>
    /// <param name="data">プレイヤーデータ</param>
    /// <param name="index">プレイヤー番号</param>
    public void SpawnPlayerFromSavedData(PlayerData data, int index)
    {
        var matchedDevices = new List<InputDevice>();

        foreach (var saved in data.devices)
        {
            InputDevice matched = null;

            // ① deviceId 優先で探す（セッション内で一意）
            foreach (var device in InputSystem.devices)
            {
                if (device.deviceId == saved.sessionDeviceId)
                {
                    matched = device;
                    break;
                }
            }

            // ② なければ description（型・メーカーなど）で妥協的に探す
            if (matched == null)
            {
                var targetDescription = InputDeviceDescription.FromJson(saved.descriptionJson);

                foreach (var device in InputSystem.devices)
                {
                    var currentDescription = device.description;

                    if (currentDescription.interfaceName == targetDescription.interfaceName &&
                        currentDescription.product == targetDescription.product &&
                        currentDescription.manufacturer == targetDescription.manufacturer)
                    {
                        matched = device;
                        break;
                    }
                }
            }

            if (matched != null)
            {
                matchedDevices.Add(matched);
            }
        }

        if (matchedDevices.Count == 0)
        {
            Debug.LogWarning($"プレイヤー {index} に一致するデバイスが見つかりませんでした。");
            return;
        }

        // 最初のデバイスでプレイヤーをスポーン
        var playerInput = PlayerInput.Instantiate(
            _playerPrefab,
            playerIndex: index,
            controlScheme: data.controlScheme,
            pairWithDevice: matchedDevices[0]
        );

        // 2個目以降のデバイスを同じプレイヤーにペアリング
        for (int i = 1; i < matchedDevices.Count; i++)
        {
            InputUser.PerformPairingWithDevice(matchedDevices[i], user: playerInput.user);
        }

        Debug.Log($"プレイヤー {index} を {matchedDevices.Count} 個のデバイスでスポーンしました。");
    }
}
