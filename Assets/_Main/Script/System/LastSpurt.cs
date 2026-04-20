using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LastSpurt : MonoBehaviour
{
    [SerializeField] GameObject lastUI;
    [SerializeField] GameObject lastUI2;

    void Start()
    {
        lastUI.SetActive(true);
        //lastUI2.SetActive(true);
        // ¡‚ÌƒƒCƒ“‚ÌBGM‚ğ~‚ß‚é
        // ‰¹‚ÆSE‚ğ‚±‚±‚É‚¢‚ê‚é

        StartCoroutine(StopLastUI());
    }

    IEnumerator StopLastUI()
    {
        yield return new WaitForSeconds(3f);
        lastUI.SetActive(false);
        //lastUI2.SetActive(false);
    }
}
