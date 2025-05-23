using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MainGameManager : MonoBehaviour
{
    private GameObject _posePanel;

    private bool isPose = false;

    public static MainGameManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _posePanel = this.transform.Find("PosePanel").gameObject;
        _posePanel.SetActive(false);
    }

    public void OnSwithPosw()
    {
        if (isPose) 
        {
            OnReStart();
        }
        else
        { 
            OnPose();
        }
    }

    public void OnPose()
    {
        GameTimer.instance.StopTimer();
        _posePanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        _posePanel.transform.Find("Back").GetComponent<Button>().Select();
        isPose = true;
    }

    public void OnReStart()
    {
        GameTimer.instance.StartTimer();
        _posePanel.SetActive(false);
        isPose = false;
    }

    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
