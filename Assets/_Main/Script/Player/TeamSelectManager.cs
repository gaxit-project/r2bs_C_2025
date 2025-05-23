using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �`�[���I���V�[���Ńv���C���[�̓��͏���ۑ�����}�l�[�W���[
/// </summary>
public class TeamSelectManager : MonoBehaviour
{
    /// <summary>
    /// �Q�����Ă���v���C���[�̍��v���i���N���X����A�N�Z�X�\�j
    /// </summary>

    /// <summary>
    /// �v���C���[���̓f�[�^��ێ�����ScriptableObject
    /// </summary>
    private PlayerTeamData _playerData;

    private void Start()
    {
        // �v���C���[�f�[�^��ǂݍ��݁A�����̃v���C���[����������
        _playerData = Resources.Load<PlayerTeamData>("PlayerData");
        _playerData.PlayerTable.Clear();
    }

    /// <summary>
    /// PlayerInput����v���C���[�̓��̓f�o�C�X�Ƒ�����@��ۑ�����
    /// </summary>
    /// <param name="input">�v���C���[�̓��͏��</param>
    public void SavePlayerInput(PlayerInput input)
    {
        int index = input.user.index;

        // �C���f�b�N�X������Ȃ��ꍇ�͋��PlayerData��ǉ����Ă���
        while (_playerData.PlayerTable.Count <= index)
        {
            _playerData.PlayerTable.Add(new PlayerData());
            EditorUtility.SetDirty(_playerData);
        }

        var data = _playerData.PlayerTable[index];
        data.controlScheme = input.currentControlScheme;
        data.devices.Clear();

        // �g�p���̊e�f�o�C�X����ۑ�
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
    /// team�I�����Ȃ��ƃV�[���ړ����Ȃ�
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
