using UnityEngine;
using System.Collections;

public class TitleBGM : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundManager.PlayBgm("cook");
    }
}
