
using UnityEngine;
using UnityEngine.SceneManagement;


public class PitariSceneManager : MonoBehaviour
{
    public static PitariSceneManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void ToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void ToResult()
    {
        SceneManager.LoadScene("ResultScene");
    }

    public void ToMain()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ToTeamSelect()
    {
        SceneManager.LoadScene("TeamSelectScene");
    }


}
