using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShimoDebugController : MonoBehaviour
{
    public float playerSpeed = 10f; //プレイヤーの速度
    private Vector2 moveInput = Vector2.zero; //入力格納

    private void Update()
    {
        var move = new Vector3(moveInput.x, 0f, moveInput.y) * playerSpeed * Time.deltaTime; //Timeはポーズ画面時止まるよう
        transform.Translate(move);
    }


    //プレイヤーの移動
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLeft()
    {
        Destroy(this.gameObject);
    }
}