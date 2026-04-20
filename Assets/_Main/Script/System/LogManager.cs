using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LogManager : MonoBehaviour
{
    public static LogManager Instance { get; private set; }

    [Header("Google Form Settings")]
    //[SerializeField] private string formUrl = "https://docs.google.com/forms/d/e/1FAIpQLSfndf_iaiPlam5PVUFU7mW7ZA56byvDz_lF8X9vOeV9w0rgyA/viewform?usp=pp_url&entry.211133840=1&entry.1299904890=2&entry.1955613585=3&entry.1195631167=4&entry.2064542295=5&entry.742839594=6&entry.852486000=7&entry.1751466862=8&entry.671341531=9";
    [SerializeField] private string formUrl = "https://docs.google.com/forms/d/e/1FAIpQLSfndf_iaiPlam5PVUFU7mW7ZA56byvDz_lF8X9vOeV9w0rgyA/viewform?usp=dialog/formResponse";

    // 各項目の entry.xxxx を設定
    private const string EntrySessionId = "entry.211133840";
    private const string EntryTimestamp = "entry.1299904890";
    private const string EntryPlayerName = "entry.1955613585";
    private const string EntryTeam = "entry.1195631167";
    private const string EntryEventType = "entry.2064542295";
    private const string EntryPosX = "entry.742839594";
    private const string EntryPosY = "entry.852486000";
    private const string EntryPosZ = "entry.1751466862";
    private const string EntryContext = "entry.671341531";

    private string _sessionId;

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }

        // 試合開始時に一度だけUUIDを生成
        _sessionId = Guid.NewGuid().ToString();
    }
    private void Start()
    {
        SendLog("test", "asdf");
    }
    /// <summary>
    /// ログを送信するメイン関数
    /// </summary>
    public void SendLog(string eventType, string context = "")
    {
        StartCoroutine(PostRoutine(eventType, context));
    }

    private IEnumerator PostRoutine(string eventType, string context)
    {
        Vector3 pos = GameObject.FindGameObjectWithTag("Player").transform.position; // 暫定的な取得

        WWWForm form = new WWWForm();
        form.AddField(EntrySessionId, _sessionId);
        form.AddField(EntryTimestamp, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"));
        form.AddField(EntryPlayerName, SystemInfo.deviceName); // PC名をプレイヤー名として利用
        form.AddField(EntryTeam, "Red"); // 実際はプレイヤーの所属チームを渡す
        form.AddField(EntryEventType, eventType);
        form.AddField(EntryPosX, $"{pos.x:F2}");
        form.AddField(EntryPosY, $"{pos.y:F2}");
        form.AddField(EntryPosZ, $"{pos.z:F2}");
        form.AddField(EntryContext, context);

        using (UnityWebRequest www = UnityWebRequest.Post(formUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning($"[Log] 送信失敗: {www.error}");
            }
        }
    }
}