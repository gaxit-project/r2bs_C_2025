using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenSplit : MonoBehaviour
{
    public GameObject tateFrame;
    public GameObject yokoFrame;
    public GameObject SoloMap;
    public GameObject MultiMap;


    private void Start()
    {
        tateFrame.SetActive(false);
        yokoFrame.SetActive(false);
        SoloMap.SetActive(true);
        MultiMap.SetActive(false);
    }

    // プレイヤー入室時に起きる関数
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if(playerInput.user.index == 1)
        {
            SoloMap.SetActive(false);
            MultiMap.SetActive(true);
            tateFrame.SetActive(true);
        }else if(playerInput.user.index == 2)
        {
            yokoFrame.SetActive(true);
        }
    }


    // プレイヤー退室時に受け取る通知
    public void OnPlayerLeft(PlayerInput playerInput)
    {
        if (playerInput.user.index == 1)
        {
            tateFrame.SetActive(false);
            SoloMap.SetActive(true);
            MultiMap.SetActive(false);
        }
        else if (playerInput.user.index == 2)
        {
            yokoFrame.SetActive(false);
        }
    }
}
