using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAngle : MonoBehaviour
{
    private Vector2 moveInput = Vector2.zero; //入力格納
    private float rotateSpeed = 8f;  //playerのアングルspeed
    private int _teamLocal = 1;
    // プレイヤーの状態を管理する (0: 生存, 1: 死亡)

    public enum PlayerState
    {
        Alive,
        Death
    }
    public PlayerState currentState;

    void Update()
    {
        PlayerMove();
    }
    private void PlayerMove()
    {
        moveInput = this.transform.parent.GetComponent<PlayerBase>().getMoveInput;
        Vector3 moveValue = new Vector3(moveInput.x * _teamLocal, 0f, moveInput.y * _teamLocal);
        if (currentState == PlayerState.Alive && GameTimer.instance.IsGameStart())
        {
            transform.forward = Vector3.Slerp(transform.forward, moveValue,Time.deltaTime * rotateSpeed); //angle変更
        }
        else
        {
            transform.forward = Vector3.Slerp(transform.forward, moveValue, 0);
        }
    }
}
