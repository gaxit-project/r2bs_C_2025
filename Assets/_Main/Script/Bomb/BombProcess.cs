using UnityEngine;

public class BombProcess
{
    public static BombProcess Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void BombSetting(int BombRange)
    {

    }
}
