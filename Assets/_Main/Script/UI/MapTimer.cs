using UnityEngine;
using UnityEngine.UI;

public class MapTimer : MonoBehaviour
{
    [SerializeField] private float _startTime = 300;

    private GameTimer _gameTimer;
    private Image _mapTimerImage;
    private float timeLeft; 



    void Start()
    {
        if (GameObject.FindWithTag("GameTimer"))
        {
            _gameTimer = GameObject.FindWithTag("GameTimer").GetComponent<GameTimer>();
        }
        else
        {
            Debug.LogError("GameTimer オブジェクトが見つかりません。タグを確認してください。");
        }

        if (GameObject.FindWithTag("MapTimer"))
        {
            _mapTimerImage = GameObject.FindWithTag("MapTimer").GetComponent<Image>();
        }
        else
        {
            Debug.LogError("MapTimer オブジェクトが見つかりません。タグを確認してください。");
        }

        timeLeft = _startTime;
        _mapTimerImage.fillAmount = 1f;

        _gameTimer.StartTimer(); //デバック用！！！！！！！！！！！！！！！！！！！！！

    }

    // Update is called once per frame
    void Update()
    {
        if (_gameTimer == null || _mapTimerImage == null) return;

        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            float fillAmount = timeLeft / _startTime;
            _mapTimerImage.fillAmount = fillAmount;

            //_mapTimerImage.color = Color.Lerp(Color.red, Color.green, fillAmount);
        }
        else
        {
            _mapTimerImage.fillAmount = 0f;

            //タイマーが0になったらここに処理を書く!!!!!!!!!!!!!!!!!!!!
        }
    }
        
}
