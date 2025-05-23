// PlayerDataIO.cs
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class PlayerDataIO
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "playerData.json");

    [System.Serializable]
    private class PlayerDataWrapper
    {
        public List<PlayerData> PlayerTable = new();
    }

    public static void Save(List<PlayerData> playerTable)
    {
        var wrapper = new PlayerDataWrapper { PlayerTable = playerTable };
        string json = JsonUtility.ToJson(wrapper, prettyPrint: true);
        File.WriteAllText(SavePath, json);
    }

    public static List<PlayerData> Load()
    {
        if (!File.Exists(SavePath)) return new List<PlayerData>();
        string json = File.ReadAllText(SavePath);
        var wrapper = JsonUtility.FromJson<PlayerDataWrapper>(json);
        return wrapper?.PlayerTable ?? new List<PlayerData>();
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
