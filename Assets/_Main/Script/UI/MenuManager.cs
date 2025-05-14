using UnityEngine;
public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject TitlePanel;　//タイトルのパネル
    [SerializeField]
    private GameObject OptionPanel;　//オプションのパネル

    private void Start()
    {
        TitlePanel.SetActive(true);
        OptionPanel.SetActive(false);
    }

    public void OnStart()　//TeamSelectSceneへ
    {
        PitariSceneManager.Instance.ToTeamSelect();
    }

    public void OnOption()
    {
        TitlePanel.SetActive(false);
        OptionPanel.SetActive(true);
    }

    public void OnBack()
    {
        TitlePanel.SetActive(true);
        OptionPanel.SetActive(false);
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