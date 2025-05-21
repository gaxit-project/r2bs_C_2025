using UnityEngine;
using UnityEngine.UI;

public class MapTimer : MonoBehaviour
{
    [SerializeField] private Image _mapTimerImage;   //�^�C�}�[�Ƃ��Ďg��Image�������
    private GameTimer _gameTimer;

    void Start()
    {
        
        if (GameObject.FindWithTag("GameTimer"))
        {
            _gameTimer = GameObject.FindWithTag("GameTimer").GetComponent<GameTimer>();
        }
        else
        {
            Debug.LogError("GameTimer �I�u�W�F�N�g��������܂���B�^�O���m�F���Ă��������B");
        }

        _mapTimerImage.fillAmount = 1f; //Image��S�ĕ\��������
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameTimer == null) return; //_gameTimer���Ȃ���Ή������Ȃ�


        //�������Ԃƌo�ߎ��Ԃ̊�����Image�ɔ��f
        if (!_gameTimer.IsMapTimer0())
        {
            float fillAmount = _gameTimer.MeltTimer();
            _mapTimerImage.fillAmount = fillAmount;
        }
        else
        {
            _mapTimerImage.fillAmount = 0f;
            //�^�C�}�[��0�ɂȂ����炱���ɏ���������!!!!!!!!!!!!!!!!!!!!
        }
    }
        
}
