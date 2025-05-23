using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MapTimer : MonoBehaviour
{
    [SerializeField] private Image _mapTimerImage;   //タイマーとして使うImageをいれる
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
            Debug.LogError("GameTimer オブジェクトが見つかりません。タグを確認してください。");
        }

        _mapTimerImage.fillAmount = 1f; //Imageを全て表示させる

        // Resources フォルダからResultデータを読み込む
        _ResultData = Resources.Load<ResultData>("ResultData");
    }

    private void Update()
    {
        if (_gameTimer == null) return; //_gameTimerがなければ何もしない


        //初期時間と経過時間の割合をImageに反映
        if (!_gameTimer.IsMapTimer0())
        {
            float fillAmount = _gameTimer.MeltTimer();
            _mapTimerImage.fillAmount = fillAmount;
        }
        else
        {
            _mapTimerImage.fillAmount = 0f;

            //結果を保存
            _ResultData.TeamOneBloomPercent = BloomJudgement.Instance.GetTeamOneBloomPer();
            _ResultData.TeamTwoBloomPercent = BloomJudgement.Instance.GetTeamTwoBloomPer();
            EditorUtility.SetDirty(_ResultData);


            //resultへ移動
            FBSceneManager.Instance.LoadResultScene();
        }
    }
        
}
