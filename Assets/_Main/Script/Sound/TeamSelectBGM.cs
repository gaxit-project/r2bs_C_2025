using UnityEngine;
using System.Collections;

public class TeamSelectBGM : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundManager.PlayBgm("StageSelect");
    }
}
