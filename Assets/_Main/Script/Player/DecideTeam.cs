using UnityEngine;

public class DecideTeam : MonoBehaviour
{
    [SerializeField] PlayerController PC = null;  // プレイヤーコントローラーを入れる変数

    private void Start()
    {
        PC = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TeamDecideOne()
    {
        PC.ChangeTeam(Team.TeamOne);
    }
    public void TeamDecideTwo()
    {
        PC.ChangeTeam(Team.TeamTwo);
    }
}
