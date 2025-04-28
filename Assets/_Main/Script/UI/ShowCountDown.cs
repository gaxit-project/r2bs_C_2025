using TMPro;
using UnityEngine;

public class ShowCountDown : MonoBehaviour
{
    [SerializeField] private float _startTime;
    [SerializeField] private string _strFormat; //
    [SerializeField] private GameTimer _gameTimer;
    [SerializeField] private TextMeshProUGUI _mytextTimer; //ŽžŠÔ‚ð•\Ž¦‚·‚é—p

    private void Update()
    {
        float showTime = Mathf.Clamp(_startTime - _gameTimer.CurrentTime, 0f, _startTime);
        _mytextTimer.text = string.Format(_strFormat, showTime);
    }
}
