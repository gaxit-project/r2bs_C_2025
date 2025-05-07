using UnityEngine;

public class CanvasToggle : MonoBehaviour
{
    // ���삵����Canvas�iCanvas���A�^�b�`���ꂽGameObject�j���C���X�y�N�^�[����w��
    [SerializeField] private GameObject targetCanvas;

    // Canvas��\������
    public void ShowCanvas()
    {
        if (targetCanvas != null)
            targetCanvas.SetActive(true);
    }

    // Canvas���\���ɂ���
    public void HideCanvas()
    {
        if (targetCanvas != null)
            targetCanvas.SetActive(false);
    }

    // �\��/��\�����g�O���؂�ւ�
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