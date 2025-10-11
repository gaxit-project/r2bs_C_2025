using UnityEngine;

public class AIMode : MonoBehaviour
{
    private PlayerTeamData _playerData;
    public void AIPlay()
    {
        _playerData = Resources.Load<PlayerTeamData>("PlayerData");
        _playerData.PlayerTable.Clear();
        FBSceneManager.Instance.LoadMainScene();
        DataBase.Instance.SetAiMode(true);
    }

    public void AiOff()
    {
        DataBase.Instance.SetAiMode(false);
    }
}
