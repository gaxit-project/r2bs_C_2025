using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private float _myTimer; //プライベートタイマー
    public float CurrentTime { get { return _myTimer; } } //現在の時間を返す(読み取り専用)

    public bool _activeTime = false; //タイマーを進めるかどうか判断する

    private void Update()
    {
        if (_activeTime)
        {
            _myTimer += Time.deltaTime;
        }
    }

    /// <summary>
    /// タイマーを動かす
    /// </summary>
    public void StartTimer()
    {
        _activeTime = true;
    }

    /// <summary>
    /// タイマーを止める
    /// </summary>
    public void StopTimer()
    {
        _activeTime = false;
    }

    /// <summary>
    /// タイマーをリセットする
    /// </summary>
    public void ResetTimer()
    {
        _myTimer = 0f;
        StopTimer();
    }
}