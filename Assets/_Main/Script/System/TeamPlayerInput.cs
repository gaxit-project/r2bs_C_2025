using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Users;

/// <summary>
/// �Z�[�u�f�[�^�Ɋ�Â��ăv���C���[���X�|�[������N���X
/// </summary>
public class TeamPlayerInput : MonoBehaviour
{
    /// <summary>
    /// �v���C���[�f�[�^�iScriptableObject�j
    /// </summary>
    private PlayerTeamData _playerData;

    /// <summary>
    /// �v���C���[�̃v���n�u�iPlayerInput �R���|�[�l���g�t���j
    /// </summary>
    [SerializeField]
    private GameObject _playerPrefab;

    private void Start()
    {
        // Resources �t�H���_����v���C���[�f�[�^��ǂݍ���
        _playerData = Resources.Load<PlayerTeamData>("PlayerData");
        // �ۑ����ꂽ�e�v���C���[��񂩂�v���C���[���X�|�[��
        for (int i = 0; i < _playerData.PlayerTable.Count; i++)
        {
            SpawnPlayerFromSavedData(_playerData.PlayerTable[i], i);
        }
    }

    /// <summary>
    /// �Z�[�u�f�[�^�����Ƀv���C���[���X�|�[������
    /// </summary>
    /// <param name="data">�v���C���[�f�[�^</param>
    /// <param name="index">�v���C���[�ԍ�</param>
    public void SpawnPlayerFromSavedData(PlayerData data, int index)
    {
        var matchedDevices = new List<InputDevice>();

        foreach (var saved in data.devices)
        {
            InputDevice matched = null;

            // �@ deviceId �D��ŒT���i�Z�b�V�������ň�Ӂj
            foreach (var device in InputSystem.devices)
            {
                if (device.deviceId == saved.sessionDeviceId)
                {
                    matched = device;
                    break;
                }
            }

            // �A �Ȃ���� description�i�^�E���[�J�[�Ȃǁj�őË��I�ɒT��
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
            Debug.LogWarning($"�v���C���[ {index} �Ɉ�v����f�o�C�X��������܂���ł����B");
            return;
        }

        // �ŏ��̃f�o�C�X�Ńv���C���[���X�|�[��
        var playerInput = PlayerInput.Instantiate(
            _playerPrefab,
            playerIndex: index,
            controlScheme: data.controlScheme,
            pairWithDevice: matchedDevices[0]
        );

        // 2�ڈȍ~�̃f�o�C�X�𓯂��v���C���[�Ƀy�A�����O
        for (int i = 1; i < matchedDevices.Count; i++)
        {
            InputUser.PerformPairingWithDevice(matchedDevices[i], user: playerInput.user);
        }

        Debug.Log($"�v���C���[ {index} �� {matchedDevices.Count} �̃f�o�C�X�ŃX�|�[�����܂����B");
    }
}
