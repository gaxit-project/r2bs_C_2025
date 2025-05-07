using UnityEngine;

public class CanvasToggle : MonoBehaviour
{
    // 操作したいCanvas（CanvasがアタッチされたGameObject）をインスペクターから指定
    [SerializeField] private GameObject targetCanvas;

    // Canvasを表示する
    public void ShowCanvas()
    {
        if (targetCanvas != null)
            targetCanvas.SetActive(true);
    }

    // Canvasを非表示にする
    public void HideCanvas()
    {
        if (targetCanvas != null)
            targetCanvas.SetActive(false);
    }

    // 表示/非表示をトグル切り替え
    public void ToggleCanvas()
    {
        if (targetCanvas != null)
            targetCanvas.SetActive(!targetCanvas.activeSelf);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            ToggleCanvas();
        }
    }
}