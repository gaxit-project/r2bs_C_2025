#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

public class MapTimer : MonoBehaviour
{
    [SerializeField] private Image _mapTimerImage;   //�^�C�}�[�Ƃ��Ďg��Image�������
    private GameTimer _gameTimer;
    private ResultData _ResultData;

    private void Start()
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

        // Resources �t�H���_����Result�f�[�^��ǂݍ���
        _ResultData = Resources.Load<ResultData>("ResultData");
    }

    private void Update()
    {
        if (_gameTimer == null) return;

        if (!_gameTimer.IsMapTimer0())
        {
            float fillAmount = _gameTimer.MeltTimer();
            _mapTimerImage.fillAmount = fillAmount;
        }
        else
        {
            _mapTimerImage.fillAmount = 0f;

            // ���ʂ�ۑ�
            _ResultData.TeamOneBloomPercent = BloomJudgement.Instance.GetTeamOneBloomPer();
            _ResultData.TeamTwoBloomPercent = BloomJudgement.Instance.GetTeamTwoBloomPer();

#if UNITY_EDITOR
            EditorUtility.SetDirty(_ResultData);
#else
        ResultDataIO.Save(_ResultData);
#endif

            // ���U���g�V�[���ֈړ�
            FBSceneManager.Instance.LoadResultScene();
        }
    }
}
