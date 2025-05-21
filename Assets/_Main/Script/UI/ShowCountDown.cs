using TMPro;
using UnityEngine;

public class ShowCountDown : MonoBehaviour
{

    private TextMeshProUGUI _textTimer;
    private GameTimer _gameTimer;

    private void Start()
    {
        GameObject timerObj = GameObject.FindWithTag("GameTimer");
        if (timerObj != null)
        {
            _gameTimer = timerObj.GetComponent<GameTimer>();
        }
        else
        {
            Debug.LogError("GameTimer オブジェクトが見つかりません。タグを確認してください。");
        }

        GameObject textObj = GameObject.Find("Timer");
        if (textObj != null)
        {
            _textTimer = textObj.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("Timer オブジェクトが見つかりません。名前を確認してください。");
        }
    }

    private void Update()
    {
        if (_gameTimer == null || _textTimer == null) return;

        if (_gameTimer.CountDownTime <= 0.5f)
        {
            _textTimer.text = "GO";
        }
        else
        {
            _textTimer.text = _gameTimer.CountDownTime.ToString("F0");
        }
        if (_gameTimer.CountDownTime < -1)
        {
            _gameTimer.StartTimer();
            _gameTimer.MainGameStart();
            Destroy(gameObject);
        }
    }
}