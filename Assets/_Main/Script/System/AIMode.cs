using UnityEngine;

public class AIMode : MonoBehaviour
{
    private PlayerTeamData _playerData;
    public void AIPlay()
    {
        _playerData = Resources.Load<PlayerTeamData>("PlayerData");
        _playerData.PlayerTable.Clear();
#if !UNITY_EDITOR
    PlayerDataIO.Reset();
#endif
        DataBase.Instance.SetAiMode(true);
        FBSceneManager.Instance.LoadMainScene();
    }

    public void AiOff()
    {
        DataBase.Instance.SetAiMode(false);
    }
}
