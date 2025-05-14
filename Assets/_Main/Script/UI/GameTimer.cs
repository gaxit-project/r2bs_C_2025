using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private float _timer; //プライベートタイマー
    public float CurrentTime { get { return _timer; } } //現在の時間を返す(読み取り専用)

    [SerializeField] private bool _isActiveTime = false; //タイマーを進めるかどうか判断する

    private void Update()
    {
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
}