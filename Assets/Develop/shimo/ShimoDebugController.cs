using UnityEngine;
using UnityEngine.InputSystem;

public class ShimoDebugController : MonoBehaviour
{
    private float speed = 10f;
    private Vector2 moveInput = Vector2.zero;

    private void Update()
    {
        var move = new Vector3(moveInput.x, 0f, moveInput.y) * speed * Time.deltaTime;
        transform.Translate(move);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jumping");
    }
}