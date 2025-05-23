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


    //カメラの分割割り当て
    private Rect GetCameraRect(int playerCount, int index)
    {
        if (playerCount <= 1)
        {
            return new Rect(0f, 0f, 1f, 1f); // 全画面
        }
        else if (playerCount == 2)
        {
            return index == 0
                ? new Rect(0f, 0f, 0.5f, 1f)   // 左
                : new Rect(0.5f, 0f, 0.5f, 1f); // 右
        }
        else if (playerCount == 3)
        {
            switch (index)
            {
                case 0: return new Rect(0f, 0f, 0.5f, 0.5f);     // 左下
                case 1: return new Rect(0.5f, 0f, 0.5f, 0.5f);   // 右下
                case 2: return new Rect(0f, 0.5f, 1f, 0.5f);     // 上全体
            }
        }
        else if (playerCount >= 4)
        {
            switch (index)
            {
                case 0: return new Rect(0f, 0f, 0.5f, 0.5f);     // 左下
                case 1: return new Rect(0.5f, 0f, 0.5f, 0.5f);   // 右下
                case 2: return new Rect(0f, 0.5f, 0.5f, 0.5f);   // 左上
                case 3: return new Rect(0.5f, 0.5f, 0.5f, 0.5f); // 右上
            }
        }

        // 5人以上は全画面（または切替UIなど別対応）
        return new Rect(0f, 0f, 1f, 1f);
    }


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
            pairWithDevice: matchedDevices[0],
            splitScreenIndex: index
        );

        // 2個目以降のデバイスを同じプレイヤーにペアリング
        for (int i = 1; i < matchedDevices.Count; i++)
        {
            InputUser.PerformPairingWithDevice(matchedDevices[i], user: playerInput.user);
        }

        // カメラ生成と割当て
        Camera playerCam = playerInput.gameObject.transform.GetChild(0).GetComponent<Camera>();
        playerInput.camera = playerCam;

        playerCam.rect = GetCameraRect(_playerData.PlayerTable.Count, index);

        Debug.Log($"プレイヤー {index} を {matchedDevices.Count} 個のデバイスでスポーンしました。");
    }
}
