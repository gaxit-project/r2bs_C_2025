using UnityEngine;
using UnityEngine.UI;

public class MapTimer : MonoBehaviour
{
    [SerializeField] private float _startTime = 300; //制限時間
    [SerializeField] private Image _mapTimerImage;   //タイマーとして使うImageをいれる

    private float timeLeft; //経過時間
    private GameTimer _gameTimer;



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

        timeLeft = _startTime; 
        _mapTimerImage.fillAmount = 1f; //Imageを全て表示させる

        _gameTimer.StartTimer(); //デバック用！！！！！！！！！！！！！！！！！！！！！

    }

    // Update is called once per frame
    void Update()
    {
        if (_gameTimer == null) return; //_gameTimerがなければ何もしない

        //初期時間と経過時間の割合をImageに反映
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            float fillAmount = timeLeft / _startTime;
            _mapTimerImage.fillAmount = fillAmount;
        }
        else
        {
            _mapTimerImage.fillAmount = 0f;

            //タイマーが0になったらここに処理を書く!!!!!!!!!!!!!!!!!!!!
        }
    }
        
}
