using System.Collections.Generic;
using UnityEngine;

//�\�͂̎��(�ǉ��\)
public enum StatusType
{
    Speed,
    Power,
    BombCount
}

/// <summary>
/// ���x���A�b�v�̃X�e�[�^�X
/// </summary>
[System.Serializable]
public class LevelUp
{
    public int Level;
    public List<StatusType> LevelStats;
}
