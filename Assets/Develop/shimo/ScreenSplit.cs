using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenSplit : MonoBehaviour
{
    public GameObject tateFrame;
    public GameObject yokoFrame;


    private void Start()
    {
        tateFrame.SetActive(false);
        yokoFrame.SetActive(false);
    }

    // �v���C���[�������ɋN����֐�
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if(playerInput.user.index == 1)
        {
            tateFrame.SetActive(true);
        }else if(playerInput.user.index == 2)
        {
            yokoFrame.SetActive(true);
        }
    }


    // �v���C���[�ގ����Ɏ󂯎��ʒm
    public void OnPlayerLeft(PlayerInput playerInput)
    {
        if (playerInput.user.index == 1)
        {
            tateFrame.SetActive(false);
        }
        else if (playerInput.user.index == 2)
        {
            yokoFrame.SetActive(false);
        }
    }
}
