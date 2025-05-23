using System.IO;
using UnityEngine;

public static class ResultDataIO
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "resultData.json");

    public static void Save(ResultData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(SavePath, json);
    }

    public static void Load(ResultData data)
    {
        if (!File.Exists(SavePath)) return;
        string json = File.ReadAllText(SavePath);
        JsonUtility.FromJsonOverwrite(json, data);
    }

    /// <summary>
    /// �ۑ��ς݂̃v���C���[�f�[�^�����Z�b�g�i�t�@�C���폜�j
    /// </summary>
    public static void Reset()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("�v���C���[�f�[�^�����Z�b�g���܂����iJSON�t�@�C���폜�j");
        }
        else
        {
            Debug.Log("�폜�Ώۂ̃v���C���[�f�[�^�t�@�C���͑��݂��܂���B");
        }
    }
}
