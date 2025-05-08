using UnityEngine;

public class MapTimer : MonoBehaviour
{
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

        _gameTimer.StartTimer(); //デバック用！！！！！！！！！！！！！！！！！！！！！

    }

    // Update is called once per frame
    void Update()
    {

    }
}
