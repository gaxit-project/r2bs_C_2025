using UnityEngine;

public class FlowerSway : MonoBehaviour
{
    [Header("—h‚ê‚Ìİ’è")]
    public float swaySpeed = 2f;       // —h‚ê‚Ì‘¬‚³
    public float swayAmount = 10f;     // —h‚ê‚ÌU‚ê•
    public float swayOffset = 0f;      // —h‚ê‚Ìƒ^ƒCƒ~ƒ“ƒO

    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.localRotation;
        swayOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    void Update()
    {
        float swayX = Mathf.Sin(Time.time * swaySpeed + swayOffset) * swayAmount;
        float swayZ = Mathf.Cos(Time.time * swaySpeed + swayOffset) * (swayAmount * 0.3f);
        transform.localRotation = initialRotation * Quaternion.Euler(swayX, 0f, swayZ);
    }
}
