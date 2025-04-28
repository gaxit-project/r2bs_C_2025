using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowTimerText : MonoBehaviour
{
    public TextMeshProUGUI MytextTimer; //ŽžŠÔ‚ð•\Ž¦‚·‚é—p
    public GameTimer GameTimer;
    void Start()
    {
        GameTimer.StartTimer();
    }

    void Update()
    {
        MytextTimer.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
            (int)GameTimer.CurrentTime / 60,
            (int)GameTimer.CurrentTime % 60,
            (int)(GameTimer.CurrentTime * 100) % 100
            );
    }
}
