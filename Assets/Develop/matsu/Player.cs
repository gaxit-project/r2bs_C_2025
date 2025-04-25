using UnityEngine;
using static MapManager;

public class Player : PlayerBase
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            MapBlockData a = CatchPlayerPos();
            BombPlacement(a);
        }
    }
}
