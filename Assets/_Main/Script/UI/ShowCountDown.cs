using TMPro;
using UnityEngine;

public class ShowCountDown : MonoBehaviour
{
    [SerializeField] private float _startTime = 300;

    private TextMeshProUGUI _textTimer;
    private GameTimer _gameTimer;
    private string _strFormat = "{0:000}";

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

        float showTime = Mathf.Clamp(_startTime - _gameTimer.CurrentTime, 0f, _startTime);
        _textTimer.text = string.Format(_strFormat, showTime);
    }
}