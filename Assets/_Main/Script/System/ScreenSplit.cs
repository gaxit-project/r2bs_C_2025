using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenSplit : MonoBehaviour
{
    public GameObject tateFrame;
    public GameObject yokoFrame;
    public GameObject SoloMap;
    public GameObject MultiMap;

    /// <summary>
    /// �v���C���[�f�[�^�iScriptableObject�j
    /// </summary>
    private PlayerTeamData _playerData;


    private void Start()
    {
        // Resources �t�H���_����v���C���[�f�[�^��ǂݍ���
        _playerData = Resources.Load<PlayerTeamData>("PlayerData");

        switch (_playerData.PlayerTable.Count)
        {
            case 1:
                tateFrame.SetActive(false);
                yokoFrame.SetActive(false);
                SoloMap.SetActive(true);
                MultiMap.SetActive(false);
                break;
            case 2:
                tateFrame.SetActive(true);
                yokoFrame.SetActive(false);
                SoloMap.SetActive(false);
                MultiMap.SetActive(true);
                break;
            case 3:
                tateFrame.SetActive(true);
                yokoFrame.SetActive(true);
                SoloMap.SetActive(false);
                MultiMap.SetActive(true);
                break;
            case 4:
                tateFrame.SetActive(true);
                yokoFrame.SetActive(true);
                SoloMap.SetActive(false);
                MultiMap.SetActive(true);
                break;
        }
    }

    // �v���C���[�������ɋN����֐�
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


    // �v���C���[�ގ����Ɏ󂯎��ʒm
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
