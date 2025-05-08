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
            Debug.LogError("GameTimer �I�u�W�F�N�g��������܂���B�^�O���m�F���Ă��������B");
        }

        _gameTimer.StartTimer(); //�f�o�b�N�p�I�I�I�I�I�I�I�I�I�I�I�I�I�I�I�I�I�I�I�I�I

    }

    // Update is called once per frame
    void Update()
    {

    }
}
