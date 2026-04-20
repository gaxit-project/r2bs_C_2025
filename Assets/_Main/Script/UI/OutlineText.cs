using TMPro;
using UnityEngine;

public class OutlineText : MonoBehaviour
{
    public TextMeshProUGUI tmp;

    void Start()
    {
        tmp.outlineWidth = 0.1f;
        tmp.outlineColor = Color.white;
    }
}
