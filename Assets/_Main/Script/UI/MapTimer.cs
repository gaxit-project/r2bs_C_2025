#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MapTimer : MonoBehaviour
{
    [SerializeField] private Image _mapTimerImage;   //タイマーとして使うImageをいれる
    private GameTimer _gameTimer;
    private ResultData _ResultData;
    private bool oneBool = false;

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

        StartCoroutine(PreTimer()); //Imageを全て表示させる

        // Resources フォルダからResultデータを読み込む
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

            // 結果を保存
            _ResultData.blueTile = BloomJudgement.Instance.GetTeamOneBloomPer();
            _ResultData.redTile = BloomJudgement.Instance.GetTeamTwoBloomPer();

            _ResultData.blueArea = GatiArea.Instance.GetGatiArea(Team.TeamOne);
            _ResultData.redArea = GatiArea.Instance.GetGatiArea(Team.TeamTwo);
            if (!oneBool)
            {
                if (GatiArea.Instance.GetGatiArea(Team.TeamOne) > GatiArea.Instance.GetGatiArea(Team.TeamTwo))
                {
                    GameObject[] p12 = GameObject.FindGameObjectsWithTag("TeamOne");
                    foreach (GameObject pb in p12)
                    {
                        pb.gameObject.GetComponent<PlayerBase>().addReward(DataBase.Instance.win);
                    }
                    GameObject[] p34 = GameObject.FindGameObjectsWithTag("TeamTwo");
                    foreach (GameObject pb in p34)
                    {
                        pb.gameObject.GetComponent<PlayerBase>().addReward(DataBase.Instance.lose);
                    }
                }
                else if (GatiArea.Instance.GetGatiArea(Team.TeamOne) < GatiArea.Instance.GetGatiArea(Team.TeamTwo))
                {
                    GameObject[] p12 = GameObject.FindGameObjectsWithTag("TeamTwo");
                    foreach (GameObject pb in p12)
                    {
                        pb.gameObject.GetComponent<PlayerBase>().addReward(DataBase.Instance.win);
                    }
                    GameObject[] p34 = GameObject.FindGameObjectsWithTag("TeamOne");
                    foreach (GameObject pb in p34)
                    {
                        pb.gameObject.GetComponent<PlayerBase>().addReward(DataBase.Instance.lose);
                    }
                }
                oneBool = true;
            }

#if UNITY_EDITOR
                EditorUtility.SetDirty(_ResultData);
#else
        ResultDataIO.Save(_ResultData);
#endif

            // リザルトシーンへ移動
            FBSceneManager.Instance.LoadResultScene();
        }
    }

    private IEnumerator PreTimer()
    {
        for (int i = 0; i < GameTimer.instance.StartTime; i++)
        {
            _mapTimerImage.fillAmount = i / GameTimer.instance.StartTime;
            yield return new WaitForSeconds(0.001f);
        }
    }
}
