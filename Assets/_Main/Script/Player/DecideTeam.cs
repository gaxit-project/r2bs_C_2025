using UnityEngine;

public class DecideTeam : PlayerBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TeamDecideOne()
    {
        TeamName = Team.TeamOne;
        transform.parent.tag = "TeamOne";
    }
    public void TeamDecideTwo()
    {
        TeamName = Team.TeamTwo;
        transform.parent.tag = "TeamTwo";
    }
}
