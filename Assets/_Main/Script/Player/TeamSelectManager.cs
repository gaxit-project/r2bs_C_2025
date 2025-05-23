#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.InputSystem;

public class TeamSelectManager : MonoBehaviour
{
    private PlayerTeamData _playerData;

    private void Start()
    {
        _playerData = Resources.Load<PlayerTeamData>("PlayerData");
        _playerData.PlayerTable.Clear();

        // ビルド環境なら保存済みデータを読み込む（あれば）
#if !UNITY_EDITOR
    PlayerDataIO.Reset();
#endif
    }

    public void SavePlayerInput(PlayerInput input)
    {
        int index = input.user.index;

        while (_playerData.PlayerTable.Count <= index)
        {
            _playerData.PlayerTable.Add(new PlayerData());

#if UNITY_EDITOR
            EditorUtility.SetDirty(_playerData);
#else
            PlayerDataIO.Save(_playerData.PlayerTable);
#endif
        }

        var data = _playerData.PlayerTable[index];
        data.controlScheme = input.currentControlScheme;
        data.devices.Clear();

        foreach (var device in input.user.pairedDevices)
        {
            var info = new SavedDeviceInfo
            {
                descriptionJson = device.description.ToJson(),
                sessionDeviceId = device.deviceId
            };
            data.devices.Add(info);
        }

#if !UNITY_EDITOR
        PlayerDataIO.Save(_playerData.PlayerTable);
#endif
    }

    public void OnGameStart()
    {
        bool isSelectTeam = true;
        for (int i = 0; i < _playerData.PlayerTable.Count; i++)
        {
            if (string.IsNullOrEmpty(_playerData.PlayerTable[i].Team))
            {
                isSelectTeam = false;
            }
        }
        if (isSelectTeam)
        {
            FBSceneManager.Instance.LoadMainScene();
        }
    }
}
