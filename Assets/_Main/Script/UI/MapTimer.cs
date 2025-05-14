using UnityEngine;
using UnityEngine.UI;

public class MapTimer : MonoBehaviour
{
    [SerializeField] private float _startTime = 300; //��������
    [SerializeField] private Image _mapTimerImage;   //�^�C�}�[�Ƃ��Ďg��Image�������

    private float timeLeft; //�o�ߎ���
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

        timeLeft = _startTime; 
        _mapTimerImage.fillAmount = 1f; //Image��S�ĕ\��������

        _gameTimer.StartTimer(); //�f�o�b�N�p�I�I�I�I�I�I�I�I�I�I�I�I�I�I�I�I�I�I�I�I�I

    }

    // Update is called once per frame
    void Update()
    {
        if (_gameTimer == null) return; //_gameTimer���Ȃ���Ή������Ȃ�

        //�������Ԃƌo�ߎ��Ԃ̊�����Image�ɔ��f
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            float fillAmount = timeLeft / _startTime;
            _mapTimerImage.fillAmount = fillAmount;
        }
        else
        {
            _mapTimerImage.fillAmount = 0f;

            //�^�C�}�[��0�ɂȂ����炱���ɏ���������!!!!!!!!!!!!!!!!!!!!
        }
    }
        
}
