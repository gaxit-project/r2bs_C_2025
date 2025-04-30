using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowTimerText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textTimer; //ŽžŠÔ‚ð•\Ž¦‚·‚é—p
    [SerializeField] private GameTimer GameTimer;
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
