using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinLog : MonoBehaviour
{
    // �v���C���[�������Ɏ󂯎��ʒm
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log($"�v���C���[#{playerInput.user.index}�������I");
    }

    // �v���C���[�ގ����Ɏ󂯎��ʒm
    private void OnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log($"�v���C���[#{playerInput.user.index}���ގ��I");
    }
}