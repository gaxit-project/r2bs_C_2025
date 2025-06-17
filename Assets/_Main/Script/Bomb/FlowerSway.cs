using UnityEngine;

public class FlowerSway : MonoBehaviour
{
    private float _swaySpeed = 2f;       // —h‚ê‚Ì‘¬‚³
    private float _swayAmount = 10f;     // —h‚ê‚ÌU‚ê•
    private float _swayOffset = 0f;      // —h‚ê‚Ìƒ^ƒCƒ~ƒ“ƒO

    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.localRotation;
        _swayOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    void Update()
    {
        // ‰Ô‚ğ—h‚ç‚·ˆ—
        float swayX = Mathf.Sin(Time.time * _swaySpeed + _swayOffset) * _swayAmount;
        float swayZ = Mathf.Cos(Time.time * _swaySpeed + _swayOffset) * (_swayAmount * 0.3f);
        transform.localRotation = initialRotation * Quaternion.Euler(swayX, 0f, swayZ);
    }
}
