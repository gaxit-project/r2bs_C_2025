using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BloomResult : MonoBehaviour
{
    public TextMeshProUGUI TeamOnePer;
    public TextMeshProUGUI TeamTwoPer;
    public TextMeshProUGUI TeamOneCnt;
    public TextMeshProUGUI TeamTwoCnt;
    public TextMeshProUGUI Winner;
    public Slider perSlider1;
    public Slider perSlider2;
    public Slider cntSlider1;
    public Slider cntSlider2;

    public GameObject handle1;
    public GameObject handle2;
    public GameObject handle3;
    public GameObject handle4;

    public GameObject judge;

    public GameObject ResultPanel;

    private int _blueTile;
    private int _redTile;
    private int sumTile;

    private int _blueArea;
    private int _redArea;
    private int sumArea;

    private bool _resultBool;

    /// <summary>
    /// Resultデータ（ScriptableObject）
    /// </summary>
    private ResultData _ResultData;
    void Start()
    {
        _ResultData = Resources.Load<ResultData>("ResultData");

#if !UNITY_EDITOR
    ResultDataIO.Load(_ResultData);
#endif


        _resultBool = DataBase.Instance.GetRssultBool();
        _blueTile = _ResultData.blueTile;
        _redTile = _ResultData.redTile;
        sumTile = _blueTile + _redTile;
        perSlider1.maxValue = sumTile;
        perSlider2.maxValue = sumTile;

        _blueArea = _ResultData.blueArea;
        _redArea = _ResultData.redArea;
        sumArea = _blueArea + _redArea;
        cntSlider1.maxValue = sumArea;
        cntSlider2.maxValue = sumArea;

        perSlider1.value = 0;
        perSlider2.value = 0;

        cntSlider1.value = 0;
        cntSlider2.value = 0;

        judge.SetActive(false);
        ResultPanel.SetActive(false);
        if (_resultBool)
        {
            perSlider1.gameObject.SetActive(false);
            perSlider2.gameObject.SetActive(false);
            TeamOnePer.gameObject.SetActive(false);
            TeamTwoPer.gameObject.SetActive(false);
        }
        else
        {
            cntSlider1.gameObject.SetActive(false);
            cntSlider2.gameObject.SetActive(false);
            TeamOneCnt.gameObject.SetActive(false);
            TeamTwoCnt.gameObject.SetActive(false);
        }

        StartCoroutine(ResultTimeline());
    }

    private IEnumerator ResultTimeline()
    {
        judge.SetActive(true);
        yield return new WaitForSeconds(2f);
        judge.SetActive(false);

        if (!_resultBool)
        {
            Coroutine cr1 = StartCoroutine(BlueTileS());
            Coroutine cr2 = StartCoroutine(RedTileS());

            yield return cr1; yield return cr2;

            if (_blueTile == _redTile)
            {
                handle1.SetActive(false);
                handle2.SetActive(false);
            }
            else if (_blueTile > _redTile)
            {
                handle2.SetActive(false);
            }
            else
            {
                handle1.SetActive(false);
            }
        }
        else
        {



            Coroutine cr3 = StartCoroutine(BlueAreaS());
            Coroutine cr4 = StartCoroutine(RedAreaS());



            yield return cr3; yield return cr4;

            if (_blueArea == _redArea)
            {
                handle3.SetActive(false);
                handle4.SetActive(false);
            }
            else if (_blueArea > _redArea)
            {
                handle4.SetActive(false);
            }
            else
            {
                handle3.SetActive(false);
            }
        }

        if (!_resultBool)
        {
            if (_blueTile == _redTile)
            {
                Winner.text = "Draw";
                Winner.color = Color.white;
            }
            else if (_blueTile > _redTile)
            {
                Winner.text = "TeamBlueWin";
                Winner.color = Color.blue;
            }
            else
            {
                Winner.text = "TeamRedWin";
                Winner.color = Color.red;
            }
        }

        //yield return new WaitForSeconds(3f);

        if (_resultBool)
        {
            if (_blueArea == _redArea)
            {
                Winner.text = "Draw";
                Winner.color = Color.white;
            }
            else if (_blueArea > _redArea)
            {
                Winner.text = "TeamBlueWin";
                Winner.color = Color.blue;
            }
            else
            {
                Winner.text = "TeamRedWin";
                Winner.color = Color.red;
            }
        }

        yield return new WaitForSeconds(1f);
        
        ResultPanel.SetActive(true);
        


    }

    public void OnSkip()
    {
        StopAllCoroutines();
        perSlider1.value = _blueTile;
        TeamOnePer.text = _blueTile.ToString();
        perSlider2.value = _redTile;
        TeamTwoPer.text = _redTile.ToString();
        cntSlider1.value = _blueArea;
        TeamOneCnt.text = _blueArea.ToString();
        cntSlider2.value = _redArea;
        TeamTwoCnt.text = _redArea.ToString();

        if (_blueArea == _redArea)
        {
            handle3.SetActive(false);
            handle4.SetActive(false);
            Winner.text = "Draw";
            Winner.color = Color.white;
        }
        else if (_blueArea > _redArea)
        {
            handle4.SetActive(false);
            Winner.text = "TeamBlueWin";
            Winner.color = Color.blue;
        }
        else
        {
            handle3.SetActive(false);
            Winner.text = "TeamRedWin";
            Winner.color = Color.red;
        }
        ResultPanel.SetActive(true);
    }

    private IEnumerator BlueTileS()
    {
        for (int i = 1; i <= _blueTile; i++)
        {
            perSlider1.value = i;
            TeamOnePer.text = i.ToString();
            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator RedTileS()
    {
        for (int i = 1; i <= _redTile; i++)
        {
            perSlider2.value = i;
            TeamTwoPer.text = i.ToString();
            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator BlueAreaS()
    {
        for (int i = 1; i <= _blueArea; i++)
        {
            cntSlider1.value = i;
            TeamOneCnt.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator RedAreaS()
    {
        for (int i = 1; i <= _redArea; i++)
        {
            cntSlider2.value = i;
            TeamTwoCnt.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
    }
}

        