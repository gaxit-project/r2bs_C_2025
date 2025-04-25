using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinLog : MonoBehaviour
{
    // プレイヤー入室時に受け取る通知
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log($"プレイヤー#{playerInput.user.index}が入室！");
    }

    // プレイヤー退室時に受け取る通知
    private void OnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log($"プレイヤー#{playerInput.user.index}が退室！");
    }
}