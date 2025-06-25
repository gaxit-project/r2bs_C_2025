using UnityEngine;


public class AreaStage : MonoBehaviour
{
    [SerializeField] private GameObject _Area1;
    [SerializeField] private GameObject _Area2;

    [SerializeField] private Vector3 _start1;
    [SerializeField] private Vector3 _start2;
    [SerializeField] private Vector3 _end1;
    [SerializeField] private Vector3 _end2;


    // Update is called once per frame
    void Update()
    {
        _Area1.transform.position = Vector3.Lerp(_end1, _start1, GameTimer.instance.MeltTimer());
        _Area2.transform.position = Vector3.Lerp(_end2, _start2, GameTimer.instance.MeltTimer());
    }
}
