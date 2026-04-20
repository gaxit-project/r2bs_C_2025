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
    /// 保存済みのプレイヤーデータをリセット（ファイル削除）
    /// </summary>
    public static void Reset()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("プレイヤーデータをリセットしました（JSONファイル削除）");
        }
        else
        {
            Debug.Log("削除対象のプレイヤーデータファイルは存在しません。");
        }
    }
}
