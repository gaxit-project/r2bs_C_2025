using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShimoDebugController : MonoBehaviour
{
    public float playerSpeed = 10f; //�v���C���[�̑��x
    private Vector2 moveInput = Vector2.zero; //���͊i�[

    private void Update()
    {
        var move = new Vector3(moveInput.x, 0f, moveInput.y) * playerSpeed * Time.deltaTime; //Time�̓|�[�Y��ʎ��~�܂�悤
        transform.Translate(move);
    }


    //�v���C���[�̈ړ�
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLeft()
    {
        Destroy(this.gameObject);
    }
}