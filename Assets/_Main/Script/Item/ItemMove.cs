using UnityEngine;

public class ItemMove : MonoBehaviour
{
    [Header("振幅（上下の移動の幅）")]
    [SerializeField] private float amplitude = 0.4f;

    [Header("周期（1秒あたりの往復回数）")]
    [SerializeField] private float frequency = 0.7f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * frequency * 2f * Mathf.PI) * amplitude;
        transform.position = startPos + new Vector3(0f, yOffset, 0f);
    }
}