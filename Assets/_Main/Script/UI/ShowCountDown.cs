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
            Debug.LogError("GameTimer �I�u�W�F�N�g��������܂���B�^�O���m�F���Ă��������B");
        }

        GameObject textObj = GameObject.Find("Timer");
        if (textObj != null)
        {
            _textTimer = textObj.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("Timer �I�u�W�F�N�g��������܂���B���O���m�F���Ă��������B");
        }
    }

    private void Update()
    {
        if (_gameTimer == null || _textTimer == null) return; //_gameTimer��_textTimer���Ȃ������牽�����Ȃ�
        
        //0�b��\������������Go��\��������,����ȊO�͕b����\��
        if (_gameTimer.CountDownTime <= 0.5f)
        {
            _textTimer.text = "GO";
        }
        else
        {
            _textTimer.text = _gameTimer.CountDownTime.ToString("F0");
        }

        //Go��\�������炱�̃I�u�W�F�N�g������
        if (_gameTimer.CountDownTime < -1)
        {
            _gameTimer.StartTimer();
            _gameTimer.MainGameStart();
            Destroy(gameObject);
        }
    }
}