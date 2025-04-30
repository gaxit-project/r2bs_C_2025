using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowTimerText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textTimer; //時間を表示する用
    [SerializeField] private GameTimer GameTimer; //ゲームタイマーを入れる用
    void Start()
    {
        GameTimer.StartTimer();
    }

    void Update()
    {
        float currentTime = GameTimer.CurrentTime;
        _textTimer.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
            (int)currentTime / 60,
            (int)currentTime % 60,
            (int)(currentTime * 100) % 100
            );
    }
}
