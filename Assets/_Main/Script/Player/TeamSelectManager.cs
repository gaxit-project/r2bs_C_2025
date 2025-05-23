using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// チーム選択シーンでプレイヤーの入力情報を保存するマネージャー
/// </summary>
public class TeamSelectManager : MonoBehaviour
{
    /// <summary>
    /// 参加しているプレイヤーの合計数（他クラスからアクセス可能）
    /// </summary>

    /// <summary>
    /// プレイヤー入力データを保持するScriptableObject
    /// </summary>
    private PlayerTeamData _playerData;

    private void Start()
    {
        // プレイヤーデータを読み込み、既存のプレイヤー情報を初期化
        _playerData = Resources.Load<PlayerTeamData>("PlayerData");
        _playerData.PlayerTable.Clear();
    }

    /// <summary>
    /// PlayerInputからプレイヤーの入力デバイスと操作方法を保存する
    /// </summary>
    /// <param name="input">プレイヤーの入力情報</param>
    public void SavePlayerInput(PlayerInput input)
    {
        int index = input.user.index;

        // インデックスが足りない場合は空のPlayerDataを追加しておく
        while (_playerData.PlayerTable.Count <= index)
        {
            _playerData.PlayerTable.Add(new PlayerData());
            EditorUtility.SetDirty(_playerData);
        }

        var data = _playerData.PlayerTable[index];
        data.controlScheme = input.currentControlScheme;
        data.devices.Clear();

        // 使用中の各デバイス情報を保存
        foreach (var device in input.user.pairedDevices)
        {
            var info = new SavedDeviceInfo
            {
                descriptionJson = device.description.ToJson(),
                sessionDeviceId = device.deviceId
            };
            data.devices.Add(info);
        }
    }

    /// <summary>
    /// team選択しないとシーン移動しない
    /// </summary>
    public void OnGameStart()
    {
        bool isSelectTeam = true;
        for (int i = 0; i < _playerData.PlayerTable.Count; i++)
        {
            if (_playerData.PlayerTable[i].Team == null)
            {
                isSelectTeam = false;
            }
        }
        if(isSelectTeam)
        {
            FBSceneManager.Instance.LoadMainScene();
        }
    }
}
