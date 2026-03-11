using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NewLog : MonoBehaviour
{
    public static NewLog Instance { get; private set; }

    [Header("GAS Settings")]
    [SerializeField] private string gasUrl = "https://script.google.com/macros/s/AKfycbyo19az3Hi-B4hYbyrUtfeTiUFj07r1PjJ1fqvPo8XCJRGYsD7a7fRs91M1oUE-uuMrTQ/exec";
    [SerializeField] private float sendInterval = 1.0f; // 1秒ごとにまとめて送信

    private List<string[]> _logBuffer = new List<string[]>();
    private string _sessionId = "";
    private string _currentStageName = "Stage_01";
    private bool _isInitialized = false; // IDが確定したか

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    async void Start() // asyncを付ける
    {
        // 1. まずはサーバーから一意のカウントアップIDを取得する
        StartCoroutine(FetchNextSessionId());

        // 2. ログ送信ループを開始
        StartCoroutine(LogUploadRoutine());

        // 💡 IDが初期化されるまで待つ
        while (!_isInitialized)
        {
            await System.Threading.Tasks.Task.Yield();
        }

        // ID確定後にテストログを発生させる
        //Debug.Log("ID確定後のテストログ生成を開始します");
        //for (int i = 0; i < 100; i++)
        //{
        //    // テスト用：座標ゼロで送信
        //    SendLog("BurstTest", Vector3.zero, "Red", "TestPlayer", "None", $"Memo_{i}");
        //}
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            SendLog("BurstTest", Vector3.zero, "Red", "TestPlayer", "None", $"Memo_");
        }
    }

    /// <summary>
    /// GASのdoGetを叩いて、Configシートから排他制御された連番IDをもらう
    /// </summary>
    private IEnumerator FetchNextSessionId()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(gasUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                _sessionId = www.downloadHandler.text.Trim();
                Debug.Log($"<color=green>[LogManager] 試合IDを取得しました: {_sessionId}</color>");
            }
            else
            {
                // サーバーエラー等の場合は、データが混ざらないようUUIDで代用
                _sessionId = "Backup_" + Guid.NewGuid().ToString().Substring(0, 8);
                Debug.LogWarning($"[LogManager] ID取得失敗。バックアップIDを使用します: {_sessionId}");
            }

            _isInitialized = true;
        }
    }

    /// <summary>
    /// 15列のログをバッファに格納する
    /// </summary>
    public void SendLog(
        string eventType,
        Vector3 pos,          // 各プレイヤーの現在座標
        string team = "None",
        string pName = null,
        string eventID = "None",
        string memo = "",
        int itemBomb = 0,
        int itemRange = 0,
        int itemSpeed = 0,
        string areaStatus = "N-N-N")
    {
        // ID取得前に呼ばれた場合は無視
        if (!_isInitialized) return;

        string playerName = string.IsNullOrEmpty(pName) ? SystemInfo.deviceName : pName;

        // ★修正点1: GameTimer.instance から正確なタイマー値を取得
        float gameTime = 0f;
        if (GameTimer.instance != null)
        {
            gameTime = GameTimer.instance.CurrentTime;
        }

        // ★修正点2: ToString()によるフル精度での格納
        string[] row = new string[] {
            _sessionId,                                     // A: SessionID
            _currentStageName,                              // B: StageID
            DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), // C: RealTime
            gameTime.ToString(),                            // D: GameTime (GameTimerの生の値)
            playerName,                                     // E: PlayerID
            team,                                           // F: Team
            eventType,                                      // G: EventType
            eventID,                                        // H: EventID
            pos.x.ToString(),                               // I: PosX (フル精度)
            pos.z.ToString(),                               // J: PosZ (フル精度)
            itemBomb.ToString(),                            // K: Item_bomb
            itemRange.ToString(),                           // L: Item_Range
            itemSpeed.ToString(),                           // M: Item_Speed
            areaStatus,                                     // N: AreaStatus
            memo                                            // O: Memo
        };

        lock (_logBuffer) { _logBuffer.Add(row); }
    }

    private IEnumerator LogUploadRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(sendInterval);

            // IDが確定していない場合は送信をスキップ
            if (!_isInitialized) continue;

            List<string[]> dataToSend = null;
            lock (_logBuffer)
            {
                if (_logBuffer.Count > 0)
                {
                    dataToSend = new List<string[]>(_logBuffer);
                    _logBuffer.Clear();
                }
            }

            if (dataToSend != null)
            {
                yield return PostToGAS(dataToSend);
            }
        }
    }

    private IEnumerator PostToGAS(List<string[]> data)
    {
        string json = JsonHelper.ToJson(data);

        using (UnityWebRequest www = new UnityWebRequest(gasUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.LogWarning($"[GAS] 送信失敗: {www.error}");
            else
                Debug.Log($"[GAS] {data.Count}件のログを送信しました (ID: {_sessionId})");
        }
    }
}

// JSON変換用のヘルパークラス
public static class JsonHelper
{
    public static string ToJson(List<string[]> list)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("[");
        for (int i = 0; i < list.Count; i++)
        {
            sb.Append("[");
            for (int j = 0; j < list[i].Length; j++)
            {
                // エスケープ処理をしてダブルクォーテーションで囲む
                sb.Append("\"" + list[i][j].Replace("\"", "\\\"") + "\"");
                if (j < list[i].Length - 1) sb.Append(",");
            }
            sb.Append("]");
            if (i < list.Count - 1) sb.Append(",");
        }
        sb.Append("]");
        return sb.ToString();
    }
}