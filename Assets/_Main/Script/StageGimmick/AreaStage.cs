using UnityEngine;


public class AreaStage : MonoBehaviour
{
    [SerializeField] private GameObject _Area1;
    [SerializeField] private GameObject _Area2;

    [SerializeField] private Vector3 _start1;
    [SerializeField] private Vector3 _start2;
    [SerializeField] private Vector3 _end1;
    [SerializeField] private Vector3 _end2;

    private void Start()
    {
        if (DataBase.Instance.GetAreaBool())
        {
            _Area1.SetActive(true);
            _Area2.SetActive(true);
        }
        else
        {
            _Area1.SetActive(false);
            _Area2.SetActive(false);
        }
    }

    private void Update()
    {
        if (DataBase.Instance.GetAreaBool())
        {
            _Area1.transform.position = Vector3.Lerp(_end1, _start1, GameTimer.instance.meltTime);
            _Area2.transform.position = Vector3.Lerp(_end2, _start2, GameTimer.instance.meltTime);
        }
    }
}
