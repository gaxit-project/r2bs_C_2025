using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : PlayerBase
{
    private void Awake()
    {
        // �{����Prefab�Ɛ�����I�u�W�F�N�g�̎擾
        StandardBomb = Resources.Load<GameObject>("Prefab/StandardBomb");
        GameObject bombParentObj = GameObject.Find("BombGenerate");
        BombParent = bombParentObj.transform;

        //�`�[������
        TeamSplit();
    }


    public void RespawnPlayer()
    {
        Respawn();
    }
}
