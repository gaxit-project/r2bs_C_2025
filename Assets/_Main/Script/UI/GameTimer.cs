using TMPro.EditorUtilities;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private bool _isActiveTime = false; //タイマーを進めるかどうか判断する
    [SerializeField] private float _startTime = 300; //制限時間
    [SerializeField] private float _countDownTime = 3; //プレイ開始時のカウントダウン用

    private float _timer; //プライベートタイマー
    private float _mapTimer; //カウントダウン用のタイマー
    public float CurrentTime { get { return _timer; } } //現在の時間を返す(読み取り専用)
    public float CountDownTime { get { return _countDownTime; } }

    private void Awake()
    {
        _mapTimer = _startTime; //マップタイマーの初期値を代入
    }
    private void Update()
    {
        _countDownTime -= Time.deltaTime;

        if (_isActiveTime)
        {
            _timer += Time.deltaTime;
        }
    }

    /// <summary>
    /// タイマーを動かす
    /// </summary>
    public void StartTimer()
    {
        _isActiveTime = true;
    }

    /// <summary>
    /// タイマーを止める
    /// </summary>
    public void StopTimer()
    {
        _isActiveTime = false;
    }

    /// <summary>
    /// タイマーをリセットする
    /// </summary>
    public void ResetTimer()
    {
        _timer = 0f;
        StopTimer();
    }

    /// <summary>
    /// マップタイマーの減ってる比率を返す
    /// </summary>
    public float MeltTimer()
    {
        if (_isActiveTime) { 
            _mapTimer -= Time.deltaTime;
        }

        return _mapTimer / _startTime;
    }

    /// <summary>
    /// マップタイマーの状態を取得
    /// </summary>
    /// <returns></returns>
    public bool IsMapTimer0()
    {
        if(_mapTimer <= 0)
        {
            _mapTimer = 0;
            return true;
        }

        return false;
    }
}