using UnityEngine;
using System.Collections;

public class BGMGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(StartBGM());
    }

    IEnumerator StartBGM()
    {
        yield return new WaitForSeconds(3f);

        SoundManager.PlayBgm("ponkotsu");
    }

}
