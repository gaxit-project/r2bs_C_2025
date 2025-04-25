using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinLog : MonoBehaviour
{
    // �v���C���[�������Ɏ󂯎��ʒm
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log($"�v���C���[#{playerInput.user.index}�������I");
    }

    // �v���C���[�ގ����Ɏ󂯎��ʒm
    public void OnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log($"�v���C���[#{playerInput.user.index}���ގ��I");
    }
}